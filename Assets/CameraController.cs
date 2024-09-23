using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 2.0f;     // Speed of camera movement
    public float zoomSpeed = 5.0f;     // Speed of zoom
    public float minZoom = 2.0f;       // Minimum orthographic size
    public float maxZoom = 10.0f;      // Maximum orthographic size

    private Vector3 dragOrigin;        // Where the drag started

    void Update()
    {
        // Handle camera dragging
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Input.mousePosition - dragOrigin;
            Vector3 move = new Vector3(-difference.x * dragSpeed * Time.deltaTime, -difference.y * dragSpeed * Time.deltaTime, 0);
            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
        }

        // Handle camera zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
    }
}
