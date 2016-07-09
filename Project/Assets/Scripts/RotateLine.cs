using UnityEngine;
using System.Collections;

public class RotateLine : MonoBehaviour {

	void Start ()
    {
	    
	}

	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RotateTheLine();
        }
	}

    void RotateTheLine()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos); // Returns the collider that the mouse collides with

        if (hitCollider) // If the mouse collides with the rotator objects
        {
            if (hitCollider.transform.tag == "Rotator")
            {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 1f);
            }
        }
    }

}
