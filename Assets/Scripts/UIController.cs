using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public Slider slider;

    private void Start()
    {
        buttonText.text = "No delay";
    }


    public void ChangeColorDelay()
    {
        Rotate.wait = !Rotate.wait;
        if (Rotate.wait)
        {
            buttonText.text = "Delay is " + Rotate.waitTime + " seconds";
        }
        else
        {
            buttonText.text = "No delay";
        }
    }

    public void ChangeDelayTime()
    {
        Rotate.waitTime = slider.value;
        if (Rotate.wait)
        {
            buttonText.text = "Delay is " + Rotate.waitTime + " seconds";
        }
    }
}
