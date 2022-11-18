using UnityEngine;

public class FirebaseDataHandler : MonoBehaviour
{
    public static FirebaseDataHandler Instance = null;

    [Header("Player Session Info")]
    [SerializeField] private string playerSessionId;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetSessionId(string sessionId)
    {
        playerSessionId = sessionId;
    }
}
