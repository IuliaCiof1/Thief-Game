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
    //CinemachineCollider vrCamCompCollider;
    float iniAplitudeGain;
    Vector3 iniTrackedOffset;
    [SerializeField] GameObject player;

    [SerializeField]Vector3 stealOffset;


    

    enum CameraMode { Initial, Collision, Steal, Tram }

    struct CameraSettings
    {
        public float screenX;
        public float screenY;
        public float amplitudeGain;
        public float softZoneHeight;
        public float softZoneWidth;
        public float biasX;
        public float minFOV;
        public Vector3 trackedOffset;
        public Quaternion cameraRotation;
        public CameraMode mode;  // add mode info here
    }

    CameraMode currentMode = CameraMode.Initial;

    CameraSettings previousSettings;
    bool hasSavedPreviousSettings = false;
    private Stack<CameraSettings> cameraSettingsStack = new Stack<CameraSettings>();

    void SaveCurrentCameraSettings(CameraMode mode)
    {

        CameraSettings currentSettings = new CameraSettings
        {
            screenX = vrCamCompFrameTransposer.m_ScreenX,
            screenY = vrCamCompFrameTransposer.m_ScreenY,
            amplitudeGain = vrCamCompPerlin.m_AmplitudeGain,
            softZoneHeight = vrCamCompFrameTransposer.m_SoftZoneHeight,
            softZoneWidth = vrCamCompFrameTransposer.m_SoftZoneWidth,
            biasX = vrCamCompFrameTransposer.m_BiasX,
            minFOV = vrCamCompZoom.m_MinFOV,
            trackedOffset = vrCamCompFrameTransposer.m_TrackedObjectOffset,
            cameraRotation = virtualCamera.transform.rotation,
            mode = mode
        };
        print("camera controller mode SAVE" + mode.ToString());
        cameraSettingsStack.Push(currentSettings);
    }

    void SaveCurrentCameraSettingsIfModeChanged(CameraMode newMode)
    {
        if (currentMode != newMode)
        {
            SaveCurrentCameraSettings(currentMode);
            currentMode = newMode;
           
        }
    }

    void RestorePreviousCameraSettings()
    {
        if (cameraSettingsStack.Count == 0) { return; }

        CameraSettings settings = cameraSettingsStack.Pop();

        targetRotation = settings.cameraRotation;
        isRotating = true;

        vrCamCompFrameTransposer.m_ScreenX = settings.screenX;
        vrCamCompFrameTransposer.m_ScreenY = settings.screenY;
        vrCamCompPerlin.m_AmplitudeGain = settings.amplitudeGain;
        vrCamCompFrameTransposer.m_SoftZoneHeight = settings.softZoneHeight;
        vrCamCompFrameTransposer.m_SoftZoneWidth = settings.softZoneWidth;
        vrCamCompFrameTransposer.m_BiasX = settings.biasX;
        vrCamCompZoom.m_MinFOV = settings.minFOV;
        vrCamCompFrameTransposer.m_TrackedObjectOffset = settings.trackedOffset;

        currentMode = settings.mode;
        print("ResetCamera:: camera controller restore to current settings "+currentMode.ToString());
        
    }

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
        //vrCamCompCollider = vrCamComp.GetComponent<CinemachineCollider>();
        iniAplitudeGain = vrCamCompPerlin.m_AmplitudeGain;
        iniTrackedOffset = vrCamCompFrameTransposer.m_TrackedObjectOffset;

       
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
        ////targetRotation = cameraRotation;
        ////isRotating = true;

        ////collisionMode = false;
        //////vrCamCompCollider.enabled = false;
        ////vrCamCompZoom.enabled = false;
        ////vrCamCompFrameTransposer.m_ScreenX = iniScreenX;
        ////vrCamCompFrameTransposer.m_ScreenY = iniScreenY;
        ////vrCamCompPerlin.m_AmplitudeGain = iniAplitudeGain;
        ////vrCamCompFrameTransposer.m_SoftZoneHeight = 0.8f;
        ////vrCamCompFrameTransposer.m_SoftZoneHeight = 0.8f;
        ////vrCamCompFrameTransposer.m_BiasX = 0;
        ////vrCamCompZoom.m_MinFOV = 3;
        ////vrCamCompFrameTransposer.m_TrackedObjectOffset = iniTrackedOffset;
        //collisionMode = false;
        //vrCamCompZoom.enabled = false;
        RestorePreviousCameraSettings();
        vrCamCompZoom.enabled = false;
        if (currentMode == CameraMode.Initial)
        {
            StopAllCoroutines();
            print("camera controller reset to initial");
            targetRotation = cameraRotation;
            isRotating = true;

            collisionMode = false;
            //vrCamCompCollider.enabled = false;
            vrCamCompZoom.enabled = false;
            vrCamCompFrameTransposer.m_ScreenX = iniScreenX;
            vrCamCompFrameTransposer.m_ScreenY = iniScreenY;
            vrCamCompPerlin.m_AmplitudeGain = iniAplitudeGain;
            vrCamCompFrameTransposer.m_SoftZoneHeight = 0.8f;
            vrCamCompFrameTransposer.m_SoftZoneHeight = 0.8f;
            vrCamCompFrameTransposer.m_BiasX = 0;
            vrCamCompZoom.m_MinFOV = 3;
            vrCamCompFrameTransposer.m_TrackedObjectOffset = iniTrackedOffset;
        }
    }

    public void SetCollisionCamera()
    {
        StopAllCoroutines();
        SaveCurrentCameraSettingsIfModeChanged(CameraMode.Collision);
        print("SetCollisionCamera:: camera controller save current settings");
        RotateCamera(Quaternion.Euler(85, 0, 0));
        collisionMode = true;
        
    }

    public void SetStealCamera()
    {
        StopAllCoroutines();
        SaveCurrentCameraSettingsIfModeChanged(CameraMode.Steal);

        stealMode = true;
        //vrCamCompCollider.enabled = true;
        //if (!collisionMode)
        //vrCamCompFrameTransposer.m_TrackedObjectOffset -= new Vector3(8,0 ,0 );
        vrCamCompFrameTransposer.m_TrackedObjectOffset = stealOffset;
        RotateCamera(Quaternion.Euler(30,0,0));
        //else
        //    vrCamComp.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = 0.49f;
        //ToggleComponent<CinemachineFollowZoom>(true);
        vrCamCompZoom.enabled = true;
        vrCamCompFrameTransposer.m_ScreenX = 0.32f;
        vrCamCompFrameTransposer.m_ScreenY = 0.62f;
        vrCamCompPerlin.m_AmplitudeGain = 0.09f;
        vrCamCompFrameTransposer.m_SoftZoneWidth = 0.20f;
        vrCamCompFrameTransposer.m_BiasX = -0.5f;
        //transform.LookAt(player.transform);
        //vrCamCompFrameTransposer.m_ScreenX = Mathf.Lerp(vrCamCompFrameTransposer.m_ScreenX, 0.5f, Time.deltaTime * 5f);
        StartCoroutine(SmoothRecenter(0.5f));
        //foreach (GameObject objectColliding in objectsColliding)
        //    objectColliding.SetActive(false);


    }

    IEnumerator SmoothRecenter(float duration)
    {
        float t = 0;
        float startX = vrCamCompFrameTransposer.m_ScreenX;
        float startY = vrCamCompFrameTransposer.m_ScreenY;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerpT = t / duration;
            vrCamCompFrameTransposer.m_ScreenX = Mathf.Lerp(startX, 0.34f, lerpT);
            vrCamCompFrameTransposer.m_ScreenY = Mathf.Lerp(startY, 0.5f, lerpT);
            yield return null;
        }

        vrCamCompFrameTransposer.m_ScreenX = 0.34f;
        vrCamCompFrameTransposer.m_ScreenY = 0.5f;
    }

    public void SetTramCamera()
    {
        SetCollisionCamera();
        vrCamCompFrameTransposer.m_TrackedObjectOffset = iniTrackedOffset;
        vrCamCompFrameTransposer.m_ScreenX = iniScreenX;
        vrCamCompFrameTransposer.m_ScreenY = 0.5f;
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
