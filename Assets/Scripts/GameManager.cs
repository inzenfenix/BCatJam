using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LayerMask floorMask;
    public LayerMask destructableMask;

    private void Awake()
    {
        instance = this;
    }

    public static Vector3 MouseToWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public bool HitFloor(out Vector3 pos)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, floorMask))
        {
            pos = hit.point;
            return true;
        }

        pos = Vector3.zero;
        return false;
    }

    public bool HitDestroyableInteractable(out Transform destroyable)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, destructableMask))
        {
            destroyable = hit.collider.transform;
            return true;
        }

        destroyable = null;
        return false;
    }
}
