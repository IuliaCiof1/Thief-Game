using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject stealCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //steal action
        if (Input.GetKeyDown(KeyCode.E))
        {
            cameraController.RotateCamera(Quaternion.identity);
            cameraController.SetStealCamera();
            cameraController.ToggleComponent<CinemachineFollowZoom>(true);
            stealCanvas.SetActive(true);
        }
    }
}
