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

    [Header("\nWalk sounds")]
    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioClip[] walkingClips;

    private float walkTime = .9f;
    private float minWalkTime = .8f;
    private int currentSound = 0;

    [Header("\nPunching sounds")]
    [SerializeField] private AudioSource punchingSource;
    [SerializeField] private AudioClip[] punchingClips;

    private float punchingTime = .9f;
    private float minPunchingTime = .8f;
    private int currentPunchingSound = 0;

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
            PunchingSounds();

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

        if (walking && !punching)
        {
            WalkingSounds();
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

    private void PunchingSounds()
    {
        if (punchingTime < minPunchingTime)
        {
            punchingTime += Time.deltaTime;
            return;
        }

        else
        {
            punchingTime = 0;
            if (punchingClips.Length > 0)
            {
                punchingSource.clip = punchingClips[currentPunchingSound];
                punchingSource.Play();

                NextPunchingSound();
            }
        }
    }

    private void NextPunchingSound()
    {
        currentPunchingSound++;

        if (currentPunchingSound >= punchingClips.Length)
        {
            currentPunchingSound = 0;
        }
    }
}
