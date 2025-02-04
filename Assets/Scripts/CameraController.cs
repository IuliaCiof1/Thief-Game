using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    [SerializeField] GameObject player;
    [SerializeField] float rotationSpeed;

    Quaternion cameraRotation;
    bool isRotating;
    

    Quaternion targetRotation;

    private void Start()
    {
        cameraRotation = virtualCamera.transform.rotation;
    }

    private void Update()
    {
        transform.position = player.transform.position;
     
        if (isRotating)
            virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, targetRotation, rotationSpeed);
      

        if (virtualCamera.transform.rotation == targetRotation)
           isRotating = false;

    }

    


    private void OnTriggerStay(Collider other)
    {
       
        if(other.TryGetComponent<Building>(out Building building))
        {
            isRotating = true;
            targetRotation = Quaternion.Euler(80, 0, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        
        if (other.TryGetComponent<Building>(out Building building))
        {
            targetRotation = cameraRotation;
            isRotating = true;
        }
    }
}
