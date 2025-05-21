using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class RewardPickup : MonoBehaviour
{
    [SerializeField] private RewardData rewardData;
    private InventoryController inventoryController;
    private XRGrabInteractable grabInteractable;
    private Animator animator;
    private Destroyable destroyable;
    private AudioSource audioSource;
    private GameObject mainCamera;

    private GameObject hand;

    private bool isPickedUp = false;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        destroyable = GetComponent<Destroyable>();
        animator.speed = 0f;
        audioSource = GetComponentInChildren<AudioSource>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        hand = GameObject.FindGameObjectWithTag("LeftHand");
    }

    private void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();   
    }

    public RewardData GetRewardData()
    {
        return rewardData;
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (isPickedUp) return;
        Pickup();
    }

    public void OnPickupAnimationComplete()
    {
        destroyable.DestroySelf();
    }

    public bool GetIsPickedUp()
    {
        return isPickedUp;
    }

    public void Pickup()
    {
        StartCoroutine(MoveToHand());
        audioSource.Play();
        animator.speed = 1f;
        inventoryController.AddMoney(rewardData.value);
        isPickedUp = true;
        Instantiate(rewardData.pickupTextEffect, hand.transform.position + hand.transform.up * 0.1f, mainCamera.transform.rotation);
    }

    private IEnumerator MoveToHand()
    {
        float speed = 3f;
        float distanceThreshold = 0.01f;

        while (Vector3.Distance(transform.position, hand.transform.position) > distanceThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, hand.transform.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}
