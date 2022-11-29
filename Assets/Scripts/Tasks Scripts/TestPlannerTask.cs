
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

    FirebaseFirestore db;
    string mainCollection;


    //Controllers
    FirebaseController fc;
    FirebaseDataHandler fd;
    private void Start()
    {
        fc = FirebaseController.Instance;
        fd = FirebaseDataHandler.Instance;

        db = fc.db;
        mainCollection = fc.MAIN_COLLECTION;

        DateTime timerNow = DateTime.Now;
        string id = timerNow.ToString();
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

        DateTime timerNow = DateTime.Now;
        string shortDate = timerNow.ToShortDateString();
        string shortTime = timerNow.ToShortTimeString();

        shortDate = DateConverter(shortDate);
        DocumentReference docRef = db.Collection(mainCollection)
            .Document(FirebaseDataHandler.Instance.GetSessionId())
            .Collection(shortDate)
            .Document(shortTime);

        FirebaseTaskData userTask = new FirebaseTaskData
        {
            taskName = taskNameInput.text,
            taskDescription = taskDesInput.text,
            taskTime = DateTime.Now,
        };


        docRef.SetAsync(userTask, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            Debug.Log("User task sent successfully");
        });
    }

    private string DateConverter(string timer)
    {
        //Converts date to include dashes
        return timer.Replace('/', '-');
    }


}
