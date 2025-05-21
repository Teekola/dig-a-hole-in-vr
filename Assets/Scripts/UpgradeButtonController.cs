using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonController : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private UpgradeType upgradeType;


    private AudioSource audioSource;
    private Button upgradeButton;

    private void Awake()
    {
        upgradeButton = GetComponent<Button>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnEnable()
    {
        inventoryController.onMoneyChanged.AddListener(UpdateButtonState);
        inventoryController.onUpgradesChanged.AddListener(UpdateButtonState);
        UpdateButtonState();
    }

    private void OnDisable()
    {
        inventoryController.onMoneyChanged.RemoveListener(UpdateButtonState);
        inventoryController.onUpgradesChanged.RemoveListener(UpdateButtonState);
    }

    private void UpdateButtonState()
    {
        var upgradeData = inventoryController.GetAvailableUpgrades().Find(u => u.type == upgradeType);
        if (upgradeData == null)
        {
            upgradeButton.interactable = false;
            return;
        }

        int currentLevel = inventoryController.GetUpgradeLevel(upgradeType);
        int cost = upgradeData.GetCostForLevel(currentLevel);

        // Disable if max level reached (cost < 0) or if player can't afford
        bool canPurchase = cost >= 0 && inventoryController.GetMoney() >= cost;
        upgradeButton.interactable = canPurchase;
    }

    public void Upgrade()
    {
        inventoryController.PurchaseUpgrade(upgradeType);
        audioSource.Play();
    }
}
