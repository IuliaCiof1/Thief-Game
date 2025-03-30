using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PaintingRoomUI : MonoBehaviour
{
    [SerializeField] Transform wallsParent;
    [SerializeField] Transform FloorsParent;
    [SerializeField] Material wallMaterial;
    [SerializeField] Material floorMaterial;

    public bool paintWall;
    public bool paintFloor;

    private void OnValidate()
    {
        if (paintWall)
        {
            PaintWalls(wallMaterial);
            
        }
        else if(paintFloor)
            PaintWalls(floorMaterial);
    }


    [SerializeField] private GameObject furniturePanel;
    [SerializeField] private GameObject topCamera;
    [SerializeField] private GameObject furnitureSlotPrefab;
    [SerializeField] private GameObject uiButtons;

    BuildingManager buildingManager;
    public ChangeInputMaps changeInputMaps;
    PlayerControls controls;

    public PlayerInput playerInput;


    private void Start()
    {

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


        controls.BaseGameplay.PaintWalls.performed += ctx => OnFurniture();

        buildingManager = FindObjectOfType<BuildingManager>();
        UpdateFurnitureUI(buildingManager.GetOwnedWallMaterials());

    }


    private void OnFurniture()
    {
        print("paint");

        if (furniturePanel.activeSelf)
        {
            controls.Paint.Disable();
            CloseFurnitureUI();
        }
        else
        {
            controls.Paint.Enable();
            controls.Paint.Paint.performed += ctx => OnFurniture();
            OpenFurnitureUI();
        }
    }





    public void UpdateFurnitureUI(Material[] ownedFurniture)
    {

        for (int i = 0; i < ownedFurniture.Length; i++)
        {
            GameObject slot = Instantiate(furnitureSlotPrefab, furniturePanel.GetComponentInChildren<GridLayoutGroup>().transform);
            slot.transform.GetChild(0).GetComponent<Image>().material = ownedFurniture[i];
            //slot.transform.GetChild(1).GetComponent<TMP_Text>().text = ownedFurniture[i].value + "$";


            int capturedIndex = i; // Capture the current value of i
            slot.GetComponent<Button>().onClick.AddListener(() => PaintWalls(buildingManager.GetOwnedWallMaterials()[capturedIndex]));
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
        
        furniturePanel.SetActive(false);
        topCamera.SetActive(false);
        //Camera.main.GetComponent<CinemachineBrain>().enabled = true;

    }


    void PaintWalls(Material material)
    {
        List<Material> materialList = new List<Material>();
        materialList.Add(material);

        foreach (Transform wall in wallsParent)
        {
            wall.GetComponent<MeshRenderer>().SetMaterials(materialList);
        }
    }

}
