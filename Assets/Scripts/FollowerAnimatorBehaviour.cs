using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class FollowerAnimatorBehaviour : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private Animator animator;

    private bool walking;
    private bool startedFunctioning = false;

    private bool pushing = false;
    private bool startedPushing = false;

    [SerializeField] FollowerBehaviour followerBehaviour;

    Transform attackingTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Destroy(this);
            return;
        }
    }

    private void OnEnable()
    {
        followerBehaviour.onStartFollowing += FollowerBehaviour_onStartFollowing;

        followerBehaviour.onStartedInteracting += FollowerBehaviour_onStartedInteracting;
        followerBehaviour.onFinishedInteracting += FollowerBehaviour_onFinishedInteracting;
    }

    private void OnDisable()
    {
        followerBehaviour.onStartFollowing -= FollowerBehaviour_onStartFollowing;

        followerBehaviour.onStartedInteracting -= FollowerBehaviour_onStartedInteracting;
        followerBehaviour.onFinishedInteracting -= FollowerBehaviour_onFinishedInteracting;

    }

    private void FollowerBehaviour_onStartedInteracting(object sender, Transform e)
    {
        if(e.CompareTag("Bridge"))
        {
            attackingTransform = e;
            pushing = true;
        }
    }

    private void FollowerBehaviour_onFinishedInteracting(object sender, System.EventArgs e)
    {
        if(pushing)
        {
            startedPushing = false;
            pushing = false;
            animator.SetBool("IsPushing", pushing);

            return;
        }
    }



    private void FollowerBehaviour_onStartFollowing(object sender, System.EventArgs e)
    {
        if (!startedFunctioning)
        {
            animator.SetTrigger("Chosen");

            StartCoroutine(WaitALittleBit());
        }
    }

    private IEnumerator WaitALittleBit()
    {
        yield return new WaitForSeconds(1f);
        startedFunctioning = true;
    }

    private void Update()
    {
        if (!startedFunctioning) return;

        if(pushing && !startedPushing)
        {
            if(attackingTransform == null)
            {
                startedPushing = false;
                pushing = false;
                animator.SetBool("IsPushing", pushing);
                return;
            }

            if (Vector3.Distance(transform.position, attackingTransform.position) < 3f)
            {
                startedPushing = true;
                animator.SetBool("IsPushing", pushing);
            }
        }

        if (agent.velocity.magnitude > 0.75f && !walking)
        {
            walking = true;
            animator.SetBool("IsWalking", true);
        }

        if (agent.velocity.magnitude <= 0.75f && walking)
        {
            walking = false;
            animator.SetBool("IsWalking", false);
        }
    }
}