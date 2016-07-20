using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RotatorAddNRemove : MonoBehaviour {

    public Camera cam; // Reference to the camera
    public GameObject rotator; // Reference to the rotator object
    public AddXandTAxis xt;
    public ScaleObjects so;
    [Space(5)]
    public Button editButton;

    public float rotatorRatio = 2f / 3; // The ratio of the rotator object that will be placed on the lines on the edit mode

    [HideInInspector]
    public static bool inEditorMode = false;

    [SerializeField] // Make the variables visible in the inspector
    private List<GameObject> linesT; // Stores the objects of the Taxis in the list
    public void GetListT() // Gets the T axis from the AddXandTAxis class
    {
        linesT = xt.GetListT();
    }
    [SerializeField]
    private List<GameObject> linesX;
    public void GetListX()
    {
        linesX = xt.GetListX();
    }

    private Renderer rendT;
    private Renderer rendX;
    private Color32 defAddEventButtonCol;

    void Start ()
    {
        cam = Camera.main;
        defAddEventButtonCol = editButton.image.color;

        GetListT();
        GetListX();
	}
	
	void Update ()
    {
	
	}

    // Get the rotation of the lines and instantiate the rotator at a specific point
    public void OnRotatorEditClick()
    {
        inEditorMode = !inEditorMode;

        if (inEditorMode == true)
        {
            CanEdit();
            cam.backgroundColor = ColorController.sharedEditorColor; // set editor color
            editButton.image.color = ColorController.sharedEditorColor; // set button color
        }
        else
        {
            CannotEdit();
            cam.backgroundColor = defAddEventButtonCol; // set editor color back to its original color
            editButton.image.color = defAddEventButtonCol; // set button color back to its original color
        }
    }

    // Add rotator objects to the lines as a child whenever we get to the edit mode and draw them 
    //      
    void CanEdit()
    {
        GetListT();
        GetListX();

        if (linesT.Count != 0 && linesX.Count != 0)
        {
            // Draw a Rotator Icon on the T axis

            // The loop starts from 1 to linesT.Count - 1 because the list contains not only the T lines but also the signal lines
            //      Note that the lists are sorted everytime we draw a new line
            for (int i = 1; i < linesT.Count - 1; i++)
            {
                rendT = linesT[i].GetComponent<Renderer>();

                float xCoord;
                if (linesT[i].transform.eulerAngles.z >= 90)
                    xCoord = rendT.bounds.min.x; // Returns the minimum x coordinate of the line (the boundary of the line)
                else
                    xCoord = rendT.bounds.max.x; // Returns the maximum x coordinate of the line

                Vector3 tempPosT = new Vector3(xCoord * rotatorRatio, rendT.bounds.max.y * rotatorRatio, rotator.transform.position.z);

                Quaternion tempRotT = Quaternion.Euler(0f, 0f, linesT[i].transform.eulerAngles.z); // Rotations are in degrees 

                // Draw the line with tempPosT coordinates (where tempPosT is the position of the center of the line)
                //      and tempRotT rotation 
                GameObject tempObjectT = Instantiate(rotator, tempPosT, tempRotT) as GameObject;
                tempObjectT.transform.parent = linesT[i].transform;
                tempObjectT.transform.localScale = new Vector3(1f / linesT[i].transform.localScale.x * cam.orthographicSize / 5f, 1f / linesT[i].transform.localScale.y * cam.orthographicSize / 5f, 0);
            }

            // Draw a Rotator Icon on the X axis
            for (int i = 0; i < linesX.Count; i++)
            {   
                rendX = linesX[i].GetComponent<Renderer>();

                float yCoord;
                if (linesX[i].transform.eulerAngles.z >= 0 && linesX[i].transform.eulerAngles.z <= 45)
                    yCoord = rendX.bounds.max.y; 
                else
                    yCoord = rendX.bounds.min.y;

                Vector3 tempPosX = new Vector3(rendX.bounds.max.x * rotatorRatio, yCoord * rotatorRatio, rotator.transform.position.z);

                Quaternion tempRotX = Quaternion.Euler(0f, 0f, linesX[i].transform.eulerAngles.z);

                GameObject tempObjectX = Instantiate(rotator, tempPosX, tempRotX) as GameObject;
                tempObjectX.transform.parent = linesX[i].transform;
                tempObjectX.transform.localScale = new Vector3(1f / linesX[i].transform.localScale.x * cam.orthographicSize / 5f, 1f / linesX[i].transform.localScale.y * cam.orthographicSize / 5f, 0);
            }
        }
    }

    // Destroy the rotator objects from the lines whenever we exit the Edit Mode
    void CannotEdit()
    {
        GetListT();
        GetListX();

        if (linesT.Count != 0 && linesX.Count != 0)
        {
            for (int i = 1; i < linesT.Count - 1; i++)
            {
                Destroy(linesT[i].transform.FindChild("RotateArrow(Clone)").gameObject);
            }

            for (int i = 0; i < linesX.Count; i++)
            {
                Destroy(linesX[i].transform.FindChild("RotateArrow(Clone)").gameObject);
            }
        }
    }
}
