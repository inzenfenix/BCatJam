using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FollowerBehaviour))]
public class FollowerUI : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Update()
    {
        Vector3 forward = transform.position - Camera.main.transform.position;
        forward.y = 1;

        image.transform.forward = forward;
        image.transform.Rotate(new Vector3(0, 0, -90));

        if (GameManager.instance.HitFollower(out Transform follower))
        {
            if(follower == this.transform)
                EnableImage();
        }

        else
        {
            DisableImage();
        }
    }

    public void DisableImage()
    {
        if (image.enabled)
        {
            image.enabled = false;
        }
    }

    public void EnableImage()
    {
        if(!image.enabled)
        {
            image.enabled = true;
        }
        
    }
}
