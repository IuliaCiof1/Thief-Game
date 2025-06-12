using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject handPalm, handFist;
    [SerializeField] BoxCollider handPalmCollider;
    [SerializeField] BoxCollider handFistCollider;
    bool isGrabbing;
    [SerializeField] float smoothing;
    [SerializeField] Canvas canvas;

    Vector3 movementDirection; // Track movement direction
    [SerializeField]Rigidbody rb;
    [SerializeField] RectTransform rectTransform;

    bool canMove = false;
    bool noItemInHand = true;

    Vector3 initialHandPos;

    [SerializeField]Transform pocketItemsContainer;

   IEnumerator Wait()
    {
        print("wait");
        rectTransform.localPosition = initialHandPos;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
    }


    private void Start()
    {
        
    }

    private void OnEnable()
    {

        initialHandPos = rectTransform.localPosition;
    
        rb.velocity = Vector3.zero;
      
        StartCoroutine(Wait());
        
        
    }

    private void OnDisable()
    {
        print("disable");
        canMove = false;
        rectTransform.localPosition = initialHandPos;

        foreach(Transform item in pocketItemsContainer)
        {
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);
        }
    }

    void Update()
    {


        if (canMove)
        {
            // Convert mouse position to world space
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
            Vector3 targetPosition = canvas.transform.TransformPoint(pos);

            // Calculate the direction and apply force
            Vector3 direction = (targetPosition - rb.position);
            float distance = direction.magnitude;
            Vector3 force = direction.normalized * smoothing * distance * 0.1f;
            //Vector3 force = direction.normalized;
           

            force = Vector3.ClampMagnitude(force, 0.2f); // Limits max force to 5

            rb.AddForce(force, ForceMode.Force);
        }

        if (Input.GetMouseButton(0))
        {
            isGrabbing = true;
            handPalmCollider.enabled = false;
            handFistCollider.enabled = true;
        }
        else
        {
            handPalmCollider.enabled = true;
            handFistCollider.enabled = false;
            isGrabbing = false;
        }

        if (isGrabbing)
        {
            handFist.SetActive(true);
            handPalm.SetActive(false);
        }
        else
        {
            handFist.SetActive(false);
            handPalm.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<PocketItem>(out PocketItem pocketItem))
        {
            if (isGrabbing && !CheckItemsInHand())
                pocketItem.GrabItem(transform);
            else
                pocketItem.UngrabItem();
        }
       
    }


    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.TryGetComponent<PocketItem>(out PocketItem pocketItem))
        {

            if (isGrabbing && !CheckItemsInHand())
            {
               

                pocketItem.GrabItem(transform);

            }
            else
                pocketItem.UngrabItem();
        }
      
    }

   bool CheckItemsInHand()
    {

        if (transform.GetChild(0).TryGetComponent<PocketItem>(out PocketItem pocketItem))
            return true;

        return false;

    }
}

