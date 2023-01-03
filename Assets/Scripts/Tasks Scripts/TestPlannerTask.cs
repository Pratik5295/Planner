
using Firebase.Firestore;
using System;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using System.Collections.Generic;

public class TestPlannerTask : MonoBehaviour
{
    public static TestPlannerTask instance = null;

    public TMP_InputField taskNameInput;
    public TMP_InputField taskDesInput;
    public TMP_Dropdown taskTimerDropdown;

    //Calendar Input
    public Calendar calendar;

    FirebaseFirestore db;
    string mainCollection;
    DateTime timerNow;

    //Controllers
    FirebaseController fc;
    FirebaseDataHandler fd;

    [SerializeField] private bool editMode;
    [SerializeField] private string tempDocumentId;
    public GameObject deleteButton;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        fc = FirebaseController.Instance;
        fd = FirebaseDataHandler.Instance;

        db = fc.db;
        mainCollection = fc.MAIN_COLLECTION;

        editMode = false;
        deleteButton.SetActive(false);
    }

    public void CreateUserDocument()
    {
        if (db == null)
        {
            fc = FirebaseController.Instance;
            fd = FirebaseDataHandler.Instance;

            db = fc.db;

            if (db == null) return;
        }

        DocumentReference docRef = db.Collection(mainCollection)
            .Document(FirebaseDataHandler.Instance.GetSessionId());

        Dictionary<string, object> update = new Dictionary<string, object>
        {
            {
                "Name", FirebaseDataHandler.Instance.GetPlayerName()
            }
        };
        docRef.SetAsync(update, SetOptions.MergeAll);
    }

    //Added to UI button
    public void SendDataButton()
    {

        if (db == null)
        {
            fc = FirebaseController.Instance;
            fd = FirebaseDataHandler.Instance;

            db = fc.db;

            if (db == null) return;
        }

        timerNow = PlannerController.Instance.currentDate;
        string calendarDate = calendar.dateValue;

        string documentId = string.Empty;
        if (!editMode)
        {
            Guid taskId = Guid.NewGuid();
            documentId = taskId.ToString();
        }
        else
        {
            documentId = tempDocumentId;
        }

        DocumentReference docRef = db.Collection(mainCollection)
            .Document(FirebaseDataHandler.Instance.GetSessionId())
            .Collection(calendarDate)
            .Document(documentId);

        FirebaseTaskData userTask = new FirebaseTaskData
        {
            taskName = taskNameInput.text,
            taskDescription = taskDesInput.text,
            taskTime = DateTime.Now,
            taskTimeId = taskTimerDropdown.options[taskTimerDropdown.value].text
        };

        docRef.SetAsync(userTask, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            Debug.Log("User task sent successfully");
            taskNameInput.text = String.Empty;
            taskDesInput.text = String.Empty;
            taskTimerDropdown.value = 0;
            deleteButton.SetActive(false);
            PlannerController.Instance.PlannerHomeScreen();
            editMode = false;
           
        });
    }

    public void DeleteEvent()
    {
        if (db == null)
        {
            fc = FirebaseController.Instance;
            fd = FirebaseDataHandler.Instance;

            db = fc.db;

            if (db == null) return;
        }

        timerNow = PlannerController.Instance.currentDate;
        string calendarDate = calendar.dateValue;

        DocumentReference docRef = db.Collection(mainCollection)
            .Document(FirebaseDataHandler.Instance.GetSessionId())
            .Collection(calendarDate)
            .Document(tempDocumentId);

        docRef.DeleteAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("Event deleted successfully");
            taskNameInput.text = String.Empty;
            taskDesInput.text = String.Empty;
            taskTimerDropdown.value = 0;
            deleteButton.SetActive(false);
            PlannerController.Instance.PlannerHomeScreen();
            editMode = false;
        });
    }

    private string DateConverter(string timer)
    {
        //Converts date to include dashes
        return timer.Replace('/', '-');
    }


    //Open event for edit

    public void OpenToEditEvent(string _docId,string _title, string _description,
        string _dateId, string _timeId)
    {
        PlannerController.Instance.PlannerCreate();

        editMode = true;

        tempDocumentId = _docId;
        taskNameInput.text = _title;
        taskDesInput.text = _description;

        int index = taskTimerDropdown.options.FindIndex((i) => { return i.text.Equals(_timeId); });
        taskTimerDropdown.value = index;

        deleteButton.SetActive(true);
    }


}
