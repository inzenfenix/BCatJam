using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class RatAnimation : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private Animator animator;

    private bool walking;

    private RatBehaviour ratBehaviour;

    private Transform currentTarget;

    private bool punching = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Destroy(this);
            return;
        }

        ratBehaviour = agent.transform.GetComponent<RatBehaviour>();

        ratBehaviour.onStartedAttacking += RatBehaviour_onStartedAttacking;
        ratBehaviour.onFinishedAttacking += RatBehaviour_onFinishedAttacking;
    }

    private void Update()
    {
        if(punching)
        {
            if(currentTarget != null)
            {
                if(Vector3.Distance(transform.position, currentTarget.position) < 3f)
                {
                    animator.SetBool("IsPunching", punching); ;
                }
            }
            return;
        }

        if (agent.velocity.magnitude > 0.45f && !walking)
        {
            walking = true;
            animator.SetTrigger("Walking");
        }

        if (agent.velocity.magnitude <= 0.45f && walking)
        {
            walking = false;
            animator.SetTrigger("Idle");
        }
    }

    private void RatBehaviour_onStartedAttacking(object sender, Transform e)
    {
        ratBehaviour.transform.forward = e.position - transform.position;
        currentTarget = e;

        punching = true;
    }

    private void RatBehaviour_onFinishedAttacking(object sender, System.EventArgs e)
    {
        currentTarget = null;
        punching = false;
        animator.SetBool("IsPunching", punching);
    }
}
