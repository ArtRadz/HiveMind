using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera camera = Camera.main;

        if (camera == null) return;

        camera.orthographicSize -= scroll * zoomSpeed;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);
    }
}