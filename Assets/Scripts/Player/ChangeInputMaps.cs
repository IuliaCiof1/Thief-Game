using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ChangeInputMaps : MonoBehaviour
{
    PlayerInput canvasPlayerInput;
    [SerializeField]PlayerInput playerPlayerInput;
    public PlayerControls controls { get; set; }

    private void Awake()
    {


        //PlayerInput input = new PlayerInput();
      
        print("controls");
        controls = new PlayerControls();
        //controls.BaseGameplay.Enable();
      
        //playerPlayerInput.SwitchCurrentActionMap("BaseGameplay");
        //controls.BaseGameplay.Enable();

    }

    private void OnDisable()
    {
        controls.BaseGameplay.Disable();
        controls.Furniture.Disable();
    }


    //void Awake()
    //{


    //    //playerPlayerInput = FindObjectOfType<GamepadCursor>().gameObject.GetComponent<PlayerInput>();


    //}


    private void Start()
    {

        //playerPlayerInput.SwitchCurrentActionMap("BaseGameplay");
        //controls.BaseGameplay.Enable();


        StartCoroutine(FixInputSystem());
    }


    IEnumerator FixInputSystem()
    {
        yield return new WaitForEndOfFrame(); // Wait until the end of the first frame
      
        playerPlayerInput.SwitchCurrentActionMap("BaseGameplay");
        controls.BaseGameplay.Enable();
    }

public void ChangeToFurnitureMap()
    {
        controls.BaseGameplay.Disable();
        playerPlayerInput.SwitchCurrentActionMap("Furniture");
       
       // playerPlayerInput.gameObject.GetComponent<GamepadCursor>().isUIState = true;
        //Time.timeScale = 0;


    }

    //public void ChangeToBagMap()
    //{
       
    //    playerPlayerInput.SwitchCurrentActionMap("Bag");
    //    controls.BaseGameplay.Disable();
    //    //playerPlayerInput.gameObject.GetComponent<GamepadCursor>().isUIState = true;
    //    Time.timeScale = 0;


    //}

    //public void ChangeToOutfit()
    //{
    //    controls.BaseGameplay.Disable();
    //    playerPlayerInput.SwitchCurrentActionMap("Outfit");
        
    //    //playerPlayerInput.gameObject.GetComponent<GamepadCursor>().isUIState = true;
        
    //}

    public void ChangeToGameplayMap()
    {
       
        controls.BaseGameplay.Enable();
        playerPlayerInput.SwitchCurrentActionMap("BaseGameplay");

        //playerPlayerInput.gameObject.GetComponent<GamepadCursor>().isUIState = false;

        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
        //Camera.main.transform.parent.GetComponent<ThirdPersonController>().enabled = true;

        //Camera.main.GetComponent<ThirdPersonCam>().enabled = true;
        Time.timeScale = 1;

    }

    //public void ChangeToWheel()
    //{
    //    //playerPlayerInput.gameObject.GetComponent<GamepadCursor>().isUIState = true;
    //    Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
    //    //Camera.main.GetComponent<ThirdPersonCam>().enabled = false;
    //    Camera.main.transform.parent.GetComponent<ThirdPersonController>().enabled = false;
    //    Time.timeScale = 0;
    //}

    //public void ChangeToRecord()
    //{
    //    controls.BaseGameplay.Disable();
    //    playerPlayerInput.SwitchCurrentActionMap("RecorderMode");
    //    //Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
    //    //Camera.main.GetComponent<ThirdPersonCam>().enabled = false;
    //    //Camera.main.transform.parent.GetComponent<ThirdPersonController>().enabled = false;
    //}
}
