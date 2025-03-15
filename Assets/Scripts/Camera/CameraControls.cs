using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float moveSpeed = 5f;        // Speed of camera movement
    public float zoomSpeed = 10f;      // Speed of zooming
    public float minZoom = 5f;         // Minimum zoom level
    public float maxZoom = 20f;        // Maximum zoom level

    void Update()
    {
        // Movement
        float horizontal = Input.GetAxis("Horizontal"); // A, D or arrow keys
        float vertical = Input.GetAxis("Vertical");     // W, S or arrow keys

        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Mouse wheel input
        Camera camera = Camera.main;                      // Get the main camera

        if (camera != null)
        {
            camera.orthographicSize -= scroll * zoomSpeed;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
        }
    }
}