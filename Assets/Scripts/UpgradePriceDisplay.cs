using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradePriceDisplay : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private UpgradeType upgradeType;
    private TMP_Text priceText;

    private void Awake()
    {
        priceText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        inventoryController.onMoneyChanged.AddListener(UpdatePriceDisplay);
        inventoryController.onUpgradesChanged.AddListener(UpdatePriceDisplay);
        UpdatePriceDisplay();
    }

    private void OnDisable()
    {
        inventoryController.onMoneyChanged.RemoveListener(UpdatePriceDisplay);
        inventoryController.onUpgradesChanged.RemoveListener(UpdatePriceDisplay);
    }

    public void UpdatePriceDisplay()
    {
        var upgradeData = inventoryController.GetAvailableUpgrades().Find(u => u.type == upgradeType);
        if (upgradeData == null)
        {
            priceText.text = "N/A";
            return;
        }

        int currentLevel = inventoryController.GetUpgradeLevel(upgradeType);
        int cost = upgradeData.GetCostForLevel(currentLevel);

        if (cost < 0)
        {
            priceText.text = "Max";
        }
        else
        {
            priceText.text = cost.ToString() + " €";
        }
    }
}
