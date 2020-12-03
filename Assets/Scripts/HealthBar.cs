using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{


    public Slider slider;

    public Image fillImage;

    void Start()
    {
        slider.wholeNumbers = true;
    }

    public void SetValue(int value)
    {
        slider.value = value;

        if (slider.normalizedValue <= 0.2)
        {
            fillImage.color = Color.red;
        }
        else if(slider.normalizedValue <= 0.4)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
    }
}
