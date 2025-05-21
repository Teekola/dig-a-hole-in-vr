using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VoxelInteractor : XRBaseInteractable
{
    private Destroyable destroyable;

    void Start()
    {
        destroyable = GetComponent<Destroyable>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (destroyable != null)
        {
            destroyable.DestroySelf();
        }
        else
        {
            Debug.LogWarning("Destroyable component missing on voxel.");
        }
    }
}
