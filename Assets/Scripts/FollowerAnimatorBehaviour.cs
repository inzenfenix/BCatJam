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

    [Header("\nStart sounds")]
    [SerializeField] private AudioSource startSource;
    [SerializeField] private AudioClip[] startClips;

    [Header("\nSent sounds")]
    [SerializeField] private AudioSource sentSource;
    [SerializeField] private AudioClip[] sentClips;

    [Header("\nWalk sounds")]
    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioClip[] walkingClips;

    private float walkTime = .9f;
    private float minWalkTime = .8f;
    private int currentSound = 0;

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
        if (sentClips.Length > 0 && (!pushing && !punching))
        {
            sentSource.clip = sentClips[Random.Range(0, sentClips.Length)];
            sentSource.Play();
        }

        if (e.CompareTag("Bridge"))
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

        followerBehaviour.transform.forward = CaptainCatBehaviour.currentPos - transform.position;
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
                followerBehaviour.transform.forward = CaptainCatBehaviour.currentPos - followerBehaviour.transform.position;

                startedPushing = false;
                pushing = false;
                animator.SetBool("IsPushing", pushing);
                return;
            }

            if (Vector3.Distance(transform.position, attackingTransform.position) < 2f)
            {
                followerBehaviour.transform.forward = attackingTransform.position - followerBehaviour.transform.position;

                startedPushing = true;
                animator.SetBool("IsPushing", pushing);
            }
        }

        if (punching && !startedPunching)
        {
            if (attackingTransform == null)
            {
                followerBehaviour.transform.forward = CaptainCatBehaviour.currentPos - followerBehaviour.transform.position;

                startedPunching = false;
                punching = false;
                animator.SetBool("IsPunching", punching);
                return;
            }

            if (Vector3.Distance(transform.position, attackingTransform.position) < 4f)
            {
                followerBehaviour.transform.forward = attackingTransform.position - followerBehaviour.transform.position;

                startedPunching = true;
                animator.SetBool("IsPunching", punching);
            }
        }

        if (walking && !startedPunching && !startedPushing)
        {
            WalkingSounds();
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

    private void WalkingSounds()
    {
        if (walkTime < minWalkTime)
        {
            walkTime += Time.deltaTime;
            return;
        }

        else
        {
            walkTime = 0;
            if (walkingClips.Length > 0)
            {
                walkingSource.clip = walkingClips[currentSound];
                walkingSource.Play();

                NextWalkingSound();
            }
        }
    }

    private void NextWalkingSound()
    {
        currentSound++;

        if (currentSound >= walkingClips.Length)
        {
            currentSound = 0;
        }
    }
}