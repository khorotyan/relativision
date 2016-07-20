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
    public GameObject lineTObj;
    public GameObject lineXObj;
    public GameObject parentsParent;
    public static List<GameObject> linesT; // Stores GameObjects of the T lines also the signal lines

    public static List<GameObject> linesX;

    [Space(5)] // Make a space between the public data types in the inspector
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

    void Awake()
    {
        linesT = new List<GameObject>();
        linesX = new List<GameObject>();

        DrawSignalLines();
    }

    void Update()
    {
        if (timeWaited == true) // If we waited enough, disappear the issue text
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
    //      and calculates where to place the X axis
    void LineCheck()
    {
        if (linesT.Count != 0 || linesT != null)
        {
            // Calculates the gap between consequtive lines to see where the best place is to draw a new line
            for (int i = linesT.Count - 1; i > 0; i--)
            {
                if (linesT[i].transform.eulerAngles.z - linesT[i - 1].transform.eulerAngles.z > biggestGapBetweenLines)
                {
                    biggestGapBetweenLines = linesT[i].transform.eulerAngles.z - linesT[i - 1].transform.eulerAngles.z;
                    angleOfBestPlaceToInsert = linesT[i - 1].transform.eulerAngles.z;
                    bestPlaceToInsert = i;
                }
            }

            GameObject txParent = new GameObject(); // Make a GameObject that will contain its T and X axis
            txParent.name = "txParent";

            float zRotForT = angleOfBestPlaceToInsert + biggestGapBetweenLines / 2; // Draws the line in between the lines
            // Spawn a line whose center is the zero Vector, the GameObject is lineTObj, with rotation zRotForT around the z axis
            GameObject tempTaxis = Instantiate(lineTObj, Vector3.zero, Quaternion.Euler(0, 0, zRotForT)) as GameObject;
            linesT.Add(tempTaxis);
            tempTaxis.transform.parent = txParent.transform;

            float zRotForX = 90 - zRotForT; // Contains the Z rotation of the X axis
            GameObject tempXaxis = Instantiate(lineXObj, Vector3.zero, Quaternion.Euler(0, 0, zRotForX)) as GameObject;
            linesX.Add(tempXaxis);
            tempXaxis.transform.parent = txParent.transform;

            txParent.transform.parent = parentsParent.transform;

            so.BackgroundScaleConfig();
        }
    }

    public void AddLine()
    {
        if (RotatorAddNRemove.inEditorMode == false)
        {
            linesT = linesT.OrderBy(temp => temp.transform.eulerAngles.z).ToList<GameObject>(); // Sorts the list depending on the z angle of the line
            LineCheck();
            linesT = linesT.OrderBy(temp => temp.transform.eulerAngles.z).ToList<GameObject>(); // Sorts the list depending on the z angle of the line
            biggestGapBetweenLines = 0; // Needs to be recalculated every time we draw a new line
        }
        else
        {
            IssueTextAction();
        }
    }

    void IssueTextAction()
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

    // Returns the GameObjects to make them accessible to the other classes
    public List<GameObject> GetListT()
    {
        return linesT;
    }

    public List<GameObject> GetListX()
    {
        return linesX;
    }
}