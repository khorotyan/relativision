using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public AddAnEvent aae;
    public AddXandTAxis xt;

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

        ES2.Save(eventTransforms, "eventGameObjects.txt?tag=eventTransforms");
        ES2.Save(eventNames, "eventGameObjects.txt?tag=eventNames");
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

        ES2.Save(tNames, "tLineGameObjects.txt?tag=tLineNames");
        ES2.Save(tNameTransforms, "tLineGameObjects.txt?tag=tLineNameTransforms");

        ES2.Save(xTransforms, "xLineGameObjects.txt?tag=xLineTransforms");
        ES2.Save(xNames, "xLineGameObjects.txt?tag=xLineNames");
        ES2.Save(xNameTransforms, "xLineGameObjects.txt?tag=xLineNameTransforms");
    }

    void SaveCamInfo()
    {
        Transform camTransform = Camera.main.transform;
        float camZoomInfo = Camera.main.orthographicSize;

        ES2.Save(camTransform, "camera.txt?tag=camTransform");
        ES2.Save(camZoomInfo, "camera.txt?tag=camZoomInfo");
    }

    void SaveParallelStatus()
    {
        bool parallelDrawn = EventParallelConfig.parallelsAdded;

        ES2.Save(parallelDrawn, "parallelInfo.txt?tag=parStatus");
    }
}
