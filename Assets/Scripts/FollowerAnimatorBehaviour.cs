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

    [SerializeField] FollowerBehaviour followerBehaviour;

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
    }

    private void OnDisable()
    {
        followerBehaviour.onStartFollowing -= FollowerBehaviour_onStartFollowing;
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