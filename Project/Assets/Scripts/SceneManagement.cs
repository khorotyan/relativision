using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    public GameObject projectsCanvas;
    public GameObject projectContainer;
    public GameObject projectContainerParent;
    public GameObject[] paintScreenObjects;

    void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    // Add a new project whenever clicked on the add page button
    public void OnAddNewProjClick()
    {
        GameObject newProj = Instantiate(projectContainer) as GameObject;
        newProj.transform.SetParent(projectContainerParent.transform);
        newProj.transform.localScale = new Vector3(1f, 1f, 1f);
        newProj.transform.SetSiblingIndex(0);
    }

    // Whenever the user goes to the projects list, the list becomes visible and the draw page becomes invisible
    public void OnBackToProjectsClick()
    {
        paintScreenObjects[0].SetActive(false);
        paintScreenObjects[1].SetActive(false);
        projectsCanvas.SetActive(true);
    }
}
