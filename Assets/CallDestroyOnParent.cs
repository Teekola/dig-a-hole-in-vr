using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallDestroyOnParent : MonoBehaviour
{
    private Destroyable destroyable;
    void Start()
    {
        destroyable = GetComponentInParent<Destroyable>();
    }

    public void DestroyParent()
    {
        destroyable.DestroySelf();
    }
}
