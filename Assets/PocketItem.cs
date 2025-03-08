using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PocketItem : MonoBehaviour
{
    Transform hand;
    Rigidbody rb;
    Transform initialParent;
    [SerializeField] float addedForce = 500;
    bool isFalling = false;
    Vector3 velocity = Vector3.zero;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        hand = FindObjectOfType<Hand>().transform;
        rb = GetComponent<Rigidbody>();
        initialParent = transform.parent;

        //Physics.IgnoreCollision(FindObjectOfType<Hand>().GetComponent<Collider>(), GetComponent<Collider>());

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


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CoinCollectArea>(out CoinCollectArea coinCollect))
        {
            animator.SetTrigger("PutInBag");
            PlayerStats.Instance.AddMoiney(5);
            Destroy(gameObject);
        }
    }

}

