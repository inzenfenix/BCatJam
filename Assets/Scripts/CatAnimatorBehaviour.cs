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

    private void OnEnable()
    {
        CaptainCatBehaviour.onAttackObjectForUI += CaptainCatBehaviour_onAttackObject;
    }

    private void OnDisable()
    {
        CaptainCatBehaviour.onAttackObjectForUI -= CaptainCatBehaviour_onAttackObject;
    }

    private void CaptainCatBehaviour_onAttackObject(object sender, Transform e)
    {
        animator.SetTrigger("Charge");
        agent.destination = transform.position;
        agent.transform.forward = e.position - transform.position;
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 0.75f && !walking)
        {
            walking = true;
            animator.SetBool("IsWalking", true);
        }

        if(agent.velocity.magnitude <= 0.75f && walking)
        {
            walking = false;
            animator.SetBool("IsWalking", false);
        }
    }
}
