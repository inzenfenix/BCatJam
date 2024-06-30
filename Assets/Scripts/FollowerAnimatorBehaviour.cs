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

    private bool punching = false;
    private bool startedPunching = false;

    [SerializeField] FollowerBehaviour followerBehaviour;

    Transform attackingTransform;

    [SerializeField] private AudioSource startSource;
    [SerializeField] private AudioClip[] startClips;

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

        followerBehaviour.onDeath += FollowerBehaviour_onDeath;
    }
    private void OnDisable()
    {
        followerBehaviour.onStartFollowing -= FollowerBehaviour_onStartFollowing;

        followerBehaviour.onStartedInteracting -= FollowerBehaviour_onStartedInteracting;
        followerBehaviour.onFinishedInteracting -= FollowerBehaviour_onFinishedInteracting;

        followerBehaviour.onDeath -= FollowerBehaviour_onDeath;

    }

    private void FollowerBehaviour_onDeath(object sender, System.EventArgs e)
    {
        startedFunctioning = false;

        animator.SetTrigger("IsDead");

        if(punching || startedPunching)
        {
            punching = false;
            startedPunching = false;

            animator.SetBool("IsPunching", punching);
        }
    }

    private void FollowerBehaviour_onStartedInteracting(object sender, Transform e)
    {
        if(e.CompareTag("Bridge"))
        {
            attackingTransform = e;
            pushing = true;

            return;
        }

        if (e.CompareTag("Rat") || e.CompareTag("Interactable"))
        {
            attackingTransform = e;
            punching = true;

            return;
        }
    }

    private void FollowerBehaviour_onFinishedInteracting(object sender, System.EventArgs e)
    {
        if(pushing)
        {
            attackingTransform = null;

            startedPushing = false;
            pushing = false;
            animator.SetBool("IsPushing", pushing);
        }

        if (punching)
        {
            attackingTransform = null;

            startedPunching = false;
            punching = false;
            animator.SetBool("IsPunching", punching);
        }

        transform.forward = CaptainCatBehaviour.currentPos - transform.forward;
    }



    private void FollowerBehaviour_onStartFollowing(object sender, System.EventArgs e)
    {
        if (!startedFunctioning)
        {
            if (startClips.Length > 0)
            {
                startSource.clip = startClips[Random.Range(0, startClips.Length)];
                startSource.Play();
            }

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
                transform.forward = CaptainCatBehaviour.currentPos - transform.forward;

                startedPushing = false;
                pushing = false;
                animator.SetBool("IsPushing", pushing);
                return;
            }

            if (Vector3.Distance(transform.position, attackingTransform.position) < 3f)
            {
                transform.forward = attackingTransform.position - transform.forward;

                startedPushing = true;
                animator.SetBool("IsPushing", pushing);
            }
        }

        if (punching && !startedPunching)
        {
            if (attackingTransform == null)
            {
                transform.forward = CaptainCatBehaviour.currentPos - transform.forward;

                startedPunching = false;
                punching = false;
                animator.SetBool("IsPunching", punching);
                return;
            }

            if (Vector3.Distance(transform.position, attackingTransform.position) < 6f)
            {
                transform.forward = attackingTransform.position - transform.forward;

                startedPunching = true;
                animator.SetBool("IsPunching", punching);
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