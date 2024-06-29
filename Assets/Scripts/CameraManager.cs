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
    private float minZoom = 25f;
    private float maxZoom = 60f;

    private float delay = 0.0f;
    private float mouseDirection;

    [SerializeField] private float distanceFromWall = 15;

    [SerializeField] private LayerMask wallMask;
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

        zoom = Mathf.Clamp(zoom, 0, 1);

        _camera.fieldOfView = Mathf.Lerp(minZoom, maxZoom, zoom);

        Vector3 offset = follow.position;

        if(Physics.Raycast(follow.position, Vector3.right, out RaycastHit hit, distanceFromWall, wallMask))
        {
            Vector3 distance = follow.position - hit.point;

            offset.x += distanceFromWall - distance.x;
        }

        if (Physics.Raycast(follow.position, -Vector3.right, out hit, distanceFromWall, wallMask))
        {
            Vector3 distance =  follow.position - hit.point;

            offset.x += distanceFromWall - distance.x;
        }

        if (Physics.Raycast(follow.position, Vector3.forward, out hit, distanceFromWall, wallMask))
        {
            Vector3 distance = follow.position - hit.point;

            offset.z += distanceFromWall - distance.z;
        }

        if (Physics.Raycast(follow.position, -Vector3.forward, out hit, distanceFromWall, wallMask))
        {
            Vector3 distance = follow.position - hit.point;

            offset.z += distanceFromWall - distance.z;
        }

        Vector3 finalPos = originalPos + offset;

        _camera.transform.position = finalPos;

        
    }
}
