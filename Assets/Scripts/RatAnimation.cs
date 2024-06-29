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

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
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
}
