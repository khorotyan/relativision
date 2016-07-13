using UnityEngine;
using System.Collections;

// Not yet working, To be Improved later ...
public class ZoomConfig : MonoBehaviour
{
    public float orthoZoomSpeed = 0.5f; // The rate of change of the orthographic size in orthographic mode of the camera

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Finds the position in the previous frame of each of the touches
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Finds the magnitude of the vector (the distance) between the touches in each frame
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag; // Finds the difference in the distances between each frame

            // If the camera is in the orthographic mode, orthographic mode is preferred because the application doesn't have a depth
            if (GetComponent<Camera>().orthographic == true)
            {            
                GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed; // change the orthographic size based on the change in distance between the touches
                GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, 1f); // Do not allow to zoom in too much (minimum total height will be 1f * 2)
            }
        }
    }
}