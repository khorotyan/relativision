using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ComponentEditor : MonoBehaviour {

    public AddXandTAxis xt;
    public AddAnEvent aae;
    public InputField newNameOfButton;
    public Button deleteButton;
    public static Collider2D hitCollider;
    public GameObject componentEditor;

    public bool componentEditorActive = false;

    private Vector2 mousePos;

    private float maxTimeFor2ndCLick = 0.3f;
    private float timeMeasure = 0f;
    private int clickCounter = 0;

    private List<GameObject> linesT;
    public void GetLinesT()
    {
        linesT = xt.GetLinesT();
    }
    private List<GameObject> linesX;
    public void GetLinesX()
    {
        linesX = xt.GetLinesX();
    }
    private List<GameObject> events;
    public void GetEvents()
    {
        events = aae.GetTheEvents();
    }

    void Awake ()
    {
        linesT = new List<GameObject>();
        linesX = new List<GameObject>();
        events = new List<GameObject>();

        GetLinesT();
        GetLinesX();
        GetEvents();
	}
    
	void Update ()
    {   
        OperationConfig();

        //if (componentEditorActive == true)
        //{
        //    EditTheComponent();
        //}
    }

    void OperationConfig()
    {
        // If the left button was clicked and the number of clicks on the line(event) is 0, check for a click
        if (Input.GetMouseButtonDown(0) && clickCounter == 0)
        {
            ClickCheck();
        }
        else if (clickCounter == 1) // If clicked once, check if the line was clicked the second time during the "maxTimeFor2ndClick"
        {
            if (Input.GetMouseButtonDown(0))
            {
                ClickCheck();
            }

            timeMeasure += 1 * Time.deltaTime;

            if (timeMeasure >= maxTimeFor2ndCLick)
            {
                timeMeasure = 0;
                clickCounter = 0;
            }
        }
        else if (clickCounter == 2) // If we doublecliked, enable component editing and reset the clicks
        {
            componentEditor.SetActive(true);
            componentEditorActive = true;
        }
    }

    // Checks whether the user clicked on the T or X axis
    void ClickCheck()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Returns the world point of the mouse
        hitCollider = Physics2D.OverlapPoint(mousePos); // Returns the collider that the mouse collides with

        if (hitCollider) // If the mouse collides with something
        { 
            if (hitCollider.tag == "TXLine" || hitCollider.tag == "Event") // If the something is a line text
            {
                if (hitCollider.tag == "TXLine")
                    deleteButton.GetComponentInChildren<Text>().text = "Delete The Line";
                else if (hitCollider.tag == "Event")
                    deleteButton.GetComponentInChildren<Text>().text = "Delete The Event";

                clickCounter++;
            }
        }
    }

    /*
    void EditTheComponent()
    {

    }
    */

    public void OnDeleteButtonClick()
    {
        GetLinesT();
        GetLinesX();
        GetEvents();

        // Destroy the selected gameobject and then remove it from the list
        if (hitCollider.tag == "TXLine")
        {
            if (hitCollider.transform.name == "TLine(Clone)")
            {
                linesT.Remove(hitCollider.gameObject);
                linesX.Remove(hitCollider.transform.parent.GetChild(1).gameObject);
            }
            else if (hitCollider.transform.name == "XLine(Clone)")
            {
                linesX.Remove(hitCollider.gameObject);
                linesT.Remove(hitCollider.transform.parent.GetChild(0).gameObject);
            }

            Destroy(hitCollider.transform.parent.gameObject);
        }
        else if (hitCollider.tag == "Event" || hitCollider.tag == "EventMover")
        {
            if (hitCollider.tag == "Event")
            {
                events.Remove(hitCollider.gameObject);
                Destroy(hitCollider.gameObject);
            }     
            else if (hitCollider.tag == "EventMover")
            {
                events.Remove(hitCollider.transform.parent.gameObject);
                Destroy(hitCollider.transform.parent.gameObject);
            }
                
        }

        componentEditor.SetActive(false);
        clickCounter = 0;
        componentEditorActive = false;
    }

    public void OnApplyButtonClick()
    {   
        if (hitCollider.tag == "TXLine")
        {
            if (hitCollider.transform.name == "TLine(Clone)")
                hitCollider.transform.FindChild("LineText(Clone)").gameObject.GetComponent<TextMesh>().text = "T - " + newNameOfButton.text;
            else if (hitCollider.transform.name == "XLine(Clone)")
                hitCollider.transform.FindChild("LineText(Clone)").gameObject.GetComponent<TextMesh>().text = "X - " + newNameOfButton.text;
        }
        else if (hitCollider.tag == "Event" || hitCollider.tag == "EventMover")
        {
            hitCollider.transform.FindChild("LineText(Clone)").gameObject.GetComponent<TextMesh>().text = "E - " + newNameOfButton.text;
        }

        componentEditor.SetActive(false);
        clickCounter = 0;
        componentEditorActive = false;
    }

    public void CloseComponentEditor()
    {      
        componentEditor.SetActive(false);
        clickCounter = 0;
        componentEditorActive = false;
    }
}
