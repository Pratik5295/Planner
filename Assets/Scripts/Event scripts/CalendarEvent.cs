using UnityEngine;
using TMPro;

public class CalendarEvent : MonoBehaviour
{
    public TextMeshProUGUI eventTitle;

    public string documentId;
    public string title;
    public string description;
    public string dateId;   // Which corresponds to short date string
    public string timeId; //Corresponds to short time string

    public void RenderInfo(string title)
    {
        eventTitle.text = title;
    }

    public void FillInfo(string _docID,string _title, string _description,
        string _dateId, string _timeId)
    {
        documentId = _docID;

        title = _title;
        description = _description;
        dateId = _dateId;
        timeId = _timeId;

        RenderInfo(title);
    }

    public void OpenEventForEdit()
    {
        TestPlannerTask.instance.OpenToEditEvent(documentId,title, description, dateId, timeId);
        Calendar.instance.SetDropDown(dateId);
    }

    private string DateReConverter(string timer)
    {
        //Converts date to include dashes
        return timer.Replace('-', '/');
    }
}
