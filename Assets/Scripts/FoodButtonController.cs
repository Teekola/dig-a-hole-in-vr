using UnityEngine;
using UnityEngine.UI;

public class FoodButtonController : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private int enduranceValue;

    private Button buyButton;
    private AudioSource audioSource;
    private InventoryController inventoryController;
    private EnduranceController enduranceController;

    private void Awake()
    {
        buyButton = GetComponent<Button>();
        inventoryController = FindObjectOfType<InventoryController>();
        enduranceController = FindObjectOfType<EnduranceController>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void OnEnable()
    {
        inventoryController.onMoneyChanged.AddListener(UpdateButtonState);
        enduranceController.onEnduranceChanged.AddListener(UpdateButtonState);
        UpdateButtonState();
    }

    private void OnDisable()
    {
        inventoryController.onMoneyChanged.RemoveListener(UpdateButtonState);
        enduranceController.onEnduranceChanged.RemoveListener(UpdateButtonState);
    }

    private void UpdateButtonState()
    {
        // Disable if max level reached (cost < 0) or if player can't afford
        bool canPurchase = inventoryController.GetMoney() >= price && enduranceController.GetEndurance() < 100;
        buyButton.interactable = canPurchase;
    }

    public void Buy()
    {
        inventoryController.ReduceMoney(price);
        enduranceController.IncreaseEndurance(enduranceValue);
        audioSource.Play();
    }
}
