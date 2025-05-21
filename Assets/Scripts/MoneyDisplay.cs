using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    private TMP_Text priceText;

    private void Awake()
    {
        priceText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        inventoryController.onMoneyChanged.AddListener(UpdateMoneyDisplay);
        inventoryController.onUpgradesChanged.AddListener(UpdateMoneyDisplay);
        UpdateMoneyDisplay();
    }

    private void OnDisable()
    {
        inventoryController.onMoneyChanged.RemoveListener(UpdateMoneyDisplay);
        inventoryController.onUpgradesChanged.RemoveListener(UpdateMoneyDisplay);
    }

    public void UpdateMoneyDisplay()
    {
        priceText.text = inventoryController.GetMoney().ToString() + " €";
    }
}
