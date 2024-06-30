using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDestructionBehaviour : FollowerInteractableBehaviour
{
    private RatBehaviour ratBehaviour;


    protected override void Awake()
    {
        base.Awake();
        ratBehaviour = GetComponent<RatBehaviour>();
    }

    protected override void Update()
    {
        if(!ratBehaviour.battling)
        {
            gettingDestroyed = false;
            currentTime = 0;
            return;
        }

        base.Update();
    }

    protected override void OnDeath()
    {
        ratBehaviour.health--;

        if(ratBehaviour.health <= 0 )
        {
            Destroy(this.gameObject);
        }
    }
}
