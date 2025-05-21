using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DigController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] InputActionProperty digAction;

    [Header("References")]
    [SerializeField] DigVoxels shovelController;

    private Animator shovelAnimator;
    private bool hasTriggeredDigThisCycle = false;
    private bool hasCheckedRewardsThisCycle= false;
    private EnduranceController enduranceController;
    private SceneController sceneController;
    private InventoryController inventoryController;

    private void Start()
    {
        shovelAnimator = GetComponent<Animator>();
        inventoryController = FindObjectOfType<InventoryController>();
        enduranceController = FindObjectOfType<EnduranceController>();
        sceneController = FindObjectOfType<SceneController>();
    }

    private void Update()
    {
        HandleGrabAction();
    }

    private void HandleGrabAction()
    {
        // If grab action is pressed, trigger voxel interaction (destruction)
        if (digAction.action.IsPressed())
        {
            if (!shovelAnimator.GetBool("isDigging"))
            {
                shovelAnimator.SetBool("isDigging", true);
                shovelAnimator.speed = inventoryController.GetValue(UpgradeType.DiggingSpeed);
            }

        }
        else
        {
            // Reset animation if grab is released
            shovelAnimator.SetBool("isDigging", false);
        }
    }
    public void TriggerDigImpact()
    {
        if (!hasTriggeredDigThisCycle)
        {
            shovelController.DigTouchedVoxels();
            hasTriggeredDigThisCycle = true;
            enduranceController.ReduceEndurance(inventoryController.GetValue(UpgradeType.Endurance));
           

            if (enduranceController.GetEndurance() <= 0)
            {
                sceneController.GameOver();
            }
        }
    }

    public void TriggerCheckRewards()
    {
        if (!hasCheckedRewardsThisCycle)
        {
            shovelController.CheckRewardHits();
            hasCheckedRewardsThisCycle = true;
        }
    }

    public void ResetDigTrigger()
    {
        hasTriggeredDigThisCycle = false;
        hasCheckedRewardsThisCycle = false;
    }
}
