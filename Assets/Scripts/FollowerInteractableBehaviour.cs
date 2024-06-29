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

    private float interactionSpeedMultiplier = 1f;
    private float interactionSpeedMultiplierDefault = 1f;

    [SerializeField] private float fillMeterAmount = .1f;

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

    public void BeingAttacked()
    {
        beingAttacked = true;
    }

    private void OnDisable()
    {
        CaptainCatBehaviour.onAttackObject -= CaptainCatBehaviour_onAttackObject;
    }

    private void Update()
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

                if(this.CompareTag("Destroyable"))
                {
                    GameManager.CurrentFillMeter += fillMeterAmount;
                }
                Destroy(this.gameObject);
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
            onObjectGettingDestroyed?.Invoke(this, EventArgs.Empty);
            gettingDestroyed = true;
            //Destroy(this.gameObject);
        }

        else if(numberOfCatsToDestroy <= currentCatsOn.Count && gettingDestroyed)
        {
            float multiplier = (currentCatsOn.Count - numberOfCatsToDestroy) / 10;

            interactionSpeedMultiplier = interactionSpeedMultiplierDefault + multiplier;
        }
    }

    public int GetAmountOfCats()
    {
        return currentCatsOn.Count;
    }
}
