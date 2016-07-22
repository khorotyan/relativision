using UnityEngine;
using System.Collections.Generic;

public class EventParallelConfig : MonoBehaviour {

    public GameObject parallelsParent;
    public GameObject xParallelObj;
    public GameObject tParallelObj;
    public AddXandTAxis xt;
    public AddAnEvent aae;

    public static bool parallelsAdded = false;

    public List<GameObject> tParallels;
    public List<GameObject> xParallels;

    private int linesXLength = 0;
    private List<GameObject> linesX;
    public void GetLinesX()
    {
        linesX = xt.GetLinesX();
    }
    private int eventsLength = 0;
    private List<GameObject> events;
    public void GetEvents()
    {
        events = aae.GetTheEvents();
    }

    void Start ()
    {
        tParallels = new List<GameObject>();
        xParallels = new List<GameObject>();
	}
	
	void Update ()
    {
        GetEvents();
        GetLinesX();

        // If we added a new line or event, destroy all the parallels and add them again
        if (linesXLength != linesX.Count || eventsLength != events.Count)
        {
            DrawParallels();
            DrawParallels();
        }

        if (parallelsAdded == true && RotatorAddNRemove.inEditorMode == true && events.Count != 0 && linesX.Count != 0)
            UpdateParallels();
    }

    public void DrawParallels()
    {
        GetLinesX();
        GetEvents();

        linesXLength = linesX.Count;
        eventsLength = events.Count;

        if (parallelsAdded == false)
        {
            for (int i = 0; i < linesX.Count; i++)
            {
                for (int j = 0; j < events.Count; j++)
                {
                    // Calculate the Length of the parallels
                    float zRotOfX = linesX[i].transform.eulerAngles.z;
                    float parLenPart = Mathf.Sin((90f + zRotOfX) * Mathf.Deg2Rad) / Mathf.Sin((90 - 2 * zRotOfX) * Mathf.Deg2Rad);

                    float eventXPos = events[j].transform.position.x;
                    float eventYPos = events[j].transform.position.y;

                    float xParLen = (eventXPos - eventYPos * Mathf.Tan(zRotOfX * Mathf.Deg2Rad)) * parLenPart;
                    float tParLen = (eventYPos - eventXPos * Mathf.Tan(zRotOfX * Mathf.Deg2Rad)) * parLenPart;
                    // End of - Calculate the Length of the parallels

                    // Set Line Lengths
                    GameObject tempXParObj = xParallelObj;
                    GameObject tempTParObj = tParallelObj;

                    tempXParObj.transform.localScale = new Vector3(xParLen, 0.03f, 0f);
                    tempTParObj.transform.localScale = new Vector3(tParLen, 0.03f, 0f);

                    // Calculate the end points of the lines
                    float xPosOfTLine = tParLen * Mathf.Sin(zRotOfX * Mathf.Deg2Rad);
                    float yPosOfTLine = tParLen * Mathf.Cos(zRotOfX * Mathf.Deg2Rad);
                    float xPosOfXLine = xParLen * Mathf.Cos(zRotOfX * Mathf.Deg2Rad);
                    float yPosOfXLine = xParLen * Mathf.Sin(zRotOfX * Mathf.Deg2Rad);

                    // Calculate the position of the Parallel Lines
                    Vector3 xPlacePos = new Vector3(eventXPos - xPosOfXLine / 2, eventYPos - yPosOfXLine / 2, 5f);
                    Vector3 tPlacePos = new Vector3(eventXPos - xPosOfTLine / 2, eventYPos - yPosOfTLine / 2, 5f);

                    // Set Line rotations
                    Quaternion tParRot = Quaternion.Euler(0f, 0f, 90 - linesX[i].transform.eulerAngles.z);
                    Quaternion xParRot = Quaternion.Euler(0f, 0f, linesX[i].transform.eulerAngles.z);

                    GameObject tPar = Instantiate(tempTParObj, tPlacePos, tParRot) as GameObject;
                    GameObject xPar = Instantiate(tempXParObj, xPlacePos, xParRot) as GameObject;

                    // Add the parallels to the lists
                    tParallels.Add(tPar);
                    xParallels.Add(xPar);

                    // Make a container that will contain its T and X parallels
                    GameObject txParContainer = new GameObject();
                    txParContainer.name = "txParContainer";

                    txParContainer.transform.parent = parallelsParent.transform;

                    tPar.transform.parent = txParContainer.transform;
                    xPar.transform.parent = txParContainer.transform;

                    // Set the initial scales of the lines
                    tPar.transform.localScale = new Vector3(tPar.transform.localScale.x, 0.03f * Camera.main.orthographicSize / 5, 0);
                    xPar.transform.localScale = new Vector3(xPar.transform.localScale.x, 0.03f * Camera.main.orthographicSize / 5, 0);
                }
            }

            parallelsAdded = true;
        }
        else
        {
            // Clear the parallel lists
            xParallels.Clear();
            tParallels.Clear();

            // Destroy all the Parallel GameObjects
            foreach (Transform tr in parallelsParent.transform)
                Destroy(tr.gameObject);

            parallelsAdded = false;
        }

    }

