using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent (typeof(Collider))]
public class BridgeInteractableBehaviour : FollowerInteractableBehaviour
{

    private NavMeshObstacle obstacle;
    private Collider obstacleCollider;

    private Quaternion originalRotation;
    private Quaternion finalRotation;

    private bool gettingPushed = false;

    [SerializeField] private GameObject canvas;

    protected override void Awake()
    {
        base.Awake();

        obstacleCollider = GetComponent<Collider>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;

        originalRotation = Quaternion.Euler(0, 0, 0);
        finalRotation = Quaternion.Euler(0, 0, -90);

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
        transform.gameObject.layer = 0;
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
        obstacleCollider.enabled = false;
    }
}
