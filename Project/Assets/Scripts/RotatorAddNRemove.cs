using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RotatorAddNRemove : MonoBehaviour {

    public Camera cam;
    public GameObject rotator;
    public AddXandTAxis xt;

    public float rotatorRatio = 2 / 3;

    [HideInInspector]
    public bool inEditorMode = false;

    [SerializeField]
    private List<GameObject> linesT;
    public void GetListT()
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

    void Start ()
    {
        GetListT();
        GetListX();
	}
	
	void Update ()
    {
	
	}

    // Get the rotation of the lines and instantiate the rotator at a specific point
    public void OnEditButtonClick()
    {
        inEditorMode = !inEditorMode;

        if (inEditorMode == true)
        {
            CanEdit();
            cam.backgroundColor = new Color32(255, 200, 200, 0);
        }
        else
        {
            CantEdit();
            cam.backgroundColor = new Color32(255, 255, 255, 0);
        }
    }

    // Add rotator objects to the lines as a child 
    void CanEdit()
    {
        GetListT();
        GetListX();

        if (linesT.Count != 0 && linesX.Count != 0)
        {
            for (int i = 1; i < linesT.Count - 1; i++)
            {
                // For T coordinates
                rendT = linesT[i].GetComponent<Renderer>();

                float xCoord;
                if (linesT[i].transform.eulerAngles.z >= 90)
                    xCoord = rendT.bounds.min.x;
                else
                    xCoord = rendT.bounds.max.x;

                Vector3 tempPosT = new Vector3(xCoord * rotatorRatio, rendT.bounds.max.y * rotatorRatio, rotator.transform.position.z);

                Quaternion tempRotT = Quaternion.Euler(0f, 0f, linesT[i].transform.eulerAngles.z);
                GameObject tempObjectT = Instantiate(rotator, tempPosT, tempRotT) as GameObject;
                tempObjectT.transform.parent = linesT[i].transform;
            }

            for (int i = 0; i < linesX.Count; i++)
            {
                // For X coordinates
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
            }
        }
    }

    // Destroy the rotator objects from the lines
    void CantEdit()
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
