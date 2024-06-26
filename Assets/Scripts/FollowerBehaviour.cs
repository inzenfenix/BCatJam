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
    }

    private void OnDisable()
    {
        CaptainCatBehaviour.onAttackObject -= CaptainCatBehaviour_onAttackObject;
    }

    private void CaptainCatBehaviour_onAttackObject(object sender, Transform e)
    {
        agent.destination = e.position;
        currentlyAttacking = e.transform;
        attacking = true;
    }

    private void Update()
    {
        if(attacking)
        {
            if(currentlyAttacking == null) attacking = false;
            return;
        }

        agent.destination = CaptainCatBehaviour.currentPos * 0.95f + CaptainCatBehaviour.behindPos;
    }
}
