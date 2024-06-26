using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform follow;

    [SerializeField] private float zoomSpeed = 2.5f;

    Camera _camera;
    private Vector3 originalPos;

    private float zoom;
    private float minZoom = 0.4f;
    private float maxZoom = 1.0f;

    private float maxDistance = 1.2f;
    private float minDistance = 0.1f;

    private float delay = 0.0f;
    private float mouseDirection;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        originalPos = _camera.transform.position - follow.position;
        zoom = maxZoom;
    }

    private void Update()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            delay = 0.025f;
            mouseDirection = Input.mouseScrollDelta.y;
        }

        if(delay > 0)
        {
            zoom -= mouseDirection * Time.deltaTime * zoomSpeed;
            delay -= Time.deltaTime;
        }

        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        Vector3 offset = Vector3.Lerp(originalPos * minDistance, originalPos * maxDistance, zoom);
        _camera.transform.position = follow.position + offset;
    }
}
