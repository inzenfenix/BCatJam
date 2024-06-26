using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    private bool attacking = false;

    private bool following = false;

    private Transform currentlyAttacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        CaptainCatBehaviour.onAttackObject += CaptainCatBehaviour_onAttackObject;
        CaptainCatBehaviour.onMoving += CaptainCatBehaviour_onMoving;
    }


    private void OnDisable()
    {
        CaptainCatBehaviour.onAttackObject -= CaptainCatBehaviour_onAttackObject;
        CaptainCatBehaviour.onMoving -= CaptainCatBehaviour_onMoving;
    }

    private void Update()
    {
        if (attacking)
        {
            if (currentlyAttacking == null) attacking = false;
            return;
        }

        if (!following) return;

        agent.destination = CaptainCatBehaviour.currentPos * 0.92f + CaptainCatBehaviour.behindPos;
    }

    private void CaptainCatBehaviour_onAttackObject(object sender, Transform e)
    {
        if (!following) return;

        agent.destination = e.position;
        currentlyAttacking = e.transform;
        attacking = true;
    }

    private void CaptainCatBehaviour_onMoving(object sender, Vector3 e)
    {
        //agent.destination = e + CaptainCatBehaviour.behindPos;
    }

    public void StartFollowing()
    {
        following = true;
    }

    public void StopFollowing()
    {
        following = true;
    }
}
