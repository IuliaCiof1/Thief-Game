using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FurnitureUI : MonoBehaviour
{
    

    [SerializeField] private GameObject furniturePanel;
    [SerializeField] private GameObject topCamera;
    [SerializeField] private GameObject furnitureSlotPrefab;
    [SerializeField] private GameObject uiButtons;

    BuildingManager buildingManager;
    public ChangeInputMaps changeInputMaps;
    PlayerControls controls;

    public PlayerInput playerInput;

    //void Start()
    //{

    //    //buildingManager = FindObjectOfType<BuildingManager>();
    //    //UpdateFurnitureUI(buildingManager.GetOwnedObjects());

    //}




    private void Start()
    {

        print("Furniture UI Awake");

        changeInputMaps = FindObjectOfType<ChangeInputMaps>();

        if (changeInputMaps == null)
        {
            Debug.LogError("ChangeInputMaps not found!");
            return;
        }

        print("Found ChangeInputMaps: " + changeInputMaps.gameObject.name);

        controls = changeInputMaps.controls;

        if (controls == null)
        {
            Debug.LogError("PlayerControls is null! Ensure ChangeInputMaps initializes it.");
            return;
        }

        //print("Current Action Map: " + playerInput.currentActionMap.name);

        // Ensure event is only added once

        //controls.BaseGameplay.Enable();
        //playerInput.SwitchCurrentActionMap("BaseGameplay");
        //controls.BaseGameplay.Enable();
        //print("furniture start");

        ////changeInputMaps = FindObjectOfType<ChangeInputMaps>();
        //print(changeInputMaps.gameObject.name);
        //// controls = changeInputMaps.controls;
        //print(playerInput.currentActionMap.name);
        controls.BaseGameplay.Furniture.performed += ctx => OnFurniture();

        buildingManager = FindObjectOfType<BuildingManager>();
        UpdateFurnitureUI(buildingManager.GetOwnedObjects());

        //OpenFurnitureUI();
        //controls.Furniture.Disable();
        //CloseFurnitureUI();

        //StartCoroutine(FixInputSystem());

        //playerInput.SwitchCurrentActionMap("BaseGameplay");
        //Debug.Log("Forced action map to: " + playerInput.currentActionMap.name);

        //buildingManager = FindObjectOfType<BuildingManager>();
        ////changeInputMaps = FindObjectOfType<ChangeInputMaps>();

        //if (changeInputMaps == null)
        //{
        //    Debug.LogError("ChangeInputMaps not found!");
        //    return;
        //}

        //controls = changeInputMaps.controls;

        //if (controls == null)
        //{
        //    Debug.LogError("PlayerControls is null!");
        //    return;
        //}

        //// Force the correct action map
        //playerInput.SwitchCurrentActionMap("BaseGameplay");
        //controls.BaseGameplay.Enable();
        //if (playerInput == null)
        //{
        //    Debug.LogError("playerInput is not assigned before InvokeRepeating!");
        //    return;
        //}
        //Debug.Log("FurnitureUI initialized with action map: " + playerInput.currentActionMap.name);

        //Invoke("FixInputSystem", 0.01f);
        //    StartCoroutine(FixInputSystem());
        //}

    }
    //IEnumerator FixInputSystem()
    //{
    //    yield return new WaitForEndOfFrame(); // Wait until the end of the first frame
    //    Debug.Log("FurnitureUI initialized with action map: " + playerInput.currentActionMap.name);
    //    playerInput.SwitchCurrentActionMap("BaseGameplay");
    //    controls.BaseGameplay.Enable();
    //}

    private void OnFurniture()
    {
        print("furniture");
        if (furniturePanel.activeSelf)
        {
            controls.Furniture.Disable();
            CloseFurnitureUI();
        }
        else
        {
            controls.Furniture.Enable();
            controls.Furniture.Furniture.performed += ctx => OnFurniture();
            OpenFurnitureUI();
        }
    }





    public void UpdateFurnitureUI(FurnitureSO[] ownedFurniture)
    {

        for (int i = 0; i < ownedFurniture.Length; i++)
        {
            GameObject slot = Instantiate(furnitureSlotPrefab, furniturePanel.GetComponentInChildren<GridLayoutGroup>().transform);
            slot.transform.GetChild(0).GetComponent<Image>().sprite = ownedFurniture[i].previewImage;


            int capturedIndex = i; // Capture the current value of i
            slot.GetComponent<Button>().onClick.AddListener(() => buildingManager.SelectObject(capturedIndex));
        }

    }


    public void OpenFurnitureUI()
    {

        changeInputMaps.ChangeToFurnitureMap();
        uiButtons.SetActive(false);

        furniturePanel.SetActive(true);

        //stop camera rotation
        //Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        topCamera.SetActive(true);
    }



    public void CloseFurnitureUI()
    {

        changeInputMaps.ChangeToGameplayMap();

        uiButtons.SetActive(true);
        DeselectObjects();

        furniturePanel.SetActive(false);
        topCamera.SetActive(false);
        //Camera.main.GetComponent<CinemachineBrain>().enabled = true;

    }


    public void DeselectObjects()
    {
        //foreach (Transform child in buildingManager.transform)
        //{
        //    child.GetComponent<Outline>().enabled = false;
        //}
    }
}
