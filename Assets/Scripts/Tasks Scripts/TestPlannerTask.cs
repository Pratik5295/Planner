
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
    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        DateTime timerNow = DateTime.Now;
        string id = timerNow.ToString();

        Debug.Log($"Now Time: {id}");
    }

    public void CreateUserDocument()
    {
        DocumentReference docRef = db.Collection("Users")
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
        DateTime timerNow = DateTime.Now;
        string id = timerNow.ToString();

        id = TimeConverter(id);
        DocumentReference docRef = db.Collection("Users")
            .Document(FirebaseDataHandler.Instance.GetSessionId())
            .Collection("Tasks")
            .Document(id);

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

    private string TimeConverter(string timer)
    {
        return timer.Replace('/', '-');
    }


}
