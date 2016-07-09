using UnityEngine;
using System.Collections;

public class UIConfig : MonoBehaviour {

    public GameObject controllPanel;

	void Start () {
	
	}
	
	void Update () {
	
	}

    // Configures the opening and closing of the control panel
    public void OnSettingsButtonClick()
    {
        controllPanel.SetActive(!controllPanel.activeSelf);
    }
}
