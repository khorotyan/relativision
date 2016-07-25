using UnityEngine;
using System.Collections.Generic;

public class ScaleObjects : MonoBehaviour {

    public AddXandTAxis xt;
    public AddAnEvent aae;
    public GameObject backgroundImage;

    public GameObject[] parallels;
    public GameObject[] eventMoveIcons;
    public GameObject[] lineRotateIcons;

    private List<GameObject> events;
    public void GetEvents()
    {
        events = aae.GetTheEvents();
    }

    private List<GameObject> linesT;
    public void GetLineT()
    {
        linesT = xt.GetLinesT();
    }

    private List<GameObject> linesX;
    public void GetLineX()
    {
        linesX = xt.GetLinesX();
    }

    void Start ()
    {
        linesT = new List<GameObject>();
        linesX = new List<GameObject>();
    }
	
	void Update ()
    {
        //if (Input.touchCount == 2)
        //{
            BackgroundScaleConfig();
        //}
	}

    // If there are 2 touches on the screen, update the background scale with the zoom scale
    public void BackgroundScaleConfig()
    {
        GetEvents();
        GetLineT();
        GetLineX();

        float camOrthoSize = Camera.main.orthographicSize;

        // Scale The Background, 1 is the default background scale and 5 is the default orthographic size
        backgroundImage.transform.localScale = new Vector3(1 * camOrthoSize / 5, 1 * camOrthoSize / 5, 0f);

        // Update Event Scales
        for (int i = 0; i < events.Count; i++)
        {
            events[i].transform.localScale = new Vector3(1 * camOrthoSize / 5, 1 * camOrthoSize / 5, 0f);
        }

        // Update T Lines
        for (int i = 0; i < linesT.Count; i++)
        {
            linesT[i].transform.localScale = new Vector3(50f * camOrthoSize / 5, 0.03f * camOrthoSize / 5, 0f);
        }

        // Update X Lines
        for (int i = 0; i < linesX.Count; i++)
        {
            linesX[i].transform.localScale = new Vector3(50f * camOrthoSize / 5, 0.03f * camOrthoSize / 5, 0f);
        }

        // Update Line Scales
        parallels = GameObject.FindGameObjectsWithTag("Parallel");

        // 0.03 is the initial y scale of the Parallel line, and 5 is the initial zoom scale of the camera
        foreach (GameObject go in parallels)
            go.transform.localScale = new Vector3(go.transform.localScale.x, 0.03f * camOrthoSize / 5, 0f);

        // Update Rotator Icon Scales
        parallels = GameObject.FindGameObjectsWithTag("Rotator");
        
        //foreach (GameObject go in parallels)
        //    go.transform.localScale = new Vector3(0.1f * camOrthoSize / 5f, 33.3333f * camOrthoSize / 5f, 0f);

        // go.transform.localScale = new Vector3(go.transform.localScale.x * 5 / (10 * camOrthoSize), go.transform.localScale.y * 5 / (10 * camOrthoSize), 0f);
    }
}
