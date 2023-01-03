using Firebase.Firestore;
using System;

public class PlannerTask
{
    ///<summary>
    /// This class will contain the structure of each task
    /// 
    /// Basic task structure for now:
    /// 1] Name
    /// 2] Date
    /// 3] Description?
    /// </summary>
    /// 

    public string taskName;
    public string taskDescription;
    public DateTime taskTime;

}


[FirestoreData]

public class FirebaseTaskData
{

    [FirestoreProperty]

    public string taskName { get; set; }

    [FirestoreProperty]
    public string taskDescription { get; set; }

    [FirestoreProperty]
    public string taskTimeId { get; set; }

    [FirestoreProperty]
    public DateTime taskTime { get; set; }
}

