using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ResetScreen : MonoBehaviour {

    public AddXandTAxis xt;
    public AddAnEvent aae;
    public GameObject txHolder;
    public GameObject eventParent;
    public GameObject parallelsParent;
    public Text issueText;

    private GameObject sl1;
    private GameObject sl2;

    private bool timeWaited = false;

    private List<GameObject> linesT; 
    public void GetListT() 
    {
        linesT = xt.GetListT();
    }
    private List<GameObject> linesX;
    public void GetListX()
    {
        linesX = xt.GetListX();
    }
    private List<GameObject> events;
    public void GetEvents() 
    {
        events = aae.GetTheEvents();
    }

    void Start ()
    {
	
	}
	
	void Update ()
    {
        if (timeWaited == true) // If we waited enough, disappear the issue text
        {
            issueText.gameObject.SetActive(false);
            timeWaited = false;
        }
    }

    public void ResetTheScreen()
    {
        if (RotatorAddNRemove.inEditorMode == false)
        {
            GetListT();
            GetListX();
            GetEvents();

            sl1 = xt.sl1;
            sl2 = xt.sl2;

            Camera.main.gameObject.transform.rotation = Quaternion.identity; // Resets the rotation of the camera
            Camera.main.orthographicSize = 5; // Resets the Zoom info 

            // Destroy all the line GameObjects except the signal line, "parentsParent" is the parent of all the X and T axis
            foreach (Transform child in txHolder.transform)
                Destroy(child.gameObject);

            // Destroy all the event GameObjects
            foreach (Transform tr in eventParent.transform)
                Destroy(tr.gameObject);

            // Destroy all the event GameObjects
            foreach (Transform tran in parallelsParent.transform)
                Destroy(tran.gameObject);

            // Clear the lists containing the information about the lines
            linesT.Clear();
            linesX.Clear();

            // Clear the events
            events.Clear();

            // Add the signal lines to the list
            linesT.Add(sl1);
            linesT.Add(sl2);
        }
        else
        {
            IssueTextAction(); // If in the editor mode, display an issue to the screen
        }
    }

    public void IssueTextAction()
    {
        issueText.gameObject.SetActive(true);
        issueText.text = "Cannot Complete The Action In The Edit Mode";

        StartCoroutine(IssueTimeConfig());
    }

    IEnumerator IssueTimeConfig()
    {   // Whenever WaitForSeconds finishes, timeWaited becomes true which means that we can disable the "issueText"
        if (timeWaited == false)
        {
            yield return new WaitForSeconds(3);
            timeWaited = true;
        }

    }
}
