using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{

    [SerializeField] CameraController cameraController;
    [SerializeField] Transform player;

    bool buildingFound =false;

    private void Update()
    {
        //transform.position = player.transform.position;
    }

    private void OnTriggerStay(Collider other)
    {

        print(other.gameObject.name + " stay");
        //cameraController.AddObjectCollidingWithCamera(other.gameObject);

        if (!buildingFound && other.TryGetComponent<Building>(out Building building))
        {
            cameraController.AddObjectCollidingWithCamera(other.gameObject);
            print(other.gameObject.name + " building stay");
            buildingFound = true;
            //cameraController.RotateCamera(Quaternion.Euler(80, 0, 0));
            cameraController.SetCollisionCamera();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        print(other.gameObject.name);
        //cameraController.AddObjectCollidingWithCamera(other.gameObject);

        if (!buildingFound && other.TryGetComponent<Building>(out Building building))
        {
            print(other.gameObject.name + " building enter");
           // cameraController.AddObjectCollidingWithCamera(other.gameObject);

          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //cameraController.RemoveObjectCollidingWithCamera(other.gameObject);

        if (other.TryGetComponent<Building>(out Building building))
        {
            //cameraController.RemoveObjectCollidingWithCamera(other.gameObject);

            cameraController.ResetCameraRotation();
            buildingFound = false;
        }
    }
}
