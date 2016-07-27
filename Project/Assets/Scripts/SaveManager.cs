using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public AddAnEvent aae;
    public AddXandTAxis xt;

    public GameObject projectsParent;
    public GameObject projectObject;

    public static int currentProjID;

    private List<GameObject> events;
    public void GetEvents()
    {
        events = aae.GetTheEvents();
    }
    private List<GameObject> linesT;
    public void GetLinesT()
    {
        linesT = xt.GetLinesT();
    }
    private List<GameObject> linesX;
    public void GetLinesX()
    {
        linesX = xt.GetLinesX();
    }

    void Start ()
    {
	    
	}
	
	void Update ()
    {
	
	}

    public void Save ()
    {
        SaveEvents();
        SaveTXLines();
        SaveCamInfo();
        SaveParallelStatus();
    }

    void SaveEvents()
    {
        GetEvents();

        List<Transform> eventTransforms = new List<Transform>();
        List<string> eventNames = new List<string>();

        foreach (GameObject g in events)
        {
            eventTransforms.Add(g.transform);
            eventNames.Add(g.transform.FindChild("LineText(Clone)").gameObject.GetComponent<TextMesh>().text);
        }

        ES2.Save(eventTransforms, "eventGameObjects.txt?tag=eventTransforms" + currentProjID);
        ES2.Save(eventNames, "eventGameObjects.txt?tag=eventNames" + currentProjID);
    }

    void SaveTXLines()
    {
        GetLinesX();

        List<string> tNames = new List<string>();
        List<Transform> tNameTransforms = new List<Transform>();

        List<Transform> xTransforms = new List<Transform>();
        List<string> xNames = new List<string>();
        List<Transform> xNameTransforms = new List<Transform>();

        foreach (GameObject g in linesX)
        {
            xTransforms.Add(g.transform);
            xNames.Add(g.transform.FindChild("LineText(Clone)").gameObject.GetComponent<TextMesh>().text);
            xNameTransforms.Add(g.transform.FindChild("LineText(Clone)").transform);

            tNames.Add(g.transform.parent.FindChild("TLine(Clone)").transform.FindChild("LineText(Clone)").gameObject.GetComponent<TextMesh>().text);
            tNameTransforms.Add(g.transform.parent.FindChild("TLine(Clone)").transform.FindChild("LineText(Clone)").transform);
        }

        ES2.Save(tNames, "tLineGameObjects.txt?tag=tLineNames" + currentProjID);
        ES2.Save(tNameTransforms, "tLineGameObjects.txt?tag=tLineNameTransforms" + currentProjID);

        ES2.Save(xTransforms, "xLineGameObjects.txt?tag=xLineTransforms" + currentProjID);
        ES2.Save(xNames, "xLineGameObjects.txt?tag=xLineNames" + currentProjID);
        ES2.Save(xNameTransforms, "xLineGameObjects.txt?tag=xLineNameTransforms" + currentProjID);
    }

    void SaveCamInfo()
    {
        Transform camTransform = Camera.main.transform;
        float camZoomInfo = Camera.main.orthographicSize;

        ES2.Save(camTransform, "camera.txt?tag=camTransform" + currentProjID);
        ES2.Save(camZoomInfo, "camera.txt?tag=camZoomInfo" + currentProjID);
    }

    void SaveParallelStatus()
    {
        bool parallelDrawn = EventParallelConfig.parallelsAdded;

        ES2.Save(parallelDrawn, "parallelInfo.txt?tag=parStatus" + currentProjID);
    }

    public void SaveProjectsIdsNNames()
    {
        int id = LoadManager.lastID; // Gets the last id
        List<int> ids = new List<int>();
        List<string> projectNames = new List<string>();

        int numberOfProjects = projectsParent.transform.childCount;

        for (int i = 0; i < numberOfProjects; i++)
        {
            ids.Add(projectsParent.transform.GetChild(i).transform.GetComponent<LoadManager>().ownID);
            projectNames.Add(projectsParent.transform.GetChild(i).transform.FindChild("InputField").FindChild("Placeholder").GetComponent<Text>().text);
        }

        ES2.Save(id, "projects.txt?tag=lastID" + currentProjID);
        ES2.Save(ids, "projects.txt?tag=ids" + currentProjID);
        ES2.Save(projectNames, "projects.txt?tag=projectNames" + currentProjID);
    }

    // Load the project objects that will be used to open a saved project
    public void LoadProjectsIdsNNames()
    {
        if (ES2.Exists("projects.txt?tag=lastID" + currentProjID) && ES2.Exists("projects.txt?tag=ids" + currentProjID) && ES2.Exists("projects.txt?tag=projectNames" + currentProjID))
        {
            LoadManager.lastID = ES2.Load<int>("projects.txt?tag=lastID" + currentProjID);

            List<int> ids = new List<int>();
            List<string> projectNames = new List<string>();

            ids = ES2.LoadList<int>("projects.txt?tag=ids" + currentProjID);
            projectNames = ES2.LoadList<string>("projects.txt?tag=projectNames" + currentProjID);

            for (int i = 0; i < ids.Count; i++)
            {
                GameObject tempProject = Instantiate(projectObject);
                tempProject.transform.SetParent(projectsParent.transform);
                tempProject.transform.localScale = new Vector3(1f, 1f, 1f);
                tempProject.GetComponent<LoadManager>().ownID = ids[i];
                tempProject.transform.FindChild("InputField").FindChild("Placeholder").GetComponent<Text>().text = projectNames[i];
                
                if (projectNames[i].Length == 0)
                {
                    tempProject.transform.FindChild("InputField").FindChild("Placeholder").GetComponent<Text>().text = "Empty Project";
                }
               
            }
        }
    }
}
