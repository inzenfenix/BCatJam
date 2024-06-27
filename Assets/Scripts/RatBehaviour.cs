using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RatBehaviour : MonoBehaviour
{
    [SerializeField] Transform[] transformsPositions;
    private Vector3[] positions;

    private NavMeshAgent agent;

    private int curPos = 0;

    private bool battling;

    private float radius = 3f;

    private Transform currentCat;

    [SerializeField] private LayerMask catMask;

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

            battling = false;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, catMask);

        if (colliders.Length > 0)
        {
            int randIndex = UnityEngine.Random.Range(0, colliders.Length);
            Transform currentCat = colliders[randIndex].transform;
            battling = true;

            return;
        }

        if (agent.destination != positions[curPos])
        {
            agent.destination = positions[curPos];
        }

        if(Vector3.Distance(transform.position, agent.destination) < 2f)
        {
            NextPos();
        }
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
