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
    private void Start()
    {
        fc = FirebaseController.Instance;
        fd = FirebaseDataHandler.Instance;

        db = fc.db;
        mainCollection = fc.MAIN_COLLECTION;

        DateTime timerNow = DateTime.Now;
        string id = timerNow.ToString();
    }

    public void GetAllTasks()
    {
        if(db == null)
        {
            fc = FirebaseController.Instance;
            fd = FirebaseDataHandler.Instance;

            db = fc.db;
        }
        string id = FirebaseDataHandler.Instance.GetSessionId();

        DateTime timerNow = DateTime.Now;
        string shortDate = timerNow.ToShortDateString();
        shortDate = DateConverter(shortDate);

        Query query = db.Collection(mainCollection).Document(id).Collection(shortDate);
        query.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot snap = task.Result;
            foreach (DocumentSnapshot documentSnapshot in snap.Documents)
            {
                Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> t = documentSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in t)
                {
                    Debug.Log(String.Format("{0}: {1}", pair.Key, pair.Value));
                }

                // Newline to separate entries
                Debug.Log("");
            }
        });
    }


    public void GetCurrentTime()
    {
        DateTime timerNow = DateTime.Now;
        string id = timerNow.ToShortTimeString();
        Debug.Log("Time:" + id);
    }

    private string DateConverter(string timer)
    {
        //Converts date to include dashes
        return timer.Replace('/', '-');
    }

}
