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

    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioClip[] walkingClips;

    private float walkTime = .8f;
    private float minWalkTime = .5f;
    private int currentSound = 0;

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
        if(walking)
        {
            WalkingSounds();
        }

        else
        {
            walkTime = .3f;
        }

        if (agent.velocity.magnitude > 0.75f && !walking)
        {
            walking = true;
            animator.SetBool("IsWalking", walking);
        }

        if(agent.velocity.magnitude <= 0.75f && walking)
        {
            walking = false;
            animator.SetBool("IsWalking", walking);
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

        if(currentSound >= walkingClips.Length)
        {
            currentSound = 0;
        }
    }
}
