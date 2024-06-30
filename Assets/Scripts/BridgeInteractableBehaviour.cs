using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Collider))]
public class BridgeInteractableBehaviour : FollowerInteractableBehaviour
{

    [SerializeField] private NavMeshObstacle obstacle;

    private Quaternion originalRotation;
    private Quaternion finalRotation;

    private bool gettingPushed = false;

    [SerializeField] private GameObject canvas;

    [SerializeField] private int floorMask;

    protected override void Awake()
    {
        base.Awake();

        obstacle.enabled = true;

        obstacle.gameObject.SetActive(true);

        originalRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -90);
        finalRotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, transform.rotation.z);

        transform.rotation = originalRotation;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDeath()
    {
        if (gettingPushed) return;

        gettingPushed = true;

        StartCoroutine(RotateBridge());
    }

    private IEnumerator RotateBridge()
    {
        float time = 0;
        transform.gameObject.layer = LayerMask.NameToLayer("Floor"); ;
        canvas.SetActive(false);

        float speed = 1.0f;

        while (time < 1.0f)
        {
            time += Time.deltaTime * speed;
            speed += .15f;
            transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, time);
            yield return new WaitForSeconds(.01f);
        }

        yield return new WaitForSeconds(2f);

        obstacle.enabled = false;
    }
}
