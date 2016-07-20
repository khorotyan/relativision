using UnityEngine;
using System.Collections;

public class MoveScreen : MonoBehaviour {

    public float screenMoveSpeed = 1f;

    private Vector2 prevTouchPos;
    private Vector2 curTouchPos;

    private bool canMoveTheScreen = false;
    private bool escapedFrame = false;
    private bool firstTouch = true;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	    if (canMoveTheScreen == true && Input.GetMouseButton(0))
        {
            MoveTheScreen();
        }

        // If we release the mouse, reset mouse information
        if (Input.GetMouseButtonUp(0))
        {
            firstTouch = true;
        }
	}

    // Moves the screen by draging the finger or mouse 
    void MoveTheScreen()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Mouse or touch position

        if (firstTouch == true)
        {
            curTouchPos = touchPos;
            firstTouch = false;
        }
        else
        {
            prevTouchPos = curTouchPos;
            curTouchPos = touchPos;

            Vector2 touchDiff = curTouchPos - prevTouchPos;
            
            // We escape a frame because whenever the camera moves to the desired position, current and previous positions swap
            //      and the camera jumps from one current to previous position and the opposite
            if (escapedFrame == false)
            {
                Camera.main.transform.position -= new Vector3(touchDiff.x * screenMoveSpeed, touchDiff.y * screenMoveSpeed, 0f);
                escapedFrame = true;
            }
            else
            {
                escapedFrame = false;
            }
            
        }      
    }

    // Enables and disables the ability to move the screen
    public void OnMoveScreen()
    {
        canMoveTheScreen = !canMoveTheScreen;
    }
}
