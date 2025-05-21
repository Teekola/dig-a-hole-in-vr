using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupTextEffectController : MonoBehaviour
{
    [SerializeField] private RewardData rewardData;
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = rewardData.rewardName + " + " + rewardData.value.ToString() + "€";
    }
}
