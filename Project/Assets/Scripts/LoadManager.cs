using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadManager : MonoBehaviour
{
    private GameObject manager;
    private GameObject[] paintScreenObjects;
    private GameObject projectsCanvas;

    private Button loadButton;


    void Start ()
    {
        // Finds the objects with their names
        projectsCanvas = GameObject.Find("LoadProjectCanvas");
        manager = GameObject.Find("Main Camera");
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
    void LoadProject()
    {
        projectsCanvas.SetActive(false);
        paintScreenObjects[0].SetActive(true);
        paintScreenObjects[1].SetActive(true);
        loadButton.transform.SetSiblingIndex(0);      
    }

    
}
