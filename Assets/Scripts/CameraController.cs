using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    //[SerializeField] GameObject player;
    [SerializeField] float rotationSpeed;

    Quaternion cameraRotation;
    bool isRotating;

    List<GameObject> objectsColliding;

    Quaternion targetRotation;

    private void Start()
    {
        cameraRotation = virtualCamera.transform.rotation;

        objectsColliding = new List<GameObject>();
    }

    private void Update()
    {
     
        if (isRotating)
            virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, targetRotation, rotationSpeed);
      

        if (virtualCamera.transform.rotation == targetRotation)
           isRotating = false;

    }


    public void RotateCamera(Quaternion targetRotation)
    {
        isRotating = true;
        this.targetRotation = targetRotation;
    }

    public void ResetCameraRotation()
    {
        targetRotation = cameraRotation;
        isRotating = true;
    }


    //public void AddObjectCollidingWithCamera(GameObject objectColliding)
    //{
    //    objectsColliding.Add(objectColliding);
    //}

    //public void RemoveObjectCollidingWithCamera(GameObject objectColliding)
    //{
    //    objectColliding.SetActive(true);
    //    objectsColliding.Remove(objectColliding);
    //}

    public void SetStealCamera()
    {
        GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.45f;
        GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.09f;

        //foreach (GameObject objectColliding in objectsColliding)
        //    objectColliding.SetActive(false);
    }

    public void ToggleComponent<T>(bool enable) where T : Behaviour
    {
        T component = GetComponent<T>();
        if (component != null)
        {
            component.enabled = enable;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{

    //    if(other.TryGetComponent<Building>(out Building building))
    //    {
    //        isRotating = true;
    //        targetRotation = Quaternion.Euler(80, 0, 0);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{


    //    if (other.TryGetComponent<Building>(out Building building))
    //    {
    //        targetRotation = cameraRotation;
    //        isRotating = true;
    //    }
    //}
}
