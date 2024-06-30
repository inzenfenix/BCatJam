using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CaptainCatBehaviour : MonoBehaviour
{
    public static event EventHandler<Transform> onAttackObject;
    public static event EventHandler<Transform> onAttackObjectForUI;
    public static event EventHandler<Vector3> onMoving;

    public static Vector3 currentPos;
    public static Vector3 behindPos;

    private NavMeshAgent agent;

    //private bool attacking;

    private List<FollowerBehaviour> cats;

    private int catsToThrow = 0;
    private int catsThrown = 0;

    private float delay = .4f;
    private float delayTime = .35f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.position;

        cats = new List<FollowerBehaviour>();
    }

    private void OnEnable()
    {
        FollowerInteractableBehaviour.onObjectGettingDestroyed += DestroyableBehaviour_onObjectDestroyed;
    }

    private void DestroyableBehaviour_onObjectDestroyed(object sender, EventArgs e)
    {
        //attacking = false;
    }

    private void Update()
    {
        if(cats.Count > 0 && catsThrown != 0)
        {
            bool isActuallyAttacking = false;

            for(int i = 0; i < cats.Count; i++)
            {
                if (cats[i].attacking)
                {
                    isActuallyAttacking = true;
                    break;
                }
            }

            if(!isActuallyAttacking)
            {
                catsThrown = 0;
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.instance.HitFollower(out Transform follower))
            {
                FollowerBehaviour cat = follower.GetComponent<FollowerBehaviour>();

                if (!cats.Contains(cat))
                {
                    cat.onDeath += Cat_onDeath;
                    cat.onFinishedInteracting += Cat_onFinishedInteracting;
                    cats.Add(cat);
                    cat.StartFollowing();
                }
                return;
            }

            if (GameManager.instance.HitDestroyableInteractable(out Transform destroyablePos))
            {
                if (catsThrown >= cats.Count || cats.Count <= 0) 
                    return;

                onAttackObject?.Invoke(this, destroyablePos);
                onAttackObjectForUI?.Invoke(this, destroyablePos);

                return;
            }

            if (GameManager.instance.HitFloor(out Vector3 pos))
            {
                agent.destination = pos;
                onMoving?.Invoke(this, pos * 0.95f);

                return;
            }

            
        }
        if (Input.GetMouseButton(1))
        {
            if(cats.Count <= 0)
            {
                return;
            }

            if(delay < delayTime)
            {
                delay += Time.deltaTime;
                return;
            }

            else
            {
                delay = 0;
            }

            if(catsThrown >= cats.Count)
            {
                return;
            }

            if (GameManager.instance.HitDestroyableInteractable(out Transform interactable))
            {
                interactable.GetComponent<FollowerInteractableBehaviour>().BeingAttacked();
                onAttackObjectForUI?.Invoke(this, interactable);

                if (catsToThrow == 0 || catsToThrow == 1)
                {
                    catsToThrow = catsThrown + 1;

                    for (int i = catsThrown; i < catsToThrow; i++)
                    {
                        if(i >= cats.Count || i < 0)
                        {
                            break;
                        }

                        if (i < cats.Count)
                        {
                            if (!cats[i].attacking)
                            {
                                catsThrown++;
                                cats[i].AttackObject(interactable);
                            }
                        }
                    }

                    return;
                }

                catsToThrow *= 2;

                for (int i = catsThrown; i < catsToThrow; i++)
                {
                    if (i >= cats.Count)
                    {
                        break;
                    }

                    if (!cats[i].attacking)
                    {
                        catsThrown++;
                        cats[i].AttackObject(interactable);
                    }
                }

                return;
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            catsToThrow = 0;
            delay = delayTime + .1f;
        }

        behindPos = -transform.forward;
        currentPos = transform.position;
    }

    private void Cat_onFinishedInteracting(object sender, EventArgs e)
    {
        if (catsThrown <= 0) return;

        catsThrown--;
    }

    private void Cat_onDeath(object sender, EventArgs e)
    {
        FollowerBehaviour cat = sender as FollowerBehaviour;

        if (cats.Contains(cat))
            cats.Remove(cat);

        else return;

        cat.onFinishedInteracting -= Cat_onFinishedInteracting;;
        cat.onDeath -= Cat_onDeath;
    }
}
