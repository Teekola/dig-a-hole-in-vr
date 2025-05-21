using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnduranceDisplay : MonoBehaviour
{
    private TMP_Text enduranceText;
    private EnduranceController enduranceController;

    private void Awake()
    {
        enduranceText = GetComponent<TMP_Text>();
        enduranceController = FindObjectOfType<EnduranceController>();
    }

    private void OnEnable()
    {
        enduranceController.onEnduranceChanged.AddListener(UpdateDisplay);
        UpdateDisplay();
    }

    private void OnDisable()
    {
        enduranceController.onEnduranceChanged.RemoveListener(UpdateDisplay);
    }

    public void UpdateDisplay()
    {
        enduranceText.text = Mathf.RoundToInt(enduranceController.GetEndurance()).ToString() + " %";
    }
}
