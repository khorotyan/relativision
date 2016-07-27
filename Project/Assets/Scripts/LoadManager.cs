using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadManager : MonoBehaviour
{
    public static int lastID = -1;
    public int ownID;

    // Events
    private GameObject eventsPicture;
    private GameObject eventsParent;

    private GameObject nameObject;

    // T and X Lines
    private GameObject tLineObject;
    private GameObject xLineObject;
    private GameObject txParentsParent;

    private GameObject manager;
    private GameObject[] paintScreenObjects;
    private GameObject projectsCanvas;
    private Button loadButton;

    // Parallel Info
    private EventParallelConfig epc;

    void Start ()
    {
        lastID++;
        ownID = lastID;

        // Finds the objects with their names
        projectsCanvas = GameObject.Find("LoadProjectCanvas");
        manager = GameObject.Find("Main Camera");

        // Find Objects for the events
        eventsPicture = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().eventsObject;
        eventsParent = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().eventsParent;
        nameObject = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().nameObject;

        // Find Objects for the T and X lines
        tLineObject = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().tLineObject;
        xLineObject = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().xLineObject;
        txParentsParent = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().txParentsParent;

        // Find EventParallelConfig Object
        epc = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().epc;

        paintScreenObjects = new GameObject[2];
        paintScreenObjects[0] = manager.GetComponent<SceneManagement>().paintScreenObjects[0];
        paintScreenObjects[1] = manager.GetComponent<SceneManagement>().paintScreenObjects[1];

        loadButton = gameObject.GetComponent<Button>();
        loadButton.onClick.AddListener(delegate () { LoadProject(); });
    }
	
	void Update ()
    {
	    
	}

    // Whenever a project is clicked to load, the load page closes and the project opens
    public void LoadProject()
    {
        projectsCanvas.SetActive(false);
        paintScreenObjects[0].SetActive(true);
        paintScreenObjects[1].SetActive(true);
        loadButton.transform.SetSiblingIndex(0);

        SaveManager.currentProjID = ownID; // Set the id of the last entered project

        Load();   
    }

    public void Load()
    {
        LoadEvents();
        LoadTXLines();
        LoadCameraInfo();
        LoadParallelInfo();
    }

    // If the events loader file exists, load the events with their names
    void LoadEvents()
    {
        if (ES2.Exists("eventGameObjects.txt?tag=eventTransforms" + SaveManager.currentProjID) && ES2.Exists("eventGameObjects.txt?tag=eventNames" + SaveManager.currentProjID))
        {
            List<Transform> eventTransforms = new List<Transform>();
            eventTransforms = ES2.LoadList<Transform>("eventGameObjects.txt?tag=eventTransforms" + SaveManager.currentProjID);

            List<string> eventTexts = new List<string>();
            eventTexts = ES2.LoadList<string>("eventGameObjects.txt?tag=eventNames" + SaveManager.currentProjID);

            for (int i = 0; i < eventTransforms.Count; i++)
            {               
                // Apply the loaded positions of the events
                GameObject tempEvents = Instantiate(eventsPicture, eventTransforms[i].transform.position, Quaternion.identity) as GameObject;
                tempEvents.transform.SetParent(eventsParent.transform);
                AddAnEvent.events.Add(tempEvents);

                // Apply the loaded names of the events
                GameObject tempNames = Instantiate(nameObject, new Vector3(eventTransforms[i].transform.position.x, eventTransforms[i].transform.position.y + 0.1f, eventTransforms[i].transform.position.z), Quaternion.identity) as GameObject;
                tempNames.transform.SetParent(tempEvents.transform);
                tempNames.GetComponent<TextMesh>().text = eventTexts[i];
                tempNames.transform.localScale = new Vector3(5f, 5f, 1f);
            }
           
        }
    }

    // Load the T and X lines with their names
    void LoadTXLines()
    {
        if (ES2.Exists("tLineGameObjects.txt?tag=tLineNames" + SaveManager.currentProjID) && ES2.Exists("xLineGameObjects.txt?tag=xLineNames" + SaveManager.currentProjID) && ES2.Exists("xLineGameObjects.txt?tag=xLineNames" + SaveManager.currentProjID)
             && ES2.Exists("tLineGameObjects.txt?tag=tLineNameTransforms" + SaveManager.currentProjID) && ES2.Exists("xLineGameObjects.txt?tag=xLineNameTransforms" + SaveManager.currentProjID))
        {        
            List<string> tLineTexts = new List<string>();
            tLineTexts = ES2.LoadList<string>("tLineGameObjects.txt?tag=tLineNames" + SaveManager.currentProjID);
            List<Transform> tNameTransforms = new List<Transform>();
            tNameTransforms = ES2.LoadList<Transform>("tLineGameObjects.txt?tag=tLineNameTransforms" + SaveManager.currentProjID);

            List<Transform> xLineTransforms = new List<Transform>();
            xLineTransforms = ES2.LoadList<Transform>("xLineGameObjects.txt?tag=xLineTransforms" + SaveManager.currentProjID);
            List<string> xLineTexts = new List<string>();
            xLineTexts = ES2.LoadList<string>("xLineGameObjects.txt?tag=xLineNames" + SaveManager.currentProjID);
            List<Transform> xNameTransforms = new List<Transform>();
            xNameTransforms = ES2.LoadList<Transform>("xLineGameObjects.txt?tag=xLineNameTransforms" + SaveManager.currentProjID);

            for (int i = 0; i < xLineTransforms.Count; i++)
            {
                GameObject txParent = new GameObject(); // Make a GameObject that will contain its T and X axis
                txParent.name = "txParent";

                // Apply T line Info
                float xLineZRot = xLineTransforms[i].eulerAngles.z;
                GameObject tempTLine = Instantiate(tLineObject, xLineTransforms[i].position, Quaternion.Euler(0, 0, 90 - xLineZRot)) as GameObject;
                tempTLine.transform.parent = txParent.transform;
                tempTLine.transform.localScale = xLineTransforms[i].transform.localScale;
                AddXandTAxis.linesT.Add(tempTLine);

                GameObject tempTLineNames = Instantiate(nameObject, tNameTransforms[i].position, Quaternion.identity) as GameObject;
                tempTLineNames.transform.SetParent(tempTLine.transform);
                tempTLineNames.GetComponent<TextMesh>().text = tLineTexts[i];
                tempTLineNames.transform.localScale = tNameTransforms[i].localScale;
                tempTLineNames.transform.rotation = tempTLine.transform.rotation;

                // Apply X Line Info
                
                GameObject tempXLine = Instantiate(xLineObject, xLineTransforms[i].position, xLineTransforms[i].rotation) as GameObject;
                tempXLine.transform.parent = txParent.transform;
                tempXLine.transform.localScale = xLineTransforms[i].transform.localScale;
                AddXandTAxis.linesX.Add(tempXLine);

                GameObject tempXLineNames = Instantiate(nameObject, xNameTransforms[i].position, Quaternion.identity) as GameObject;
                tempXLineNames.transform.SetParent(tempXLine.transform);
                tempXLineNames.GetComponent<TextMesh>().text = xLineTexts[i];
                tempXLineNames.transform.localScale = xNameTransforms[i].localScale;
                tempXLineNames.transform.rotation = tempXLine.transform.rotation;

                txParent.transform.SetParent(txParentsParent.transform);
            }
        }
    }
    // Checks if in the saved project the parallels were enabled
    void LoadParallelInfo()
    {
        bool parallelsAdded = false;
        //ES2.Save(parallelDrawn, "parallelInfo.txt?tag=parStatus");
        if (ES2.Exists("parallelInfo.txt?tag=parStatus" + SaveManager.currentProjID))
        {
            parallelsAdded = ES2.Load<bool>("parallelInfo.txt?tag=parStatus" + SaveManager.currentProjID);

            if (parallelsAdded == true)
            {
                epc.DrawParallels();
            }
        }
    }

    // Load Camera Position and its zoom info 
    void LoadCameraInfo()
    {
        if (ES2.Exists("camera.txt?tag=camTransform" + SaveManager.currentProjID) && ES2.Exists("camera.txt?tag=camZoomInfo" + SaveManager.currentProjID))
        { 
            Camera.main.transform.position = ES2.Load<Transform>("camera.txt?tag=camTransform" + SaveManager.currentProjID).position;
            Camera.main.transform.rotation = ES2.Load<Transform>("camera.txt?tag=camTransform" + SaveManager.currentProjID).rotation;

            Camera.main.orthographicSize = ES2.Load<float>("camera.txt?tag=camZoomInfo" + SaveManager.currentProjID);
        }
    }

    
}
