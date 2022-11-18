using UnityEngine;

public class FirebaseDataHandler : MonoBehaviour
{
    public static FirebaseDataHandler Instance = null;

    [Header("Player Session Info")]

    [SerializeField] private string playerSessionId;
    [SerializeField] private string playerName;

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

    public void SetSessionId(string sessionId,string userName)
    {
        playerSessionId = sessionId;
        playerName = userName;
    }

    public string GetSessionId()
    {
        return playerSessionId;
    }
    public string GetPlayerName()
    {
        return playerName;
    }

}
