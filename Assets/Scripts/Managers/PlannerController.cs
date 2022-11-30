using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlannerController : MonoBehaviour
{
    ///<summary>
    ///
    /// This script will be responsible for switching planner states
    /// 
    /// Initial planner states:
    /// 0] Login
    /// 1] Home
    /// 2] Create a task
    /// 3] Week view?
    /// </summary>

    public static PlannerController Instance = null;

    private int PARAM_STATE = Animator.StringToHash("State");
    [SerializeField] private Animator animator;

    public DateTime currentDate;    //This will point to the current date shown on calendar
    public enum State
    {
        LOGIN = 0,
        HOME = 1,
        CREATE = 2
    }

    public State currentState;

    [SerializeField] private List<GameObject> eventsList;

    public TextMeshProUGUI headerDateText;

    public Action OnDateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        currentDate = DateTime.Now;
        UpdateDateHeader();
    }

    #region State Changes
    public void SetState(State state)
    {
        currentState = state;

        if (animator != null && animator.gameObject.activeInHierarchy)
            animator.SetInteger(PARAM_STATE, (int)state);
    }

    public void PlannerHomeScreen()
    {
        SetState(State.HOME);
    }

    public void PlannerCreate()
    {
        SetState(State.CREATE);
    }

    public void ShowLoginScreen()
    {
        SetState(State.LOGIN);
    }
    #endregion


    #region Handle Calendar Events

    public void AddEventToList(GameObject calEvent)
    {
        if (!eventsList.Contains(calEvent))
        {
            eventsList.Add(calEvent);
        }
    }

    public void RemoveAllFromList()
    {
        if (eventsList.Count == 0) return;
        if(eventsList.Count > 0)
        {
            foreach(GameObject calEvent in eventsList)
            {
                Destroy(calEvent);
            }
        }
    }
    #endregion

    #region Handle Date Change

    public void SetToNextDate()
    {
        currentDate = currentDate.AddDays(1);
        Debug.Log("Now current Date is: " + currentDate.ToShortDateString());
        RemoveAllFromList();
        UpdateDateHeader();
    }

    public void SetToPreviousDate()
    {
        currentDate = currentDate.AddDays(-1);
        Debug.Log("Now current Date is: " + currentDate.ToShortDateString());
        RemoveAllFromList();
        UpdateDateHeader();
    }

    public void UpdateDateHeader()
    {
        headerDateText.text = $"{currentDate.ToString("dd")} {currentDate.ToString("MMMM")} {currentDate.ToString("yyyy")}";
        OnDateChanged?.Invoke();
    }
    #endregion
}
