using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadManager : MonoBehaviour
{
    // Events
    private GameObject eventsPicture;
    private GameObject eventsParent;
    private GameObject eventsName;

    private GameObject manager;
    private GameObject[] paintScreenObjects;
    private GameObject projectsCanvas;
    private Button loadButton;

    void Start ()
    {
        // Finds the objects with their names
        projectsCanvas = GameObject.Find("LoadProjectCanvas");
        manager = GameObject.Find("Main Camera");

        // Find Objects for the events
        eventsPicture = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().eventsPicture;
        eventsParent = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().eventsParent;
        eventsName = GameObject.Find("LoadObjectContainer").GetComponent<LoadObjectContainer>().eventsName;

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

        Load();   
    }

    public void Load()
    {
        LoadEvents();
    }

    // If the events loader file exists, load the events with their names
    void LoadEvents()
    {
        if (ES2.Exists("eventGameObjects.txt?tag=eventTransforms") && ES2.Exists("eventGameObjects.txt?tag=eventNames"))
        {
            List<Transform> eventTransforms = new List<Transform>();
            eventTransforms = ES2.LoadList<Transform>("eventGameObjects.txt?tag=eventTransforms");

            List<string> eventTexts = new List<string>();
            eventTexts = ES2.LoadList<string>("eventGameObjects.txt?tag=eventNames");

            for (int i = 0; i < eventTransforms.Count; i++)
            {
                // Apply the loaded positions of the events
                GameObject tempEvents = Instantiate(eventsPicture, eventTransforms[i].transform.position, Quaternion.identity) as GameObject;
                tempEvents.transform.SetParent(eventsParent.transform);
                AddAnEvent.events.Add(tempEvents);

                // Apply the loaded names of the events
                GameObject tempNames = Instantiate(eventsName, new Vector3(eventTransforms[i].transform.position.x, eventTransforms[i].transform.position.y + 0.1f, eventTransforms[i].transform.position.z), Quaternion.identity) as GameObject;
                tempNames.transform.SetParent(tempEvents.transform);
                tempNames.GetComponent<TextMesh>().text = eventTexts[i];
                tempNames.transform.localScale = new Vector3(5f, 5f, 1f);
            }
           
        }
    }

    
}
