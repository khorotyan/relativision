using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EventEdit : MonoBehaviour {

    public GameObject moveIcon;
    public Text issueText;
    public AddAnEvent aae;

    private bool mouseDown = false;
    private bool timeWaited = false;

    private Vector2 mousePos;
    private Collider2D hitCollider;

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
        if (Input.GetMouseButtonDown(0))
        {
            EventCheck();
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }

        if (mouseDown == true)
        {
            MoveEvent();
        }

        if (timeWaited == true) // If we waited enough, disappear the issue text
        {
            issueText.gameObject.SetActive(false);
            timeWaited = false;
        }
    }

    public void OnEventEditClick()
    {
        if (RotatorAddNRemove.inEditorMode == true)
        {
            SpawnMoveIcons();
        }
        else
        {
            DeleteMoveIcons();
        }

    }

    // Spawns all the Move Icons on top of the events
    void SpawnMoveIcons()
    {
        GetEvents();

        if (events.Count != 0)
        {
            for (int i = 0; i < events.Count; i++)
            {
                GameObject tempEventMover = Instantiate(moveIcon, events[i].transform.position, Quaternion.identity) as GameObject;       
                tempEventMover.transform.parent = events[i].transform;
                tempEventMover.transform.localScale = new Vector3(1.4f, 1.4f, 0);
            }
        }
    }

    // Deletes all the Move Icons on top of the events
    void DeleteMoveIcons()
    {
        if (events.Count != 0)
        {
            for (int i = 0; i < events.Count; i++)
            {
                Destroy(events[i].transform.FindChild("moveIcon(Clone)").gameObject);
            }
        }
    }

    // Check if we clicked on the Move Icon
    void EventCheck()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Returns the world point of the mouse
        hitCollider = Physics2D.OverlapPoint(mousePos); // Returns the collider that the mouse collides with

        if (hitCollider) 
        {
            if (hitCollider.transform.tag == "EventMover") 
            {
                mouseDown = true;
            }
        }
    }

    void MoveEvent()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
        hitCollider.transform.parent.position = mousePos;
    }

    void IssueTextAction(string message)
    {
        issueText.gameObject.SetActive(true);
        issueText.text = message;

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
