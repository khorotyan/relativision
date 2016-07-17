using UnityEngine;
using System.Collections.Generic;

public class EventParallelConfig : MonoBehaviour {

    public GameObject parallelsParent;
    public GameObject xParallel;
    public GameObject tParallel;
    public AddXandTAxis xt;
    public AddAnEvent aae;

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
	
	}

    public void DrawParallels()
    {
        GetListT();
        GetListX();
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
                GameObject tempXParObj = xParallel;
                GameObject tempTParObj = tParallel;

                tempXParObj.transform.localScale = new Vector3(xParLen, 0.03f, 0f);
                tempTParObj.transform.localScale = new Vector3(tParLen, 0.03f, 0f);

                // Calculate the end points of the lines
                float xPosOfTLine = tParLen * Mathf.Sin(zRotOfX * Mathf.Deg2Rad);
                float yPosOfTLine = tParLen * Mathf.Cos(zRotOfX * Mathf.Deg2Rad);
                float xPosOfXLine = xParLen * Mathf.Cos(zRotOfX * Mathf.Deg2Rad);
                float yPosOfXLine = xParLen * Mathf.Sin(zRotOfX * Mathf.Deg2Rad);

                // Calculate the position of the Parallel Lines
                Vector3 xPlacePos = new Vector3(eventXPos - xPosOfXLine / 2, eventYPos - yPosOfXLine / 2, 0f);
                Vector3 tPlacePos = new Vector3(eventXPos - xPosOfTLine / 2, eventYPos - yPosOfTLine / 2, 0f);

                // Set Line rotations
                Quaternion tParRot = Quaternion.Euler(0f, 0f, 90 - linesX[i].transform.eulerAngles.z);
                Quaternion xParRot = Quaternion.Euler(0f, 0f, linesX[i].transform.eulerAngles.z);

                GameObject tPar = Instantiate(tempTParObj, tPlacePos, tParRot) as GameObject;
                GameObject xPar = Instantiate(tempXParObj, xPlacePos, xParRot) as GameObject;

                // Make a container that will contain its T and X parallels
                GameObject txParContainer = new GameObject();
                txParContainer.name = "txParContainer";

                txParContainer.transform.parent = parallelsParent.transform;

                tPar.transform.parent = txParContainer.transform;
                xPar.transform.parent = txParContainer.transform;
            }     
        }
    }
}
