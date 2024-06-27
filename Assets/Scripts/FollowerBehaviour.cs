using System;
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

    public event EventHandler onStartFollowing;

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

        if (Vector3.Distance(transform.position, agent.destination) < 1.4f)
        {
            agent.destination = transform.position;
        }
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
        StartCoroutine(WaitALittleBit());
        onStartFollowing?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator WaitALittleBit()
    {
        yield return new WaitForSeconds(1f);
        following = true;
    }

    public void StopFollowing()
    {
        following = false;
    }
}
