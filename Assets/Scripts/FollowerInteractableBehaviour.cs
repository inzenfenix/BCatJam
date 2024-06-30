using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerInteractableBehaviour : MonoBehaviour
{
    public static event EventHandler onObjectGettingDestroyed;
    public event EventHandler onChangeAmountOfCats;

    public int numberOfCatsToDestroy;
    [SerializeField] protected GameObject destroyedVersion;

    protected List<Transform> currentCatsOn;
    [SerializeField] protected float radius = 6f;

    protected int catMask = 12;

    protected bool beingAttacked = false;

    [HideInInspector]
    public bool gettingDestroyed = false;

    public float timeToBeDestroyed = 5f;

    [HideInInspector]
    public float currentTime = 0f;

    protected float interactionSpeedMultiplier = 1f;
    protected float interactionSpeedMultiplierDefault = 1f;

    [SerializeField] protected float fillMeterAmount = .1f; 

    protected virtual void Awake()
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

    public void BeingAttacked()
    {
        beingAttacked = true;
    }

    private void OnDisable()
    {
        CaptainCatBehaviour.onAttackObject -= CaptainCatBehaviour_onAttackObject;
    }

    protected virtual void Update()
    {
        if(gettingDestroyed)
        {
            currentTime += Time.deltaTime * interactionSpeedMultiplier;
            if(currentTime > timeToBeDestroyed)
            {
                if (this.CompareTag("Rat"))
                {
                    GameManager.CurrentFillMeter -= fillMeterAmount;
                }

                if(this.CompareTag("Interactable"))
                {
                    GameManager.CurrentFillMeter += fillMeterAmount;
                }

                onObjectGettingDestroyed?.Invoke(this, EventArgs.Empty);
                currentTime = 0;
                OnDeath();
            }
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
                if(!currentCatsOn.Contains(collider.transform) && collider.CompareTag("Follower") && collider.GetComponent<FollowerBehaviour>().attacking)
                {
                    currentCatsOn.Add(collider.transform);
                    onChangeAmountOfCats?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        if(numberOfCatsToDestroy <= currentCatsOn.Count && !gettingDestroyed)
        {
            gettingDestroyed = true;
            //Destroy(this.gameObject);
        }

        if(numberOfCatsToDestroy <= currentCatsOn.Count && gettingDestroyed)
        {

            float multiplier = (float)(currentCatsOn.Count - numberOfCatsToDestroy) / 5;

            interactionSpeedMultiplier = interactionSpeedMultiplierDefault + multiplier;
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(this.gameObject);
    }

    public int GetAmountOfCats()
    {
        return currentCatsOn.Count;
    }
}
