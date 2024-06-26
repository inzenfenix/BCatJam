using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestroyableBehaviour : MonoBehaviour
{
    public static event EventHandler onObjectGettingDestroyed;
    public event EventHandler onChangeAmountOfCats;

    public int numberOfCatsToDestroy;
    [SerializeField] private GameObject destroyedVersion;

    private List<Transform> currentCatsOn;
    [SerializeField] private float radius = 6f;

    private int catMask = 12;

    private bool beingAttacked = false;

    [HideInInspector]
    public bool gettingDestroyed = false;

    public float timeToBeDestroyed = 5f;

    [HideInInspector]
    public float currentTime = 0f;

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
        if(gettingDestroyed)
        {
            currentTime += Time.deltaTime;
            if(currentTime > timeToBeDestroyed)
            {
                Destroy(this.gameObject);
            }

            return;
        }

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
                    onChangeAmountOfCats?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        if(numberOfCatsToDestroy <= currentCatsOn.Count)
        {
            onObjectGettingDestroyed?.Invoke(this, EventArgs.Empty);
            gettingDestroyed = true;
            //Destroy(this.gameObject);
        }
    }

    public int GetAmountOfCats()
    {
        return currentCatsOn.Count;
    }
}
