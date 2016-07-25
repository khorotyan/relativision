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
    public GameObject lineText;
    public GameObject parentsParent;
    public static List<GameObject> linesT; // Stores GameObjects of the T lines also the signal lines

    public static List<GameObject> linesX;

    [Space(5)] // Make a space between the public data types in the inspector
    public int bestPlaceToInsert; // Determines the position of the list after which the new T axis will be added;
    public float biggestGapBetweenLines = 0; // Stores the biggest gap between the T axis
    public float angleOfBestPlaceToInsert; // Stores the angle of the line after which the newest line will be inserted
    public float textPlaceRatio = 0.09f;
    public Text issueText;

    [HideInInspector]
    public GameObject sl1;
    [HideInInspector]
    public GameObject sl2;

    private float textScale = 5f;
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
    void LineCheckNAdd()
    {
        if (linesT.Count != 0 || linesT != null)
        {
            // Calculates the gap between consecutive lines to see where the best place to draw a new line is
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
            tempTaxis.transform.position = new Vector3(0, 0, -0.1f);

            float zRotForX = 90 - zRotForT; // Contains the Z rotation of the X axis
            GameObject tempXaxis = Instantiate(lineXObj, Vector3.zero, Quaternion.Euler(0, 0, zRotForX)) as GameObject;
            linesX.Add(tempXaxis);
            tempXaxis.transform.parent = txParent.transform;
            tempXaxis.transform.position = new Vector3(0, 0, -0.1f);  

            txParent.transform.parent = parentsParent.transform;

            so.BackgroundScaleConfig();

            AddLineNames(tempTaxis ,tempXaxis);
        }
    }

    // Adds the line names of the specified axis
    void AddLineNames(GameObject tAxis, GameObject xAxis)
    {
        // Configures the T line name
        lineText.GetComponent<TextMesh>().text = "T - " + linesX.Count;

        Renderer rendT = tAxis.GetComponent<Renderer>();

        float xCoord;
        if (tAxis.transform.eulerAngles.z >= 90)
            xCoord = rendT.bounds.min.x; // Returns the minimum x coordinate of the line (the boundary of the line)
        else
            xCoord = rendT.bounds.max.x; // Returns the maximum x coordinate of the line

        Vector3 tempPosOfT = new Vector3(xCoord * textPlaceRatio, rendT.bounds.max.y * textPlaceRatio, -0.2f);

        GameObject tText = Instantiate(lineText, tempPosOfT, tAxis.transform.rotation) as GameObject;
        tText.transform.parent = tAxis.transform;
        tText.transform.localScale = new Vector3(textScale / tAxis.transform.localScale.x, textScale / tAxis.transform.localScale.y, 1f);

        // Configures the X line name
        lineText.GetComponent<TextMesh>().text = "X - " + linesX.Count;

        Renderer rendX = xAxis.GetComponent<Renderer>();

        float yCoord;
        if (xAxis.transform.eulerAngles.z >= 0 && xAxis.transform.eulerAngles.z <= 45)
            yCoord = rendX.bounds.max.y;
        else
            yCoord = rendX.bounds.min.y;

        Vector3 tempPosOfX = new Vector3(rendX.bounds.max.x * textPlaceRatio, yCoord * textPlaceRatio, -0.2f);

        GameObject xText = Instantiate(lineText, tempPosOfX, xAxis.transform.rotation) as GameObject;
        xText.transform.parent = xAxis.transform;
        xText.transform.localScale = new Vector3(textScale / xAxis.transform.localScale.x, textScale / xAxis.transform.localScale.y, 1f);
    }

    public void AddLine()
    {
        if (RotatorAddNRemove.inEditorMode == false)
        {
            linesT = linesT.OrderBy(temp => temp.transform.eulerAngles.z).ToList(); // Sorts the list depending on the z angle of the line
            LineCheckNAdd();
            linesT = linesT.OrderBy(temp => temp.transform.eulerAngles.z).ToList(); // Sorts the list depending on the z angle of the line
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
    public List<GameObject> GetLinesT()
    {
        return linesT;
    }

    public List<GameObject> GetLinesX()
    {
        return linesX;
    }
}