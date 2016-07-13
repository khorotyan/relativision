using UnityEngine;
using System.Collections;

public class UIConfig : MonoBehaviour {

    // The GameObject which out menu
    public GameObject controllPanel;

	void Start () {
	
	}
	
	void Update () {
	
	}

    // Configures the opening and closing of the control panel
    //      Activates whenever we click on the settings button
    public void OnSettingsButtonClick()
    {
        controllPanel.SetActive(!controllPanel.activeSelf);
    }
}
