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
        if (canRotateToggle.isOn)
        {
            transform.rotation = Quaternion.Euler(0, 0, -Input.acceleration.x * 45);
            //Debug.Log(Input.acceleration.x + "a");
            //Debug.Log(Input.acceleration.x * 180 + "b");
        }
            
	}
}
