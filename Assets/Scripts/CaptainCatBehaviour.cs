using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CaptainCatBehaviour : MonoBehaviour
{
    public static event EventHandler<Transform> onAttackObject;
    public static event EventHandler<Vector3> onMoving;

    public static Vector3 currentPos;
    public static Vector3 behindPos;

    private NavMeshAgent agent;

    private bool attacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;
    }

    private void OnEnable()
    {
        DestroyableBehaviour.onObjectGettingDestroyed += DestroyableBehaviour_onObjectDestroyed;
    }

    private void DestroyableBehaviour_onObjectDestroyed(object sender, EventArgs e)
    {
        attacking = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (GameManager.instance.HitFollower(out Transform follower))
            {
                follower.GetComponent<FollowerBehaviour>().StartFollowing();
                return;
            }

            if (GameManager.instance.HitDestroyableInteractable(out Transform destroyablePos))
            {
                onAttackObject?.Invoke(this, destroyablePos);

                return;
            }

            if (GameManager.instance.HitFloor(out Vector3 pos))
            {
                agent.destination = pos;
                onMoving?.Invoke(this, pos * 0.95f);

                return;
            }

            
        }

        behindPos = -transform.forward;
        currentPos = transform.position;
    }
}
