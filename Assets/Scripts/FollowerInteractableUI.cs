using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FollowerInteractableBehaviour))]
public class FollowerInteractableUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image fillImage;
    [SerializeField] Image borderImage;

    private FollowerInteractableBehaviour mDestroyable;

    private float fadeIn = 9f;

    private void Awake()
    {
        mDestroyable = GetComponent<FollowerInteractableBehaviour>();
        text.text = "0/" + mDestroyable.numberOfCatsToDestroy.ToString();

    }

    private void Update()
    {
        text.transform.forward =  transform.position - Camera.main.transform.position;
        fillImage.transform.forward = transform.position - Camera.main.transform.position;
        borderImage.transform.forward = transform.position - Camera.main.transform.position;

        if(mDestroyable.gettingDestroyed)
        {

            borderImage.color += new Color(0, 0, 0, Time.deltaTime * fadeIn);

            fillImage.fillAmount = mDestroyable.currentTime / mDestroyable.timeToBeDestroyed;
        }

        else
        {
            borderImage.color -= new Color(0, 0, 0, borderImage.color.a);

            fillImage.fillAmount = 0;
        }
    }

    private void OnEnable()
    {
        mDestroyable.onChangeAmountOfCats += MDestroyable_onChangeAmountOfCats;
    }

    private void OnDisable()
    {
        mDestroyable.onChangeAmountOfCats -= MDestroyable_onChangeAmountOfCats;
    }

    private void MDestroyable_onChangeAmountOfCats(object sender, System.EventArgs e)
    {
        text.text = mDestroyable.GetAmountOfCats().ToString() + "/" + mDestroyable.numberOfCatsToDestroy.ToString();
    }
}
