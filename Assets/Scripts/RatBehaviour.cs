using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RatBehaviour : MonoBehaviour
{
    public event EventHandler<Transform> onStartedAttacking;
    public event EventHandler onFinishedAttacking;

    [SerializeField] Transform[] transformsPositions;
    private Vector3[] positions;

    private NavMeshAgent agent;

    private int curPos = 0;

    [HideInInspector]
    public bool battling;

    private float radius = 2f;

    private Transform currentCat;

    [SerializeField] private LayerMask catMask;

    private float delay = 0f;
    private float delayTime = 1.25f;

    [SerializeField] private int damagePerTickRate = 1;

    public int health = 3;

    private bool changing = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        positions = new Vector3[transformsPositions.Length];

        for(int i = 0; i < transformsPositions.Length; i++)
        {
            positions[i] = transformsPositions[i].position;
        }
    }

    private void Update()
    {
        if(battling && currentCat != null)
        {
            agent.destination = currentCat.position;

            if(delay < delayTime)
            {
                delay += Time.deltaTime;
                return;
            }

            else
            {
                delay = 0;
            }

            currentCat.GetComponent<FollowerBehaviour>().GetHit(damagePerTickRate);

            if(currentCat.GetComponent<FollowerBehaviour>().GetHealth() <= 0)
            {
                onFinishedAttacking?.Invoke(this, EventArgs.Empty);
                currentCat = null;
            }

            return;
        }

        if(battling && currentCat == null)
        {

            Vector3 closestPosition = positions[0];
            float minDistance = Vector3.Distance(transform.position, closestPosition);
            curPos = 0;

            for(int i = 1; i < positions.Length; i++)
            {
                Vector3 currentPosition = positions[i];

                if(Vector3.Distance(currentPosition, transform.position) < minDistance)
                {
                    closestPosition = currentPosition;
                    curPos = i;
                }
            }

            delay = 0;
            battling = false;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, catMask);

        if (colliders.Length > 0)
        {
            foreach(Collider collider in colliders)
            {
                FollowerBehaviour tempCat = collider.GetComponent<FollowerBehaviour>();

                if (tempCat.GetHealth() <= 0 || Mathf.Abs(transform.position.y - tempCat.transform.position.y) > .6f)
                {
                    continue;
                }

                else
                {
                    onStartedAttacking?.Invoke(this, tempCat.transform);
                    currentCat = tempCat.transform;
                    battling = true;
                    break;
                }
            }            

            return;
        }

        if (agent.destination != positions[curPos] && !changing)
        {
            StartCoroutine(ChangingPos());
        }

        if(Vector3.Distance(transform.position, agent.destination) < 2f && !changing)
        {
            NextPos();
        }
    }

    private IEnumerator ChangingPos()
    {
        changing = true;

        yield return new WaitForSeconds(1f);

        agent.destination = positions[curPos];
        changing = false;
    }

    private void NextPos()
    {
        curPos++;

        if (curPos >= positions.Length)
        {
            curPos = 0;
            return;
        }
    }
}
