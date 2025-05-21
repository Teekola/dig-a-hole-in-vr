using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Upgrades/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public UpgradeType type;
    public List<int> levelCosts;
    public List<float> levelValues;

    public int MaxLevel => levelCosts.Count;

    public int GetCostForLevel(int level)
    {
        if (level < levelCosts.Count)
            return levelCosts[level];
        return -1;
    }
}
