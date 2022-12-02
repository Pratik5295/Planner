using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class Calendar : MonoBehaviour
{
    public string year;

    public string dateValue;

    public int yearValue,monthValue,dayValue;

    public TextMeshProUGUI yearText;
    public TMP_Dropdown monthDropdown;
    public TMP_Dropdown dayDropdown;

    public Action OnYearValueUpdate;

    public Action OnMonthValueUpdate;

    private List<string> fullOptions = new List<string> {"1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30","31"};
    private List<string> thirtyOptions = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30" };
    private List<string> twentynineOptions = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29"};
    private List<string> twentyeightOptions = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28" };


    private void Start()
    {
        SetToCurrentYear();
        SetToCurrentMonth();
        SetToCurrentDay();
    }

    private void OnEnable()
    {
        OnYearValueUpdate += OnYearValueUpdateHandler;
        OnMonthValueUpdate += OnMonthValueUpdateHandler;
    }

    private void OnDisable()
    {
        OnYearValueUpdate -= OnYearValueUpdateHandler;
        OnMonthValueUpdate -= OnMonthValueUpdateHandler;
    }
    #region InitializationMethods
    private void SetToCurrentYear()
    {
        yearValue = DateTime.Now.Year;
        OnYearValueUpdate?.Invoke();
    }

    private void SetToCurrentMonth()
    {
        monthValue = DateTime.Now.Month;
        monthDropdown.value = monthValue - 1;
        UpdateTotalDate();
    }

    private void SetToCurrentDay()
    {
        dayValue = DateTime.Now.Day;
        dayDropdown.value = dayValue - 1;
        UpdateTotalDate();
    }
    #endregion
    #region Year Handle
    public void AddYear()
    {
        yearValue += 1;
        OnYearValueUpdate?.Invoke();
    }

    public void SubYear()
    {
        yearValue -= 1;
        OnYearValueUpdate?.Invoke();
    }

    private void OnYearValueUpdateHandler()
    {
        year = yearValue.ToString();
        yearText.text = year;
        UpdateTotalDate();
    }
    #endregion

    #region Month
    public void UpdateMonth()
    {
        Debug.Log("Value updated to: " + monthDropdown.options[monthDropdown.value].text);
        OnMonthValueUpdate?.Invoke();
    }
    #endregion

    #region Day
    public void OnMonthValueUpdateHandler()
    {
        monthValue = monthDropdown.value + 1;
        ShowDaysInMonth(monthDropdown.value);
        UpdateTotalDate();
    }

    private void ShowDaysInMonth(int value)
    {
        switch (value)
        {
            case 0:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(fullOptions);
                break;
            case 1:
                IsLeapYear(yearValue);
                break;
            case 2:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(fullOptions);
                break;
            case 3:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(thirtyOptions);
                break;
            case 4:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(fullOptions);
                break;
            case 5:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(thirtyOptions);
                break;
            case 6:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(fullOptions);
                break;
            case 7:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(fullOptions);
                break;
            case 8:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(thirtyOptions);
                break;
            case 9:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(fullOptions);
                break;
            case 10:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(thirtyOptions);
                break;
            case 11:
                dayDropdown.ClearOptions();
                dayDropdown.AddOptions(fullOptions);
                break;
        }
    }

    public void IsLeapYear(int value)
    {
        int res = value % 4;

        if (res == 0)
        {
            dayDropdown.ClearOptions();
            dayDropdown.AddOptions(twentynineOptions);
        }
        else
        {
            dayDropdown.ClearOptions();
            dayDropdown.AddOptions(twentyeightOptions);
        }
    }

    public void DayValueUpdate()
    {
        dayValue = int.Parse(dayDropdown.options[dayDropdown.value].text);
        UpdateTotalDate();
    }
    #endregion

    public void UpdateTotalDate()
    {
        //mm-dd-yyyy

        dateValue = $"{monthValue}-{dayValue}-{yearValue}";
        Debug.Log("Date:" + dateValue);
    }
}
