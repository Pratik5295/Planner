using Firebase.Firestore;
using UnityEngine;

public class FirebaseController : MonoBehaviour
{
    //This script will be responsible for storing the information
    // in regards to the database.

    public static FirebaseController Instance = null;

    public FirebaseFirestore db;

    public string MAIN_COLLECTION = "Users";

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

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }
}
