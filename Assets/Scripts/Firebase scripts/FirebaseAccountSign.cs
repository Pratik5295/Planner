using UnityEngine;
using System.Linq;

using Firebase.Auth;
using Firebase.Extensions;

using TMPro;

public class FirebaseAccountSign : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;

    bool hasSignedIn;

    public string userName;
    public string password = "password";

    public string extensionString = "@test.com";

    public TMP_InputField userNameField;

    public TMP_InputField existNameField;

    [Header("Modals")]
    public GameObject signUpModal;
    public GameObject signInModal;
    public GameObject createTaskModal;


    //Testing to be changed in the future
    [Header("Testing")]
    public TestPlannerTask testPlannerTask;
    private void Awake()
    {
        InitializeFirebase();

    }

    private void Start()
    {
        signUpModal.SetActive(true);
        signInModal.SetActive(true);
        createTaskModal.SetActive(false);
    }

    void AutoLoginWithLocalUser()
    {
        string generalUserName = userName + extensionString;
        // Try Sign In with Saved UserName
        auth.SignInWithEmailAndPasswordAsync(generalUserName, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)  // if fail then try to Create User with the Username
            {
                auth.CreateUserWithEmailAndPasswordAsync(generalUserName, password).ContinueWithOnMainThread(createTask =>
                {
                    if (createTask.IsCanceled || createTask.IsFaulted)
                    {
                        return;
                    }
                    if (createTask.IsCompleted)
                    {
                        user = task.Result;

                        signUpModal.SetActive(false);
                        signInModal.SetActive(false);

                        createTaskModal.SetActive(true);

                        SetSessionId(userName);
                    }
                });
            }
            else if (task.IsCompleted)
            {
                user = task.Result;
            }
        });
    }

    void InitializeFirebase()
    {

        auth = FirebaseAuth.DefaultInstance;

        //Leave this part commented out
        // auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            hasSignedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!hasSignedIn && user != null)
            {
                LogOut();
            }
            user = auth.CurrentUser;

            if (hasSignedIn)
            {

            }
        }
    }

    public bool ValidateString(string text)
    {
        string value = text;

        bool result = value.Any(c => !char.IsLetterOrDigit(c));

        return result;
    }
    public void CreateUser()
    {
        if (ValidateString(userNameField.text))
        {
            userNameField.text = string.Empty;
        }
        else
        {
            string generalUserName = userNameField.text + extensionString;
            auth.CreateUserWithEmailAndPasswordAsync(generalUserName, password).ContinueWithOnMainThread(task =>
            {

                if (task.IsCanceled)
                {
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                    Debug.LogWarning(task.Exception.InnerException);
                    task.Exception.Handle(x =>
                    {
                        return true;
                    });
                    return;
                }
                if (task.IsCompleted)
                {
                    user = task.Result;

                    signUpModal.SetActive(false);
                    signInModal.SetActive(false);

                    createTaskModal.SetActive(true);

                    SetSessionId(userNameField.text);

                    testPlannerTask.CreateUserDocument();
                }
            });
        }
    }

    public void SignInUser()
    {
        if (existNameField.text.Trim() == "") return;
        if (ValidateString(existNameField.text))
        {
            existNameField.text = string.Empty;
            return;
        }

        string generalUserName = existNameField.text + extensionString;
        auth.SignInWithEmailAndPasswordAsync(generalUserName, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {

                return;
            }
            if (task.IsFaulted)
            {
                return;
            }
            if (task.IsCompleted)
            {
                user = task.Result;
                signUpModal.SetActive(false);
                signInModal.SetActive(false);

                createTaskModal.SetActive(true);

                SetSessionId(existNameField.text);
                return;
            }
        });
    }

    public void LogOut()
    {
        auth.SignOut();

        if (FirebaseDataHandler.Instance == null) return;
        FirebaseDataHandler.Instance.SetSessionId(string.Empty, string.Empty);

        if (PlannerController.Instance == null) return;
        PlannerController.Instance.ShowLoginScreen();
    }

    private void SetSessionId(string userName)
    {
        if (FirebaseDataHandler.Instance == null) return;
        FirebaseDataHandler.Instance.SetSessionId(user.UserId, userName);

        if (PlannerController.Instance == null) return;
        PlannerController.Instance.PlannerHomeScreen();
    }
}