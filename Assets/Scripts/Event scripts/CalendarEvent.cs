using UnityEngine;
using TMPro;

public class CalendarEvent : MonoBehaviour
{
    public TextMeshProUGUI eventTitle;


    public void RenderInfo(string title)
    {
        eventTitle.text = title;
    }
}
