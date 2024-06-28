using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

public class MeterUI : MonoBehaviour
{
    [SerializeField] private ProceduralImage image;

    private float currentFillingAmount;

    private void Start()
    {
        currentFillingAmount = GameManager.CurrentFillMeter;
        image.fillAmount = currentFillingAmount;
    }

    private void Update()
    {
        if(currentFillingAmount !=  GameManager.CurrentFillMeter)
        {
            currentFillingAmount = GameManager.CurrentFillMeter;
            image.fillAmount = currentFillingAmount;
        }
    }
}

