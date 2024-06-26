using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class CatAnimatorBehaviour : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private Animator animator;

    private bool walking;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if(agent == null)
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 0.5f && !walking)
        {
            walking = true;
            animator.SetTrigger("Walking");
        }

        if(agent.velocity.magnitude <= 0.5f && walking)
        {
            walking = false;
            animator.SetTrigger("Idle");
        }
    }
}
