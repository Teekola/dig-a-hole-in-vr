using UnityEngine;

[CreateAssetMenu(menuName = "Digging/Reward")]
public class RewardData : ScriptableObject
{
    public string rewardName;
    public GameObject prefab;
    public GameObject destroyPrefab;
    public GameObject pickupTextEffect;
    public float spawnChance; // 0.0 to 1.0
    public float minY;
    public float maxY;
    public int value;
}
