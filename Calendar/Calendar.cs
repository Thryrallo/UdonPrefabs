using System;
using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;

namespace Thry.Udon.Calendar
{
    public class Calendar : UdonSharpBehaviour
    {
        public GameObject HolidayPrefab;
        public Transform DayParent;
        public Transform HolidayParent;
        public TMPro.TextMeshProUGUI MonthText;
        public TMPro.TextMeshProUGUI TimeText;
        public GameObject ButtonToday;
        public TextAsset HolidaysData;
        public UnityEngine.UI.VerticalLayoutGroup ContainerLayout;
        public Transform DaysPool;

        const float UPDATE_RATE = 1;

        int _currentYear;
        int _currentMonth;
        int _currentDay;

        int _displayYear;
        int _displayMonth;

        float _lastUpdate;
        bool _showDots;

        string[] _holidayDates;
        string[] _holidaysNames;

        Day _selectedDay;

        float GRID_HEIGHT = 100;
        float GRID_SPACE = 10;

        private void Start() 
        {
            if(HolidaysData != null)
            {
                string[] lines = HolidaysData.text.Split('\n');
                _holidayDates = new string[lines.Length];
                _holidaysNames = new string[lines.Length];
                for(int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(';');
                    _holidayDates[i] = parts[0];
                    _holidaysNames[i] = parts[1];
                }
            }
        }

        void SetupCalendar(int selectDay)
        {
            // Clear the calendar
            for(int i = DayParent.childCount - 1; i >= 7; i--)
            {
                Transform t = DayParent.GetChild(i);
                t.gameObject.SetActive(false);
                t.SetParent(DaysPool);
            }
            // Get the first day of the month
            System.DateTime firstDayOfMonth = new System.DateTime(_displayYear, _displayMonth, 1);
            // Get the current day
            System.DateTime now = System.DateTime.Now;
            // Get the last day of the month
            System.DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            // Get the first day of the week
            System.DateTime firstDayOfWeek = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
            // Get the last day of the week
            System.DateTime lastDayOfWeek = lastDayOfMonth.AddDays(6 - (int)lastDayOfMonth.DayOfWeek);
            // Set the month text
            MonthText.text = firstDayOfMonth.ToString("MMMM yyyy");
            // Create the days
            for (System.DateTime day = firstDayOfWeek; day <= lastDayOfWeek; day = day.AddDays(1))
            {
                string id = day.ToString("yyyy-MM-dd");
                bool isHoliday = false;
                if(_holidayDates != null)
                {
                    isHoliday = Array.IndexOf(_holidayDates, id) != -1;
                }
                GameObject go = DaysPool.GetChild(0).gameObject;
                go.transform.SetParent(DayParent);
                go.SetActive(true);
                Day d = go.GetComponent<Day>();
                d.Setup(day, day.Day == now.Day && day.Month == now.Month && day.Year == now.Year, day.Month == firstDayOfMonth.Month, id, isHoliday);
                // Day d = DayPrefab.Create(DayParent.gameObject, day, day.Day == now.Day && day.Month == now.Month && day.Year == now.Year, day.Month == firstDayOfMonth.Month, id, isHoliday);
                if(selectDay == day.Day && day.Month == _displayMonth)
                {
                    SelectDay(d);
                }
            }

            ButtonToday.SetActive(now.Month != _displayMonth || now.Year != _displayYear);

            RectTransform rect = DayParent.GetComponent<RectTransform>();
            Vector2 size = rect.sizeDelta;
            size.y = (int)(Math.Ceiling((lastDayOfWeek - firstDayOfWeek).TotalDays / 7 + 1) * (GRID_HEIGHT + GRID_SPACE)) + GRID_SPACE;
            rect.sizeDelta = size;
        }

        void Update() 
        {
            if(Time.time - _lastUpdate > UPDATE_RATE)
            {
                _lastUpdate = Time.time;
                _showDots = !_showDots;
                System.DateTime now = System.DateTime.Now;
                TimeText.text = now.ToString("HH" + (_showDots ? ":" : " ") + "mm");
                if(now.Year != _currentYear || now.Month != _currentMonth || now.Day != _currentDay)
                {
                    if(_displayYear == _currentYear && _displayMonth == _currentMonth)
                    {
                        _displayMonth = now.Month;
                        _displayYear = now.Year;
                        SetupCalendar(now.Day);
                    }else
                    {
                        ButtonToday.SetActive(now.Month != _displayMonth || now.Year != _displayYear);
                    }
                    _currentYear = now.Year;
                    _currentMonth = now.Month;
                    _currentDay = now.Day;
                }
            }
        }

        public void Next()
        {
            _displayMonth++;
            if(_displayMonth > 12)
            {
                _displayMonth = 1;
                _displayYear++;
            }
            Deselect();
            SetupCalendar(-1);
        }

        public void Previous()
        {
            _displayMonth--;
            if(_displayMonth < 1)
            {
                _displayMonth = 12;
                _displayYear--;
            }
            Deselect();
            SetupCalendar(-1);
        }

        public void Today()
        {
            System.DateTime now = System.DateTime.Now;
            _displayMonth = now.Month;
            _displayYear = now.Year;
            SetupCalendar(now.Day);
        }

        void Deselect()
        {
            if(_selectedDay != null)
            {
                _selectedDay.SetSelected(false);
                // Remove all holidays
                for(int i = HolidayParent.childCount - 1; i >= 2; i--)
                {
                    Destroy(HolidayParent.GetChild(i).gameObject);
                }
            }
        }

        public void SelectDay(Day day)
        {
            // Go to the month of the selected day if it's not the current month
            if(day.Date.Month != _displayMonth)
            {
                _displayMonth = day.Date.Month;
                _displayYear = day.Date.Year;
                SetupCalendar(day.Date.Day);
                return;
            }

            Deselect();

            _selectedDay = day;
            day.SetSelected(true);
            if(day.IsHoliday)
            {
                int index = Array.IndexOf(_holidayDates, day.Id);
                if(index != -1)
                {
                    string[] holidays = _holidaysNames[index].Split('&');
                    for(int i = 0; i < holidays.Length; i++)
                    {
                        GameObject holiday = Instantiate(HolidayPrefab, HolidayParent);
                        holiday.SetActive(true);
                        holiday.GetComponent<TMPro.TextMeshProUGUI>().text = holidays[i];
                    }
                }
            }
        }
    }
}