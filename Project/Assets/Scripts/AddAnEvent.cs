using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AddAnEvent : MonoBehaviour {

    public GameObject eventObject;
    public GameObject eventParent;
    public AddXandTAxis xt;
    [SerializeField]
    public List<GameObject> events; // Stores GameObjects of the events
    [Space(5)]
    public Button addEventButton;

    public float positionRatio = 2f / 3;

    private Vector2 mousePos;
    private Collider2D hitCollider;

    private Color32 defAddEventButtonCol;

    private bool mouseDown = false;
    public static bool canAddEvents = false;

    private List<GameObject> linesT;
    public void GetListT() 
    {
        linesT = xt.GetListT();
    }

    void Awake ()
    {
        events = new List<GameObject>();
        defAddEventButtonCol = addEventButton.image.color; // Keep the default button color

        GetListT();   
    }
	
	void Update ()
    {
        // Add events if not in the editor mode
	    if (canAddEvents == true && RotatorAddNRemove.inEditorMode == false)
        {
            AddEvent(); 
        }

        // Whenever we go to the editor mode, disable Adding events
        if (canAddEvents == true && RotatorAddNRemove.inEditorMode == true)
        {
            addEventButton.image.color = defAddEventButtonCol; // Change the image color back to its default
            canAddEvents = false;
        }
	}

    // Add an event to the released touch (mouse)position
    void AddEvent()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            hitCollider = Physics2D.OverlapPoint(mousePos); // Returns the collider that the mouse collides with

            if (hitCollider) // If the mouse collides with something
            {
                if (hitCollider.transform.tag == "Screen") // If the something is the screen
                {
                    mouseDown = true;
                }
            }
        }

        if (mouseDown == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Returns the world point of the mouse

                GameObject tempEvent = Instantiate(eventObject, mousePos, Quaternion.identity) as GameObject; // Spawn the event
                events.Add(tempEvent); 
                tempEvent.transform.parent = eventParent.transform; // Sets the parent of the event

                mouseDown = false;
            } 
        }
    }

    public void EventConfig()
    {
        if (canAddEvents == false)
        {
            addEventButton.image.color = new Color32(255, 200, 200, 255); // Change the image color

            canAddEvents = true;
        }
        else
        {
            addEventButton.image.color = defAddEventButtonCol; // Change the image color back to its default

            canAddEvents = false;
        }
    }

    public List<GameObject> GetTheEvents()
    {
        return events;
    }
}
