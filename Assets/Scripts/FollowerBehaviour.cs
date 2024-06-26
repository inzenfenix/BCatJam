using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    private bool attacking = false;

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

    private void CaptainCatBehaviour_onAttackObject(object sender, Transform e)
    {
        agent.destination = e.position;
        currentlyAttacking = e.transform;
        attacking = true;
    }

    private void CaptainCatBehaviour_onMoving(object sender, Vector3 e)
    {
        agent.destination = e;
    }

    private void Update()
    {
        if(attacking)
        {
            if(currentlyAttacking == null) attacking = false;
            return;
        }
    }
}
