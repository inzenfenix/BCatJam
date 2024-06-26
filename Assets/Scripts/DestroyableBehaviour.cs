using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestroyableBehaviour : MonoBehaviour
{
    [SerializeField] private int numberOfCatsToDestroy;
    [SerializeField] private GameObject destroyedVersion;

    private List<Transform> currentCatsOn;
    [SerializeField] private float radius = 6f;

    private int catMask = 12;

    private bool beingAttacked = false;

    private void Awake()
    {
        currentCatsOn = new List<Transform>();
    }

    private void OnEnable()
    {
        CaptainCatBehaviour.onAttackObject += CaptainCatBehaviour_onAttackObject;
    }

    private void CaptainCatBehaviour_onAttackObject(object sender, Transform e)
    {
        if(e == this.transform)
        {
            beingAttacked = true;
        }
    }

    private void OnDisable()
    {
        CaptainCatBehaviour.onAttackObject -= CaptainCatBehaviour_onAttackObject;
    }

    private void Update()
    {
        if (!beingAttacked)
        {
            if (currentCatsOn != null)
                currentCatsOn = new List<Transform>();
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << catMask);

        if (colliders.Length > 0)
        {
            foreach(Collider collider in colliders)
            {
                if(!currentCatsOn.Contains(collider.transform) && collider.CompareTag("Follower"))
                {
                    currentCatsOn.Add(collider.transform);
                }
            }
        }

        if(numberOfCatsToDestroy <= currentCatsOn.Count)
        {
            Destroy(this.gameObject);
        }
    }
}
