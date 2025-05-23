using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class FurnitureUI : MonoBehaviour
{
    

    [SerializeField] private GameObject furniturePanel;
    [SerializeField] private GameObject topCamera;
    [SerializeField] private GameObject furnitureSlotPrefab;
    [SerializeField] private GameObject uiButtons;
    [SerializeField] private GameObject objectivesUI;

    BuildingManager buildingManager;
    public ChangeInputMaps changeInputMaps;
    PlayerControls controls;

    public PlayerInput playerInput;

    CursorController cursorController;

    //void Start()
    //{

    //    //buildingManager = FindObjectOfType<BuildingManager>();
    //    //UpdateFurnitureUI(buildingManager.GetOwnedObjects());

    //}




    private void Start()
    {

        cursorController = FindFirstObjectByType<CursorController>();

        changeInputMaps = FindObjectOfType<ChangeInputMaps>();

        if (changeInputMaps == null)
        {
            Debug.LogError("ChangeInputMaps not found!");
            return;
        }

      

        controls = changeInputMaps.controls;

        if (controls == null)
        {
            Debug.LogError("PlayerControls is null! Ensure ChangeInputMaps initializes it.");
            return;
        }

        
        controls.BaseGameplay.Furniture.performed += ctx => OnFurniture();

        buildingManager = FindObjectOfType<BuildingManager>();
        UpdateFurnitureUI(buildingManager.GetOwnedObjects());

    }


    private void OnFurniture()
    {
        
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
            slot.transform.GetChild(1).GetComponent<TMP_Text>().text = "- "+ownedFurniture[i].value+"$";
            slot.transform.GetChild(2).GetComponent<TMP_Text>().text = "+ "+ownedFurniture[i].reputation;

            print("add listener");
            int capturedIndex = i; // Capture the current value of i
            slot.GetComponent<Button>().onClick.AddListener(() => buildingManager.SelectObject(capturedIndex));
        }

    }


    public void OpenFurnitureUI()
    {
        cursorController.CursorVisibility(true);

        changeInputMaps.ChangeToFurnitureMap();
        uiButtons.SetActive(false);
        objectivesUI.SetActive(false);

        furniturePanel.SetActive(true);

        //stop camera rotation
        //Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        topCamera.SetActive(true);
    }



    public void CloseFurnitureUI()
    {
        cursorController.CursorVisibility(false);
        //changeInputMaps.ChangeToGameplayMap();

        uiButtons.SetActive(true);
        objectivesUI.SetActive(true);
        DeselectObjects();

        furniturePanel.SetActive(false);
        topCamera.SetActive(false);

        changeInputMaps.ChangeToGameplayMap();
        //Camera.main.GetComponent<CinemachineBrain>().enabled = true;

    }


    public void DeselectObjects()
    {
        foreach (Transform child in buildingManager.transform)
        {
            child.GetComponent<Outline>().enabled = false;
        }
    }
}
