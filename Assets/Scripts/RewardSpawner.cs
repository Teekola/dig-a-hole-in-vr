using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardSpawner : MonoBehaviour
{

    [SerializeField] private RewardData[] rewardTypes;
    void Start()
    {
        TrySpawnReward(transform.position);
    }

    void TrySpawnReward(Vector3 position)
    {
        float y = position.y;

        foreach (var reward in rewardTypes)
        {
            if (y >= reward.minY && y <= reward.maxY)
            {
                if (Random.value < reward.spawnChance)
                {
                    Instantiate(reward.prefab, position, Random.rotation);
                    break; // Only spawn one reward per call
                }
            }
        }
    }
}
