using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CaptainCatBehaviour : MonoBehaviour
{
    public static event EventHandler<Transform> onAttackObject;

    public static Vector3 currentPos;
    public static Vector3 behindPos;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (GameManager.instance.HitDestroyableInteractable(out Transform destroyablePos))
            {
                onAttackObject?.Invoke(this, destroyablePos);
                return;
            }

            if (GameManager.instance.HitFloor(out Vector3 pos))
            {
                agent.destination = pos;
                return;
            }
        }

        behindPos = -transform.forward;
        currentPos = transform.position;
    }
}
