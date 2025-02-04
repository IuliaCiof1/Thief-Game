using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    [SerializeField] GameObject player;
    Quaternion cameraRotation;
    bool isRotating;
    bool exitTrigger;
    [SerializeField] float rotationSpeed;
    [SerializeField] float zDistance;

    Quaternion targetRotation;

    private void Start()
    {
        cameraRotation = virtualCamera.transform.rotation;
    }

    private void Update()
    {
        transform.position = player.transform.position;
       // transform.position = new Vector3(virtualCamera.transform.position.x, virtualCamera.transform.position.y, virtualCamera.transform.position.z);
        if (isRotating)
        {
            print(targetRotation);
            virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, targetRotation, rotationSpeed);

            //Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + zDistance);

            //// Smooth interpolation for a more natural movement
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationSpeed);

        }
        else
        {
            //if (Vector3.Distance(virtualCamera.transform.position, transform.position) > zDistance)
            //    exitTrigger = true;
            //if (exitTrigger)
            //    transform.position = new Vector3(virtualCamera.transform.position.x, virtualCamera.transform.position.y, virtualCamera.transform.position.z);
        }

        if (virtualCamera.transform.rotation == targetRotation)
        {
           // transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, transform.position.z);
           isRotating = false;

        }

    }

    


    private void OnTriggerStay(Collider other)
    {
        //print(other.name);

        if(other.TryGetComponent<Building>(out Building building))
        {
            //  transform.position = Camera.main.transform.position;
            print("stay building");
           

            exitTrigger = false;
            isRotating = true;
            targetRotation = Quaternion.Euler(80, 0, 0);


            print("true");
            //building.GetComponent<MeshRenderer>().materials[3].color -= new Color(0,0,0,0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        
        if (other.TryGetComponent<Building>(out Building building))
        {
            print("exit building");
            //Quaternion cameraRotation = transform.rotation;
            //virtualCamera.transform.rotation = cameraRotation;
            targetRotation = cameraRotation;
            isRotating = true;
            exitTrigger = true;
            // print("true");
            // building.GetComponent<MeshRenderer>().materials[3].color -= new Color(0, 0, 0, 0.5f);
        }
    }
}