    void UpdateParallels()
    {
        GetLinesX();
        GetEvents();

        for (int i = 0; i < linesX.Count; i++)
        {
            for (int j = 0; j < events.Count; j++)
            {
                // Calculate the Length of the parallels
                float zRotOfX = linesX[i].transform.eulerAngles.z;
                float parLenPart = Mathf.Sin((90f + zRotOfX) * Mathf.Deg2Rad) / Mathf.Sin((90 - 2 * zRotOfX) * Mathf.Deg2Rad);

                float eventXPos = events[j].transform.position.x;
                float eventYPos = events[j].transform.position.y;

                float xParLen = (eventXPos - eventYPos * Mathf.Tan(zRotOfX * Mathf.Deg2Rad)) * parLenPart;
                float tParLen = (eventYPos - eventXPos * Mathf.Tan(zRotOfX * Mathf.Deg2Rad)) * parLenPart;
                // End of - Calculate the Length of the parallels

                // Set Line Lengths
                GameObject tempXParObj = xParallelObj;
                GameObject tempTParObj = tParallelObj;

                Vector3 xScale = new Vector3(xParLen, 0.03f, 0f);
                Vector3 tScale = new Vector3(tParLen, 0.03f, 0f);

                tempXParObj.transform.localScale = new Vector3(xParLen, 0.03f, 0f);
                tempTParObj.transform.localScale = new Vector3(tParLen, 0.03f, 0f);

                // Calculate the end points of the lines
                float xPosOfTLine = tParLen * Mathf.Sin(zRotOfX * Mathf.Deg2Rad);
                float yPosOfTLine = tParLen * Mathf.Cos(zRotOfX * Mathf.Deg2Rad);
                float xPosOfXLine = xParLen * Mathf.Cos(zRotOfX * Mathf.Deg2Rad);
                float yPosOfXLine = xParLen * Mathf.Sin(zRotOfX * Mathf.Deg2Rad);

                // Calculate the position of the Parallel Lines
                Vector3 xPlacePos = new Vector3(eventXPos - xPosOfXLine / 2, eventYPos - yPosOfXLine / 2, 5f);
                Vector3 tPlacePos = new Vector3(eventXPos - xPosOfTLine / 2, eventYPos - yPosOfTLine / 2, 5f);

                // Set Line rotations
                Quaternion tParRot = Quaternion.Euler(0f, 0f, 90 - linesX[i].transform.eulerAngles.z);
                Quaternion xParRot = Quaternion.Euler(0f, 0f, linesX[i].transform.eulerAngles.z);

                // Update the lists of the paralles with 2 loops
                tParallels[i * events.Count + j].transform.position = tPlacePos; // Set the position of the parallel
                tParallels[i * events.Count + j].transform.rotation = tParRot; // Set the rotation of the parallel
                tParallels[i * events.Count + j].transform.localScale = tScale; // Set the length of the parallel

                xParallels[i * events.Count + j].transform.position = xPlacePos;
                xParallels[i * events.Count + j].transform.rotation = xParRot;
                xParallels[i * events.Count + j].transform.localScale = xScale;
            }
        }
        
    }

    public List<GameObject> GetTParallels()
    {
        return tParallels;
    }

    public List<GameObject> GetXParallels()
    {
        return xParallels;
    }
}
