﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UdonSharp;
using UnityEngine;

namespace Thry.Udon.Calendar
{
    public class Calendar : UdonSharpBehaviour
    {
        [Header("Customization")]
        public TextAsset HolidaysData;
        public TextAsset BirthdaysData;


        [Header("References")]
        public GameObject HolidayPrefab;
        public Transform DayParent;
        public GameObject DayPrefab;
        public Transform HolidayParent;
        public TMPro.TextMeshProUGUI MonthText;
        public TMPro.TextMeshProUGUI TimeText;
        public GameObject ButtonToday;
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

        string[] _specificDates;
        string[] _specificData;

        string[] _yearlyDates;
        string[] _yearlyData;
        string[] _weeklyData = new string[] { "", "", "", "", "", "", "" };

        Day _selectedDay;

        float GRID_HEIGHT = 100;
        float GRID_SPACE = 10;

        void AddEntry(DateTime day, string data, bool isYearly, bool isWeekly)
        {
            if(isYearly)
            {
                string dateString = day.ToString("MM-dd");
                int index = Array.IndexOf(_yearlyDates, dateString);
                if(index != -1)
                {
                    _yearlyData[index] += ";" + data;
                }else
                {
                    _yearlyDates[_yearlyIndex] = dateString;
                    _yearlyData[_yearlyIndex] = data;
                    _yearlyIndex++;
                    if(_yearlyDates.Length == _yearlyIndex)
                    {
                        string[] newDates = new string[_yearlyDates.Length * 2];
                        string[] newData = new string[_yearlyData.Length * 2];
                        Array.Copy(_yearlyDates, newDates, _yearlyDates.Length);
                        Array.Copy(_yearlyData, newData, _yearlyData.Length);
                        _yearlyDates = newDates;
                        _yearlyData = newData;
                    }
                }
            }else if(isWeekly)
            {
                if(string.IsNullOrEmpty(_weeklyData[(int)day.DayOfWeek]))
                {
                    _weeklyData[(int)day.DayOfWeek] = data;
                }else
                {
                    _weeklyData[(int)day.DayOfWeek] += ";" + data;
                }
            }else
            {
                string dateString = day.ToString("yyyy-MM-dd");
                int index = Array.IndexOf(_specificDates, dateString);
                if(index != -1)
                {
                    _specificData[index] += ";" + data;
                }else
                {
                    _specificDates[_specificIndex] = dateString;
                    _specificData[_specificIndex] = data;
                    _specificIndex++;
                    if(_specificDates.Length == _specificIndex)
                    {
                        string[] newDates = new string[_specificDates.Length * 2];
                        string[] newData = new string[_specificData.Length * 2];
                        Array.Copy(_specificDates, newDates, _specificDates.Length);
                        Array.Copy(_specificData, newData, _specificData.Length);
                        _specificDates = newDates;
                        _specificData = newData;
                    }
                }
            }
        }

        DateTime DayOfTheWeekToDatetime(string dateStr)
        {
            string[] parts = dateStr.Split(' ');
            string dayname = parts[0];
            int hour = 0;
            int minute = 0;
            if(parts.Length > 1)
            {
                string[] timeParts = parts[1].Split(':');
                hour = int.Parse(timeParts[0]);
                minute = int.Parse(timeParts[1]);
            }
            // 2018 started on a monday, so we can use that to calculate the date of the week
            switch(dayname.ToLower()){
                case "monday": return new DateTime(2018, 1, 1, hour, minute, 0);
                case "tuesday": return new DateTime(2018, 1, 2, hour, minute, 0);
                case "wednesday": return new DateTime(2018, 1, 3, hour, minute, 0);
                case "thursday": return new DateTime(2018, 1, 4, hour, minute, 0);
                case "friday": return new DateTime(2018, 1, 5, hour, minute, 0);
                case "saturday": return new DateTime(2018, 1, 6, hour, minute, 0);
                case "sunday": return new DateTime(2018, 1, 7, hour, minute, 0);
                default: return new DateTime(2018, 1, 1, hour, minute, 0);
            }
        }

