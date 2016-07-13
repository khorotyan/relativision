using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour {

    public Toggle canRotateToggle; 

	void Start ()
    {
	
	}
	
	void Update ()
    {
        // If the toggle is on, rotate the camera to the degree of the Aceelerometer angle
        //      It is hardcoded, to be improved later ...
        if (canRotateToggle.isOn)
        {
            transform.rotation = Quaternion.Euler(0, 0, -Input.acceleration.x * 45);
            //Debug.Log(Input.acceleration.x + "a");
            //Debug.Log(Input.acceleration.x * 180 + "b");
        }
            
	}
}
