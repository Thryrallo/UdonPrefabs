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
        public Image[] HolidayIndicators;
        public Color IsCurrentMonthColor = Color.white;
        public Color IsNotCurrentMonthColor = Color.gray;
        public Color IsCurrentDayColor = Color.black;

        [HideInInspector] public bool IsHoliday;
        [HideInInspector] public System.DateTime Date;

        public void Setup(System.DateTime date, bool isCurrentDay, bool isCurrentMonth, Color[] holidayColors)
        {
            IsHoliday = holidayColors.Length > 0;
            Date = date;
            DayText.text = date.Day.ToString();
            DayText.color = isCurrentMonth ? (isCurrentDay ? IsCurrentDayColor : IsCurrentMonthColor) : IsNotCurrentMonthColor;
            IsCurrentDayBG.gameObject.SetActive(isCurrentDay);
            gameObject.name = "Day " + date.Day;
            for(int i = 0; i < holidayColors.Length && i < HolidayIndicators.Length; i++)
            {
                HolidayIndicators[i].gameObject.SetActive(true);
                holidayColors[i].a = isCurrentMonth ? 0.6f : 0.3f;
                HolidayIndicators[i].color = holidayColors[i];
            }
            for(int i = holidayColors.Length; i < HolidayIndicators.Length; i++)
            {
                HolidayIndicators[i].gameObject.SetActive(false);
            }
        }

        public Day Create(GameObject parent, System.DateTime date, bool isCurrentDay, bool isCurrentMonth, Color[] holidayColors)
        {
            GameObject dayGO = Instantiate(gameObject, parent.transform);
            dayGO.SetActive(true);
            Day day = dayGO.GetComponent<Day>();
            day.Setup(date, isCurrentDay, isCurrentMonth, holidayColors);
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
