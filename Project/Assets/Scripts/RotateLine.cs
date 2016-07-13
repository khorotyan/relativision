using UnityEngine;
using System.Collections.Generic;

public class RotateLine : MonoBehaviour
{
    private Vector2 mousePos;
    private Quaternion rotation;
    private Collider2D hitCollider;

    private bool mouseDown = false;

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        // If the press the left mouse button, check if we selected the line
        if (Input.GetMouseButtonDown(0))
        {
            RotateTheLineCheck();
        }
        // If we released the mouse, do not allow to move the line according to the mouse position
        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }

        // If the mouse is pressed on the line, move it using the mouse position
        if (mouseDown == true)
        {
            RotateTheLine();
        }
            
    }

    // Checks whether we clicked on the rotator icon
    void RotateTheLineCheck()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Returns the world point of the mouse
        hitCollider = Physics2D.OverlapPoint(mousePos); // Returns the collider that the mouse collides with

        if (hitCollider) // If the mouse collides with something
        {
            if (hitCollider.transform.tag == "Rotator") // If the something is a rotator object
            {
                mouseDown = true;
            }
        }
    }

    // Rotates the line using the mouse position to get the angle of the line 
    void RotateTheLine()
    {
        float colliderZangle = hitCollider.transform.parent.rotation.eulerAngles.z; // Gets the rotation of the line
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculates the angle of the line (whose center is (0,0) and end position is the touch point) from the cartesian x axis in degrees
        float tempZAngle = Mathf.Atan((mousePos.y) / (mousePos.x)) * Mathf.Rad2Deg;

        // If the mouse is on the left side of the graph, recalculate the z angles
        if (mousePos.x < 0)
        {
            tempZAngle = (tempZAngle + 180) % 360;
        }

        // The other Line (the opposite of the selected one)
        float otherZAngle = 90 - tempZAngle;
        Quaternion otherRot;

        // Do not allow the user to rotate the line to an incorrect rotation (for the T component)
        // Calculate the angle of the other line (if we select T,calculate for X and the opposite)
        if (hitCollider.transform.parent.name == "TLine(Clone)")
        {
            // If the line comes closer to the left part of the signal line
            if (colliderZangle > 130 && colliderZangle < 270)
            {
                // Push back the T line
                tempZAngle = 125;
                rotation = Quaternion.Euler(0f, 0f, tempZAngle);
                hitCollider.transform.parent.rotation = rotation;

                // Push back the X line
                otherZAngle = 90 - tempZAngle;
                otherRot = Quaternion.Euler(0f, 0f, otherZAngle);
                hitCollider.transform.parent.transform.parent.GetChild(1).rotation = otherRot;

                // Get out of the loop (do not allow to move the line), you will have to select the line again in order to move
                mouseDown = false; 
            }
            // If the line comes closer to the right part of the signal line
            else if (colliderZangle < 50 && colliderZangle >= 0 || colliderZangle <= 360 && colliderZangle >= 270)
            {
                // Push back the T line
                tempZAngle = 55;
                rotation = Quaternion.Euler(0f, 0f, tempZAngle);
                hitCollider.transform.parent.rotation = rotation;

                // Push back the X line
                otherZAngle = 90 - tempZAngle;
                otherRot = Quaternion.Euler(0f, 0f, otherZAngle);
                hitCollider.transform.parent.transform.parent.GetChild(1).rotation = otherRot;

                mouseDown = false;
            }      
        }
        // Do not allow the user to rotate the line to an incorrect rotation (for the X component)
        else if (hitCollider.transform.parent.name == "XLine(Clone)")
        {
            // If the line comes closer to the upper part of the signal line
            if (colliderZangle > 40 && colliderZangle < 180)
            {
                // Push back the X line
                tempZAngle = 35;
                rotation = Quaternion.Euler(0f, 0f, tempZAngle);
                hitCollider.transform.parent.rotation = rotation;

                // Push back the T line
                otherZAngle = 90 - tempZAngle;
                otherRot = Quaternion.Euler(0f, 0f, otherZAngle);
                hitCollider.transform.parent.transform.parent.GetChild(0).rotation = otherRot;

                mouseDown = false;
            }
            // If the line comes closer to the down part of the signal line
            else if (colliderZangle < 320 && colliderZangle >= 180)
            {
                // Push back the X line
                tempZAngle = 325;
                rotation = Quaternion.Euler(0f, 0f, tempZAngle);
                hitCollider.transform.parent.rotation = rotation;

                // Push back the T line
                otherZAngle = 90 - tempZAngle;
                otherRot = Quaternion.Euler(0f, 0f, otherZAngle);
                hitCollider.transform.parent.transform.parent.GetChild(0).rotation = otherRot;

                mouseDown = false;
            }
        }

        otherRot = Quaternion.Euler(0f, 0f, otherZAngle);

        // Set the rotation of the opposite line (the opposite of the selected line)
        if (hitCollider.transform.parent.name == "TLine(Clone)")
        {
            hitCollider.transform.parent.transform.parent.GetChild(1).rotation = otherRot;
        }
        else if (hitCollider.transform.parent.name == "XLine(Clone)")
        {
            hitCollider.transform.parent.transform.parent.GetChild(0).rotation = otherRot;
        }

        rotation = Quaternion.Euler(0f, 0f, tempZAngle);
        hitCollider.transform.parent.rotation = rotation; // Set the rotation of the selected line
    }
}
