
using Firebase.Firestore;
using System;
using UnityEngine;
using TMPro;
using Firebase.Extensions;
using System.Collections.Generic;

public class TestPlannerTask : MonoBehaviour
{
    public TMP_InputField taskNameInput;
    public TMP_InputField taskDesInput;
    public TMP_Dropdown taskTimerDropdown;

    FirebaseFirestore db;
    string mainCollection;
    DateTime timerNow;

    //Controllers
    FirebaseController fc;
    FirebaseDataHandler fd;
    private void Start()
    {
        fc = FirebaseController.Instance;
        fd = FirebaseDataHandler.Instance;

        db = fc.db;
        mainCollection = fc.MAIN_COLLECTION;

    
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
        Debug.Log("Created for date: " + timerNow.ToShortDateString());
        string shortDate = timerNow.ToShortDateString();
        string shortTime = timerNow.ToShortTimeString();

        shortDate = DateConverter(shortDate);
        DocumentReference docRef = db.Collection(mainCollection)
            .Document(FirebaseDataHandler.Instance.GetSessionId())
            .Collection(shortDate)
            .Document(taskTimerDropdown.options[taskTimerDropdown.value].text);

        

        FirebaseTaskData userTask = new FirebaseTaskData
        {
            taskName = taskNameInput.text,
            taskDescription = taskDesInput.text,
            taskTime = DateTime.Now,
        };


        docRef.SetAsync(userTask, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            Debug.Log("User task sent successfully");
            taskNameInput.text = String.Empty;
            taskDesInput.text = String.Empty;
            taskTimerDropdown.value = 0;
            PlannerController.Instance.PlannerHomeScreen();
        });
    }

    private string DateConverter(string timer)
    {
        //Converts date to include dashes
        return timer.Replace('/', '-');
    }


}
