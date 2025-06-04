using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PocketItem : MonoBehaviour
{
    Transform hand;
    Rigidbody rb;
    Transform initialParent;
    //[SerializeField] float addedForce = 500;
    //bool isFalling = false;
    Vector3 velocity = Vector3.zero;
    //[SerializeField] float gravity = -9.8f;
    [SerializeField] Animator animator;

    PlayerStats playerStats;

    public int Value { get; set; }

    public PocketItemSO pocketItemSO { get; set; }
    public NPC npc;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        hand = FindObjectOfType<Hand>().transform;
        rb = GetComponent<Rigidbody>();
        initialParent = transform.parent;
        playerStats = FindObjectOfType<PlayerStats>();
        //Physics.IgnoreCollision(FindObjectOfType<Hand>().GetComponent<Collider>(), GetComponent<Collider>());

        inventory = FindAnyObjectByType<Inventory>();

        PlayerActions.OnStealUIDisable += HandleStealUIDisable;


    }


    private void OnDisable()
    {
        PlayerActions.OnStealUIDisable -= HandleStealUIDisable;
    }

    void HandleStealUIDisable()
    {
        print(gameObject.name + " handle steal ui disable");

        transform.SetParent(initialParent);
        transform.localPosition = Vector3.zero;
        rb.isKinematic = false;
        gameObject.SetActive(false);
    }

    public void GrabItem(Transform parent)
    {
        transform.SetParent(parent);
       rb.isKinematic = true;
        transform.SetAsFirstSibling();
    }

    public void UngrabItem()
    {
        rb.isKinematic = false;
        transform.SetParent(initialParent);
    }

    //private void AddToInventory()
    //{
    //    Instantiate(pocketItemSO.ownedObjectPrefab, inventory.transform);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CoinCollectArea>(out CoinCollectArea coinCollect))
        {
            animator.SetTrigger("PutInBag");
            playerStats.AddMoiney(Value);

            if (pocketItemSO.canBeOwned && !inventory.CheckIfItemExists(pocketItemSO.ownedObjectPrefab.name))
            {
                inventory.AddToInventory(pocketItemSO.ownedObjectPrefab);
            }
            //Destroy(gameObject);
            //transform.SetParent(initialParent);
            //transform.localPosition = Vector3.zero;
            rb.isKinematic = false;
            gameObject.SetActive(false);

            for (int i = npc.ItemsInPocket.Count - 1; i >= 0; i--)
            {
                if (npc.ItemsInPocket[i] == pocketItemSO)
                {
                    npc.ItemsInPocket.RemoveAt(i);
                }
            }

            TutorialMain.OnItemCollected?.Invoke();
        }
    }

    //private void OnDisable()
    //{
    //    print("item disabled");
    //    //hand.gameObject.SetActive(true);
    //    transform.SetParent(initialParent);
    //    transform.localPosition = Vector3.zero;
    //    //hand.gameObject.SetActive(false);
    //}

}

