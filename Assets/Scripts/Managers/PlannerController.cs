using UnityEngine;

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


    public enum State
    {
        LOGIN = 0,
        HOME = 1,
        CREATE = 2
    }

    public State currentState;

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
}
