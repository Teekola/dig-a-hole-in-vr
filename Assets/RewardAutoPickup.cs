using UnityEngine;

public class RewardAutoPickupZone : MonoBehaviour
{
    private InventoryController inventoryController;
    private float voxelCheckRadius = 0.005f;

    private void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (inventoryController.GetUpgradeLevel(UpgradeType.Magnet) < 1) return;

        RewardPickup reward = other.GetComponent<RewardPickup>();
        if (reward != null && !reward.GetIsPickedUp())
        {
            if (!IsTouchingVoxel(reward.transform.position))
            {
                reward.Pickup();
            }
        }
    }


    private bool IsTouchingVoxel(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, voxelCheckRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Voxel"))
                return true;
        }
        return false;
    }
}
