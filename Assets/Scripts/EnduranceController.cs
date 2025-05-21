using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnduranceController : MonoBehaviour
{
    private float endurance = 100f;
    public UnityEvent onEnduranceChanged;


    public float GetEndurance()
    {
        return endurance;
    }

    public void IncreaseEndurance(float amount)
    {
        var newEndurance = endurance + amount;
        endurance = Mathf.Min(100, newEndurance);
        onEnduranceChanged?.Invoke();
    }
    public void ReduceEndurance(float amount)
    {
        var newEndurance = endurance - amount;
        endurance = Mathf.Max(0, newEndurance);
        onEnduranceChanged?.Invoke();
    }
}
