using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> availableUpgrades;
    public UnityEvent onMoneyChanged;
    public UnityEvent onUpgradesChanged;

    private Dictionary<UpgradeType, int> upgradeLevels = new();
    private int money;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var upgrade in availableUpgrades)
        {
            upgradeLevels[upgrade.type] = 0;
        }
    }

    public int GetMoney()
    {
        return money;
    }

    public List<UpgradeData> GetAvailableUpgrades()
    {
        return availableUpgrades;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        onMoneyChanged?.Invoke();
    }

    public void ReduceMoney(int amount)
    {
        money -= amount;
        onMoneyChanged?.Invoke();
    }

    public int GetUpgradeLevel(UpgradeType type) => upgradeLevels.TryGetValue(type, out var level) ? level : 0;

    public float GetValue(UpgradeType type)
    {
        var upgradeData = availableUpgrades.Find(u => u.type == type);
        if (upgradeData == null)
        {
            return 0f;
        }

        int currentLevel = GetUpgradeLevel(type);
            

        // Clamp level to max valid index in levelValues
        if (currentLevel >= upgradeData.levelValues.Count)
            currentLevel = upgradeData.levelValues.Count - 1;

        return upgradeData.levelValues[currentLevel];
    }


    public bool CanAffordUpgrade(UpgradeType type)
    {
        var upgrade = availableUpgrades.Find(u => u.type == type);

        if (upgrade == null) return false;

        int cost = upgrade.GetCostForLevel(GetUpgradeLevel(type));
        return money >= cost && cost > 0;
    }

    public void PurchaseUpgrade(UpgradeType type)
    {
        var upgrade = availableUpgrades.Find(u => u.type == type);
        if (upgrade == null) return;

        int level = GetUpgradeLevel(type);
        int cost = upgrade.GetCostForLevel(level);

        if (cost > 0 && money >= cost)
        {
            ReduceMoney(cost);
            upgradeLevels[type]++;
            onUpgradesChanged?.Invoke();
        }
    }
}
