using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public static partial class Global
{
    public enum TIMESLOT
    {
        MIDNIGHT = 0,
        ONE = 1,
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7,
        EIGHT = 8,
        NINE = 9,
        TEN = 10,
        ELEVEN = 11,
        TWELVE = 12,
        THIRTEEN = 13,
        FOURTEEN = 14,
        FIFTEEN = 15,
        SIXTEEN = 16,
        SEVENTEEN = 17,
        EIGHTTEEN = 18,
        NINETEEN = 19,
        TWENTY = 20,
        TWENTYONE = 21,
        TWENTYTWO = 22,
        TWENTYTHREE = 23
            
    }

 
}
public class TimerSlotManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> timeSlots;

    public static TimerSlotManager Instance = null;

    public GameObject eventPrefab;

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
    public GameObject GetTimeSlot(string value)
    {
        GameObject res = timeSlots[0];
        switch (value)
        {
            case "12:00 AM":
                res = timeSlots[0];
                break;
            case "1:00 AM":
                res = timeSlots[1];
                break;
            case "2:00 AM":
                res = timeSlots[2];
                break;
            case "3:00 AM":
                res = timeSlots[3];
                break;
            case "4:00 AM":
                res = timeSlots[4];
                break;
            case "5:00 AM":
                res = timeSlots[5];
                break;
            case "6:00 AM":
                res = timeSlots[6];
                break;
            case "7:00 AM":
                res = timeSlots[7];
                break;
            case "8:00 AM":
                res = timeSlots[8];
                break;
            case "9:00 AM":
                res = timeSlots[9];
                break;
            case "10:00 AM":
                res = timeSlots[10];
                break;
            case "11:00 AM":
                res = timeSlots[11];
                break;
            case "12:00 PM":
                res = timeSlots[12];
                break;
            case "1:00 PM":
                res = timeSlots[13];
                break;
            case "2:00 PM":
                res = timeSlots[14];
                break;
            case "3:00 PM":
                res = timeSlots[15];
                break;
            case "4:00 PM":
                res = timeSlots[16];
                break;
            case "5:00 PM":
                res = timeSlots[17];
                break;
            case "6:00 PM":
                res = timeSlots[18];
                break;
            case "7:00 PM":
                res = timeSlots[19];
                break;
            case "8:00 PM":
                res = timeSlots[20];
                break;
            case "9:00 PM":
                res = timeSlots[21];
                break;
            case "10:00 PM":
                res = timeSlots[22];
                break;
            case "11:00 PM":
                res = timeSlots[23];
                break;


        }

        return res;
    }

    public GameObject CreateTimeSlot(GameObject go)
    {
        //For now, we instantiate the event prefab
        //Object pooling would be used later

        GameObject eventInst = Instantiate(eventPrefab, go.transform.position, Quaternion.identity);
        eventInst.transform.SetParent(go.transform);

        return eventInst;
    }

    public GameObject GetTimeSlotObject(string value)
    {
        GameObject slot = GetTimeSlot(value);

        GameObject res = CreateTimeSlot(slot);

        return res;
    }

}
