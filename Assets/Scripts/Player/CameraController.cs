using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    CinemachineVirtualCamera vrCamComp;
    //[SerializeField] GameObject player;
    [SerializeField] float rotationSpeed;

    Quaternion cameraRotation;
    bool isRotating;

    List<GameObject> objectsColliding;

    Quaternion targetRotation;

    bool stealMode = false;
    bool collisionMode = false;


    CinemachineFollowZoom vrCamCompZoom;
    float iniScreenX;
    float iniScreenY;
    CinemachineFramingTransposer vrCamCompFrameTransposer;
    CinemachineBasicMultiChannelPerlin vrCamCompPerlin;
    float iniAplitudeGain;

    private void Start()
    {

        

        cameraRotation = virtualCamera.transform.rotation;

        objectsColliding = new List<GameObject>();

        vrCamComp = GetComponent<CinemachineVirtualCamera>();


        vrCamCompZoom = vrCamComp.GetComponent<CinemachineFollowZoom>();
        vrCamCompFrameTransposer = vrCamComp.GetCinemachineComponent<CinemachineFramingTransposer>();
        iniScreenX = vrCamCompFrameTransposer.m_ScreenX;
        iniScreenY = vrCamCompFrameTransposer.m_ScreenY;
        vrCamCompPerlin = vrCamComp.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        iniAplitudeGain = vrCamCompPerlin.m_AmplitudeGain;
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

    public void ResetCamera()
    {
        targetRotation = cameraRotation;
        isRotating = true;

        collisionMode = false;

        vrCamCompZoom.enabled = false;
        vrCamCompFrameTransposer.m_ScreenX = iniScreenX;
        vrCamCompFrameTransposer.m_ScreenY = iniScreenY;
        vrCamCompPerlin.m_AmplitudeGain = iniAplitudeGain;
        vrCamCompFrameTransposer.m_SoftZoneHeight = 0.8f;
        vrCamCompFrameTransposer.m_SoftZoneHeight = 0.8f;
        vrCamCompZoom.m_MinFOV = 3;
    }

    public void SetCollisionCamera()
    {
        RotateCamera(Quaternion.Euler(80, 0, 0));
        collisionMode = true;
    }

    public void SetStealCamera()
    {
        stealMode = true;
        //if (!collisionMode)
            RotateCamera(Quaternion.Euler(30,0,0));
        //else
        //    vrCamComp.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.49f;
        //ToggleComponent<CinemachineFollowZoom>(true);
        vrCamCompZoom.enabled = true;
        vrCamCompFrameTransposer.m_ScreenX = 0.46f;
        vrCamCompFrameTransposer.m_ScreenY = 0.62f;
        vrCamCompPerlin.m_AmplitudeGain = 0.09f;

        //foreach (GameObject objectColliding in objectsColliding)
        //    objectColliding.SetActive(false);

        
    }

    public void SetTramCamera()
    {
        SetCollisionCamera();

        vrCamCompFrameTransposer.m_SoftZoneHeight = 0.22f;
        vrCamCompFrameTransposer.m_SoftZoneHeight = 0.23f;
        vrCamCompZoom.m_MinFOV = 29;
       vrCamCompZoom.enabled = true;
        //vrCamCompFrameTransposer.m_ScreenX = 0.46f;
        //vrCamCompFrameTransposer.m_ScreenY = 0.62f;
        //vrCamCompPerlin.m_AmplitudeGain = 0.09f;

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


    public void AddObjectCollidingWithCamera(GameObject objectColliding)
    {
        objectsColliding.Add(objectColliding);
        
    }

    public void RemoveObjectCollidingWithCamera(GameObject objectColliding)
    {
        objectColliding.SetActive(true);
        objectsColliding.Remove(objectColliding);
    }






    private void OnTriggerEnter(Collider other)
    {

      
        //cameraController.AddObjectCollidingWithCamera(other.gameObject);

        if (other.TryGetComponent<Building>(out Building building))
        {
            
            //AddObjectCollidingWithCamera(other.gameObject);


        }
        //if (stealMode)
        //    foreach (GameObject objectColl in objectsColliding)
        //        objectColl.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        //cameraController.RemoveObjectCollidingWithCamera(other.gameObject);

        if (other.TryGetComponent<Building>(out Building building))
        {
            //cameraController.RemoveObjectCollidingWithCamera(other.gameObject);


        }
    }

    

}
