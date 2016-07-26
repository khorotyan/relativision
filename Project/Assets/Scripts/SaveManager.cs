using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public AddAnEvent aae;

    private List<GameObject> events;
    public void GetEvents()
    {
        events = aae.GetTheEvents();
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
}
