using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadTask : MonoBehaviour
{
    FirebaseFirestore db;
    string mainCollection;

    //Controllers
    FirebaseController fc;
    FirebaseDataHandler fd;

    //For garbage management! Object pooling to be used later
    [SerializeField] public GameObject contentParent;

    public GameObject eventPrefab;

    private void Start()
    {
        fc = FirebaseController.Instance;
        fd = FirebaseDataHandler.Instance;

        db = fc.db;
        mainCollection = fc.MAIN_COLLECTION;

        DateTime timerNow = DateTime.Now;
        string id = timerNow.ToString();
    }

    private void OnEnable()
    {
        PlannerController.Instance.OnDateChanged += OnDateChangedHandler;
    }

    private void OnDisable()
    {
        PlannerController.Instance.OnDateChanged -= OnDateChangedHandler;
    }

    private void OnDateChangedHandler()
    {
        ClearTaskObjects();
        GetAllTasks();
    }

    public void GetAllTasks()
    {
        if(db == null)
        {
            fc = FirebaseController.Instance;
            fd = FirebaseDataHandler.Instance;

            db = fc.db;

            if (db == null) return;
        }
        string id = FirebaseDataHandler.Instance.GetSessionId();

        DateTime timerNow = PlannerController.Instance.currentDate;
        string shortDate = timerNow.ToShortDateString();
        shortDate = DateConverter(shortDate);

        Query query = db.Collection(mainCollection).Document(id).Collection(shortDate);
        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot snap = task.Result;
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
                GameObject slot = GetTimeSlotObject(shortDate, documentSnapshot.Id);

                Dictionary<string, object> t = documentSnapshot.ToDictionary();

                //Task information

                string taskTitle = string.Empty;
                foreach (KeyValuePair<string, object> pair in t)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));

                    if (string.Equals(pair.Key, "taskName"))
                        taskTitle = pair.Value.ToString();
                }

                slot.GetComponent<CalendarEvent>().RenderInfo(taskTitle);
                PlannerController.Instance.AddEventToList(slot.gameObject);
                // Newline to separate entries
                Debug.Log("");
            }
        });
    }

    private void ClearTaskObjects()
    {
        PlannerController.Instance.RemoveAllFromList();
    }

    public void GetCurrentTime()
    {
        DateTime timerNow = DateTime.Now;
        string id = timerNow.ToShortTimeString();
        Debug.Log("Time:" + id);
    }

    public GameObject GetTimeSlotObject(string _shortDate,string timeId)
    {
        _shortDate = ReconvertDate(_shortDate);
        string combineTimer = $"{_shortDate} {timeId}";
        DateTime timer = DateTime.Parse(combineTimer);

        GameObject slot = TimerSlotManager.Instance.GetTimeSlotObject(timer.ToShortTimeString());

        return slot;
    }

    private string DateConverter(string timer)
    {
        //Converts date to include dashes
        return timer.Replace('/', '-');
    }

    private string ReconvertDate(string timer)
    {
        return timer.Replace('-', '/');
    }

}
