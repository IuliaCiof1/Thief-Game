using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject handPalm, handFist;
    [SerializeField] bool isGrabbing;
    [SerializeField] float smoothing;
    [SerializeField] Canvas canvas;

    Vector3 movementDirection; // Track movement direction
    [SerializeField]Rigidbody rb;


    void Update()
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
        
        force = Vector3.ClampMagnitude(force, 0.5f); // Limits max force to 5

        rb.AddForce(force, ForceMode.Force);

        if (Input.GetMouseButton(0))
        {
            isGrabbing = true;
        }
        else
            isGrabbing = false;

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
        {;
            if (isGrabbing)
                pocketItem.GrabItem(transform);
            else
                pocketItem.UngrabItem();
        }
       
    }


    private void OnTriggerStay(Collider other)
    {
       
        if (other.gameObject.TryGetComponent<PocketItem>(out PocketItem pocketItem))
        {
           
            if (isGrabbing)
                pocketItem.GrabItem(transform);
            else
                pocketItem.UngrabItem();
        }
      
    }

   
}

