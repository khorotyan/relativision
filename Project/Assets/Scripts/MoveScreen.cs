using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveScreen : MonoBehaviour {

    public Button moveScreenButton;

    public float screenMoveSpeed = 1f;

    private Vector2 prevTouchPos;
    private Vector2 curTouchPos;
    private Color32 defMoveScreenCol;
    private Color32 editorModeColor;

    private float moveRestrictLen = 100f;
    private bool canMoveTheScreen = false;
    private bool escapedFrame = false;
    private bool firstTouch = true;

	void Start ()
    {
        defMoveScreenCol = moveScreenButton.image.color;
        editorModeColor = ColorController.sharedEditorColor;
    }
	
	void Update ()
    {
        // If we can move the screen and touched (clicked) the screen, move the it
        if (canMoveTheScreen == true && Input.GetMouseButton(0))
        {           
            MoveTheScreen();
        }

        // If we release the mouse, reset the mouse information
        if (Input.GetMouseButtonUp(0))
        {
            firstTouch = true;
        }

        // If we get to the editor mode, disable drawing and the drawing mode color 
        if (RotatorAddNRemove.inEditorMode == true && moveScreenButton.image.color == editorModeColor)
        {
            moveScreenButton.image.color = defMoveScreenCol;
            canMoveTheScreen = false;
        }

        MoveRestriction();
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

    void MoveRestriction()
    {
        Vector3 camPos = Camera.main.transform.position;

        if (camPos.x > moveRestrictLen)
            Camera.main.transform.position = new Vector3(moveRestrictLen, camPos.y, camPos.z);
        else if (camPos.x < -moveRestrictLen)
            Camera.main.transform.position = new Vector3(-moveRestrictLen, camPos.y, camPos.z);

        if (camPos.y > moveRestrictLen)
            Camera.main.transform.position = new Vector3(camPos.x, moveRestrictLen, camPos.z);
        else if (camPos.y < -moveRestrictLen)
            Camera.main.transform.position = new Vector3(camPos.x, -moveRestrictLen, camPos.z);
    }

    // Enables and disables the ability to move the screen
    public void OnMoveScreen()
    {
        if (canMoveTheScreen == false)
        {
            editorModeColor = ColorController.sharedEditorColor;
            moveScreenButton.image.color = editorModeColor; // Change the image color
            canMoveTheScreen = true;
        }
        else
        {
            moveScreenButton.image.color = defMoveScreenCol;
            canMoveTheScreen = false;
        }
    }
}
