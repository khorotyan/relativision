using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class AddXandTAxis : MonoBehaviour {

    #region variables
    public ScaleObjects so;
    public GameObject signalLine; 
    public GameObject signalLineParent;
    [Space(5)]
    public GameObject axisTandX;
    public GameObject tAxisParent;
    public GameObject xAxisParent;
    public List<GameObject> linesT;
    public List<GameObject> GetListT()
    {
        return linesT;
    }
    public List<GameObject> linesX;
    public List<GameObject> GetListX()
    {
        return linesX;
    }
    public int numberOfAddedLines = 0;
    [Space(5)]
    public int bestPlaceToInsert; // Determines the position of the list after which the new T axis will be added;
    public float biggestGapBetweenLines = 0; // Stores the biggest gap between the T axis
    public float angleOfBestPlaceToInsert; // Stores the angle of the line after which the newest line will be inserted
    public Text issueText;

    [HideInInspector]
    public GameObject sl1;
    [HideInInspector]
    public GameObject sl2;
    private bool timeWaited = false;
    #endregion

    void Awake ()
    {
        linesT = new List<GameObject>();
        linesX = new List<GameObject>();

        DrawSignalLines();
    }
	
	void Update ()
    {
        if (timeWaited == true) // If we waitedenough, disappear the issue text
        {
            issueText.gameObject.SetActive(false);
            timeWaited = false;
        }
    }

    // Draws the signal lines, adds them to the list and sets their parents
    void DrawSignalLines()
    {
        sl1 = Instantiate(signalLine, Vector3.zero, Quaternion.Euler(0, 0, 45)) as GameObject;
        sl2 = Instantiate(signalLine, Vector3.zero, Quaternion.Euler(0, 0, 135)) as GameObject;
        linesT.Add(sl1);
        linesT.Add(sl2);
        sl1.transform.parent = signalLineParent.transform;
        sl2.transform.parent = signalLineParent.transform;
    }

    // Decides where to place the new T axis based on the previously placed lines
    //  and calculates where to place the X axis
    void LineCheck()
    {
        if (linesT.Count != 0 || linesT != null)
        {
            for (int i = linesT.Count - 1; i > 0; i--)
            {
                if (linesT[i].transform.eulerAngles.z - linesT[i - 1].transform.eulerAngles.z > biggestGapBetweenLines)
                {
                    biggestGapBetweenLines = linesT[i].transform.eulerAngles.z - linesT[i - 1].transform.eulerAngles.z;
                    angleOfBestPlaceToInsert = linesT[i - 1].transform.eulerAngles.z;
                    bestPlaceToInsert = i;
                }
            }

            float zRotForT = angleOfBestPlaceToInsert + biggestGapBetweenLines / 2;
            GameObject tempTaxis = Instantiate(axisTandX, Vector3.zero, Quaternion.Euler(0, 0, zRotForT)) as GameObject;
            linesT.Add(tempTaxis);
            tempTaxis.transform.parent = tAxisParent.transform;

            float zRotForX = 90 - zRotForT;
            GameObject tempXaxis = Instantiate(axisTandX, Vector3.zero, Quaternion.Euler(0, 0, zRotForX)) as GameObject;
            linesX.Add(tempXaxis);
            tempXaxis.transform.parent = txParent.transform;

            txParent.transform.parent = parentsParent.transform;

            so.BackgroundScaleConfig();
            tempXaxis.transform.parent = xAxisParent.transform;
        }
    }

    public void AddLine()
    {
        if (RotatorAddNRemove.inEditorMode == false)
        {
            linesT = linesT.OrderBy(temp => temp.transform.eulerAngles.z).ToList<GameObject>(); // Sorts the list depending on the z angle of the line
            LineCheck();
            linesT = linesT.OrderBy(temp => temp.transform.eulerAngles.z).ToList<GameObject>(); // Sorts the list depending on the z angle of the line
            numberOfAddedLines++;
            biggestGapBetweenLines = 0; // Needs to be recalculated every time we draw a new line
        }
        else
        {
            IssueTextAction();
            issueText.gameObject.SetActive(true);
            issueText.text = "Cannot Complete The Action In The Edit Mode";

            StartCoroutine(IssueTimeConfig());  
        }
    }

    void IssueTextAction()
    {
        issueText.gameObject.SetActive(true);
        issueText.text = "Cannot Complete The Action In The Edit Mode";
    
        StartCoroutine(IssueTimeConfig());
    }

    IEnumerator IssueTimeConfig()
    {
        if (timeWaited == false)
        {
            yield return new WaitForSeconds(3);
            timeWaited = true;
        }
        
    }

}
