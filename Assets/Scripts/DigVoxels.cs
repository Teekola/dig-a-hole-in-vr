using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DigVoxels : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject diggingEffect;
    [SerializeField] private GameObject voxelPrefab;

    [Header("Configuration")]
    private float shrinkStepAmount;
    [SerializeField] private float minVoxelScale;

    private InventoryController inventoryController;
    private AudioSource audioSource;

    private float voxelSize;
    private HashSet<Collider> currentlyTouchedVoxels = new HashSet<Collider>();
    private HashSet<Collider> currentlyTouchedRewards = new HashSet<Collider>();
    private HashSet<Vector3> dugVoxels = new HashSet<Vector3>();

    private void Start()
    {
        voxelSize = voxelPrefab.transform.localScale.x;
        inventoryController = FindObjectOfType<InventoryController>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Voxel"))
        {
            currentlyTouchedVoxels.Add(other);
        }

        if (other.CompareTag("Reward"))
        {
            currentlyTouchedRewards.Add(other);
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Voxel"))
        {
            currentlyTouchedVoxels.Remove(other);
        }

        if (other.CompareTag("Reward"))
        {
            currentlyTouchedRewards.Remove(other);
        }     
    }

    public void CheckRewardHits()
    {
        foreach (var reward in currentlyTouchedRewards)
        {
            if (reward == null) continue;
           
            var destroyable = reward.GetComponent<Destroyable>();
            var rewardData = reward.GetComponent<RewardPickup>().GetRewardData();

            // Destroy Reward
            destroyable.DestroySelf();

            // Instantiate and destroy particle effect
            var effect = Instantiate(rewardData.destroyPrefab, reward.transform.position, Quaternion.identity);
            var ps = effect.GetComponent<ParticleSystem>();
            Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        currentlyTouchedRewards.Clear();
    }

    public void DigTouchedVoxels()
    {
        if (currentlyTouchedVoxels.Count == 0) return;

        audioSource.Play();

        List<Collider> voxelsToRemove = new List<Collider>();

        foreach (var voxelCollider in currentlyTouchedVoxels)
        {
            if (voxelCollider == null) continue;

            Transform voxelTransform = voxelCollider.transform;

            GameObject effect = Instantiate(diggingEffect, voxelTransform.position, Quaternion.identity);
            Destroy(effect, 3f);

            Vector3 worldHitDir = (voxelTransform.position - transform.position).normalized;
            Vector3 localHitDir = voxelTransform.InverseTransformDirection(worldHitDir);
            Vector3 localScale = voxelTransform.localScale;
            Vector3 voxelPos = voxelTransform.position;

            // Spawn adjacent voxels
            Vector3[] directions = { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back };

            foreach (Vector3 dir in directions)
            {
                Vector3 neighborPos = voxelPos + dir * voxelSize;
                Vector3Int gridCoord = ToGridCoord(neighborPos);
                Vector3 gridPos = new Vector3(gridCoord.x, gridCoord.y, gridCoord.z) * voxelSize;

                if (!dugVoxels.Contains(ToGridCoord(gridPos)) && gridPos.y < 0f)
                {
                    Collider[] hits = Physics.OverlapBox(gridPos, Vector3.one * voxelSize * 0.45f);
                    bool hasVoxel = false;
                    foreach (var hit in hits)
                    {
                        if (hit.CompareTag("Voxel"))
                        {
                            hasVoxel = true;
                            break;
                        }
                    }

                    if (!hasVoxel)
                    {
                        Instantiate(voxelPrefab, gridPos, Quaternion.identity);
                        dugVoxels.Add(ToGridCoord(gridPos));
                    }
                }
            }

            // Find the axis with the strongest hit direction (X=0, Y=1, Z=2)
            int dominantAxis = 0;
            float maxMagnitude = Mathf.Abs(localHitDir[0]);

            for (int i = 1; i < 3; i++)
            {
                if (Mathf.Abs(localHitDir[i]) > maxMagnitude)
                {
                    dominantAxis = i;
                    maxMagnitude = Mathf.Abs(localHitDir[i]);
                }
            }

            // Shrink along only the dominant axis
            float sign = Mathf.Sign(localHitDir[dominantAxis]);
            float shrinkAmount = inventoryController.GetValue(UpgradeType.DiggingPower);
            float newScaleValue = localScale[dominantAxis] - shrinkAmount;

            if (newScaleValue <= minVoxelScale)
            {
                dugVoxels.Add(ToGridCoord(voxelTransform.position));
                var destroyable = voxelCollider.GetComponent<Destroyable>();
                if (destroyable != null) destroyable.DestroySelf();
                voxelsToRemove.Add(voxelCollider);
            }
            else
            {
                localScale[dominantAxis] = newScaleValue;
                Vector3 localOffset = Vector3.zero;
                localOffset[dominantAxis] = (shrinkAmount / 2f) * -sign;
                voxelTransform.position -= voxelTransform.TransformDirection(localOffset);
                voxelTransform.localScale = localScale;
            }
            voxelTransform.localScale = localScale;
        }

        // Remove destroyed voxels from the set
        foreach (var removed in voxelsToRemove)
        {
            currentlyTouchedVoxels.Remove(removed);
        }
    }

    Vector3Int ToGridCoord(Vector3 pos)
    {
        return Vector3Int.RoundToInt(pos / voxelSize);
    }
}
