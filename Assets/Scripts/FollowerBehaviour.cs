using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    public bool attacking = false;

    private bool following = false;

    private Transform currentlyAttacking;

    public event EventHandler onStartFollowing;

    public event EventHandler onDeath;

    public event EventHandler onFinishedInteracting;

    private int hp = 3;

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
            if(currentlyAttacking != null)
            {
                agent.destination = currentlyAttacking.position;
            }

            if (currentlyAttacking == null)
            {
                onFinishedInteracting?.Invoke(this, EventArgs.Empty);
                attacking = false;
            }
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

    public void AttackObject(Transform e)
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
        if (hp <= 0)
        {
            hp = 3;
            
        }

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

    public void GetHit(int hit)
    {
        hp -= hit;

        if(hp <= 0)
        {
            hp = 0;
            onDeath?.Invoke(this, EventArgs.Empty);
            StopFollowing();
        }
    }

    public int GetHealth()
    {
        return hp;
    }
}
