using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Thry.Udon.Calendar
{
    public class Day : UdonSharpBehaviour
    {
        public Calendar Calendar;
        public TMPro.TextMeshProUGUI DayText;
        public Image IsCurrentDayBG;
        public Image SelectedIndicator;
        public Image HolidayIndicator;
        public Color IsCurrentMonthColor = Color.white;
        public Color IsNotCurrentMonthColor = Color.gray;
        public Color IsCurrentDayColor = Color.black;

        [HideInInspector] public bool IsHoliday;
        [HideInInspector] public string Id;
        [HideInInspector] public System.DateTime Date;

        public void Setup(System.DateTime date, bool isCurrentDay, bool isCurrentMonth, string id, bool isHoliday)
        {
            Id = id;
            IsHoliday = isHoliday;
            Date = date;
            DayText.text = date.Day.ToString();
            DayText.color = isCurrentMonth ? (isCurrentDay ? IsCurrentDayColor : IsCurrentMonthColor) : IsNotCurrentMonthColor;
            IsCurrentDayBG.gameObject.SetActive(isCurrentDay);
            HolidayIndicator.gameObject.SetActive(isHoliday);
            gameObject.name = "Day " + date.Day;
        }

        public Day Create(GameObject parent, System.DateTime date, bool isCurrentDay, bool isCurrentMonth, string id, bool isHoliday)
        {
            GameObject dayGO = Instantiate(gameObject, parent.transform);
            dayGO.SetActive(true);
            Day day = dayGO.GetComponent<Day>();
            day.Setup(date, isCurrentDay, isCurrentMonth, id, isHoliday);
            return day;
        }

        public void SetSelected(bool b)
        {
            SelectedIndicator.gameObject.SetActive(b);
        }

        public void Select()
        {
            Calendar.SelectDay(this);
        }
    }
}
