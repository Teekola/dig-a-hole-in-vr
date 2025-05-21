using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHueBasedOnEnergy : MonoBehaviour
{
    private EnduranceController enduranceController;
    private Image panelImage;

    private void Awake()
    {
        panelImage = GetComponent<Image>();
        enduranceController = FindObjectOfType<EnduranceController>();
    }

    private void OnEnable()
    {
        if (enduranceController != null)
        {
            enduranceController.onEnduranceChanged.AddListener(UpdatePanelColor);
            UpdatePanelColor();
        }
    }

    private void OnDisable()
    {
        if (enduranceController != null)
        {
            enduranceController.onEnduranceChanged.RemoveListener(UpdatePanelColor);
        }
    }

    private void UpdatePanelColor()
    {
        float endurance = enduranceController.GetEndurance();
        float t = Mathf.Clamp01(endurance / 100f);
        panelImage.color = Color.Lerp(new Color(0.9f, 0.5f, 0.5f), new Color(0.5f, 0.9f, 0.5f), t);
    }
}
