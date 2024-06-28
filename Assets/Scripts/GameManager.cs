using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LayerMask floorMask;
    public LayerMask destructableMask;
    public LayerMask catMask;

    private static float currentFillMeter;

    public static float CurrentFillMeter
    {
        get
        {
            return currentFillMeter;
        }

        set
        {
            currentFillMeter = Mathf.Clamp(value, 0.0f, 1.0f);
            if(currentFillMeter >= 1.0f)
            {
                Debug.Log("Game Finished");
                Application.Quit();
            }
        }
    }

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

        if (Physics.Raycast(ray, out RaycastHit hit, 10000, floorMask))
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

        if (Physics.Raycast(ray, out RaycastHit hit, 10000, destructableMask))
        {
            destroyable = hit.collider.transform;
            return true;
        }

        destroyable = null;
        return false;
    }

    public bool HitFollower(out Transform follower)
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000, catMask))
        {
            if (hit.collider.CompareTag("Follower"))
            {
                follower = hit.collider.transform;
                return true;
            }
        }

        follower = null;
        return false;
    }
}