        int _specificIndex;
        int _yearlyIndex;
        int _weeklyIndex;
        private void Start() 
        {
            int allEntries = 0;
            string[] entriesLines = null;
            string[] birthdayLines = null;
            if(HolidaysData != null)
            {
                entriesLines = HolidaysData.text.Split('\n');
                allEntries += entriesLines.Length;
            }
            if(BirthdaysData != null)
            {
                birthdayLines = BirthdaysData.text.Split('\n');
                allEntries += birthdayLines.Length;
            }
            _specificDates = new string[allEntries];
            _specificData = new string[allEntries];
            _yearlyDates = new string[allEntries];
            _yearlyData = new string[allEntries];
            if(entriesLines != null)
            {
                for(int i = 0; i < entriesLines.Length; i++)
                {
                    string[] parts = entriesLines[i].Split(new char[]{';'}, 2);
                    string[] dates = parts[0].Split(new string[] {"->"}, StringSplitOptions.None);
                    bool isYearly = dates[0].Split('-').Length == 2;
                    bool isWeekly = dates[0].Split('-').Length == 1;
                    bool isTimeframe = dates.Length == 2;
                    DateTime from;
                    if(isYearly) from = DateTime.Parse("2020-"+dates[0]); // add a year to yearly dates to make them parseable
                    else if(isWeekly) from = DayOfTheWeekToDatetime(dates[0]);
                    else from = DateTime.Parse(dates[0]);
                    if(isTimeframe)
                    {
                        DateTime to;
                        if(isYearly) to = DateTime.Parse("2020-"+dates[1]); // add a year to yearly dates to make them parseable
                        else if(isWeekly) to = DayOfTheWeekToDatetime(dates[1]);
                        else to = DateTime.Parse(dates[1]);
                        // translate from & to from UTC to local DateTime
                        from = from.ToLocalTime();
                        to = to.ToLocalTime();
                        // iterate over all days from from to to
                        DateTime startDay = new DateTime(from.Year, from.Month, from.Day);
                        DateTime endDay = new DateTime(to.Year, to.Month, to.Day);
                        for(DateTime day = startDay; day <= endDay; day = day.AddDays(1))
                        {
                            string fromStr = day == startDay ? from.ToString("HH:mm") : "00:00";
                            string toStr = day == endDay ? to.ToString("HH:mm") : "24:00";
                            AddEntry(day, "[" + fromStr + " - " + toStr + "] " + parts[1], isYearly, isWeekly);
                        }
                    }else
                    {
                        // check if from has any time information
                        // done using indexof because minutes / hours could be 0 at midnight
                        if(parts[0].IndexOf(':') != -1)
                        {
                            // if it does prefix the name with the time
                            parts[1] = "[" + from.ToString("HH:mm") + "] " + parts[1];
                        }
                        AddEntry(from, parts[1], isYearly, isWeekly);
                    }
                }
            }
            if(birthdayLines != null)
            {
                for(int i = 0; i < birthdayLines.Length; i++)
                {
                    string[] parts = birthdayLines[i].Split(';');
                    if(parts.Length < 4)
                    {
                        Debug.LogError("[Thry][Calendar] Birthday entry " + i + " is invalid. Skipping.");
                        continue;
                    }
                    string name = parts[0]+"'s Birthday";
                    string month = parts[1];
                    string day = parts[2];
                    if(month.Length == 1) month = "0" + month;
                    if(day.Length == 1) day = "0" + day;
                    string color = parts[3];
                    string id = month + "-" + day;
                    DateTime date = DateTime.ParseExact(id, "MM-dd", CultureInfo.InvariantCulture);
                    AddEntry(date, name + ";" + color, true, false);
                }
            }

            string[] temp_specificDates = new string[_specificIndex];
            string[] temp_specificData = new string[_specificIndex];
            string[] temp_yearlyDates = new string[_yearlyIndex];
            string[] temp_yearlyData = new string[_yearlyIndex];

            Array.Copy(_specificDates, temp_specificDates, _specificIndex);
            Array.Copy(_specificData, temp_specificData, _specificIndex);
            Array.Copy(_yearlyDates, temp_yearlyDates, _yearlyIndex);
            Array.Copy(_yearlyData, temp_yearlyData, _yearlyIndex);

            _specificDates = temp_specificDates;
            _specificData = temp_specificData;
            _yearlyDates = temp_yearlyDates;
            _yearlyData = temp_yearlyData;
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
                Color[] hcolors = new Color[0];
                string[] htexts = new string[0];
                LoadHolidaysForDate(day, ref htexts, ref hcolors);
                GameObject go;
                if(DaysPool.childCount == 0)
                {
                    go = Instantiate(DayPrefab, DayParent);
                }else
                {
                    go = DaysPool.GetChild(0).gameObject;
                }
                go.transform.SetParent(DayParent);
                go.SetActive(true);
                Day d = go.GetComponent<Day>();
                d.Setup(day, day.Day == now.Day && day.Month == now.Month && day.Year == now.Year, day.Month == firstDayOfMonth.Month, hcolors);
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

        Color ParseColor(string s)
        {
            if(s.StartsWith("#"))
            {
                // hex to rgb
                int red = int.Parse(s.Substring(1, 2), NumberStyles.AllowHexSpecifier);
                int green = int.Parse(s.Substring(3, 2), NumberStyles.AllowHexSpecifier);
                int blue = int.Parse(s.Substring(5, 2), NumberStyles.AllowHexSpecifier);
                return new Color(red / 255f, green / 255f, blue / 255f, 1f);

            }else
            {
                string[] color = s.Trim('(', ')', ' ', '\n', '\r').Split(',');
                return new Color(float.Parse(color[0], CultureInfo.InvariantCulture), float.Parse(color[1], CultureInfo.InvariantCulture), float.Parse(color[2], CultureInfo.InvariantCulture), 1);
            }
        }

        void LoadHolidaysForDate(System.DateTime date, ref string[] names, ref Color[] colors)
        {
            int indexSpecific = Array.IndexOf(_specificDates, date.ToString("yyyy-MM-dd"));
            int indexYearly = Array.IndexOf(_yearlyDates, date.ToString("MM-dd"));
            int resultLength = 0;
            string[] specificEntries = null;
            string[] yearliesEntries = null;
            string[] weeklyEntries = _weeklyData[(int)date.DayOfWeek].Split(';');
            if(indexSpecific != -1)
            {
                specificEntries = _specificData[indexSpecific].Split(';');
                resultLength += specificEntries.Length / 2;
            }
            if(indexYearly != -1)
            {
                yearliesEntries = _yearlyData[indexYearly].Split(';');
                resultLength += yearliesEntries.Length / 2;
            }
            resultLength += weeklyEntries.Length / 2;

            names = new string[resultLength];
            colors = new Color[resultLength];

            int j = 0;
            if(indexSpecific != -1)
            {
                for(int i = 0; i < specificEntries.Length / 2; i++)
                {
                    names[j] = specificEntries[i * 2];
                    colors[j++] = ParseColor(specificEntries[i * 2 + 1]);
                }
            }
            if(indexYearly != -1)
            {
                for(int i = 0; i < yearliesEntries.Length / 2; i++)
                {
                    names[j] = yearliesEntries[i * 2];
                    colors[j++] = ParseColor(yearliesEntries[i * 2 + 1]);
                }
            }
            for(int i = 0; i < weeklyEntries.Length / 2; i++)
            {
                names[j] = weeklyEntries[i * 2];
                colors[j++] = ParseColor(weeklyEntries[i * 2 + 1]);
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
                string[] names = null;
                Color[] colors = null;
                LoadHolidaysForDate(day.Date, ref names, ref colors);
                for(int i = 0; i < names.Length; i++)
                {
                    GameObject holiday = Instantiate(HolidayPrefab, HolidayParent);
                    holiday.SetActive(true);
                    TMPro.TextMeshProUGUI text = holiday.GetComponent<TMPro.TextMeshProUGUI>();
                    text.text = names[i];
                    text.color = colors[i];
                }
            }
        }
    }
}