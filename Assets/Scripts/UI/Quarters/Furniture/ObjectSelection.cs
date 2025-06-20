using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class ObjectSelection : MonoBehaviour
{
    private GameObject selectedObject;
    [SerializeField] Camera topCamera;
    BuildingManager buildingManager;
    [SerializeField] GameObject moveDelete;
    VirtualMouseInput virtualMouse;
    [SerializeField] private PlayerInput playerInput;

    ChangeInputMaps changeInputMaps;
    PlayerControls controls;
    Furniture selectedFurniture;
    Furniture furniture;

    FurnitureUI furnitureUI;

    private void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
        virtualMouse = FindObjectOfType<VirtualMouseInput>();

        changeInputMaps = FindObjectOfType<ChangeInputMaps>();
        furnitureUI = FindFirstObjectByType<FurnitureUI>();

        controls = changeInputMaps.controls;
        controls.Furniture.Click.performed += ctx => OnLeftClick();
        controls.Furniture.MoveFurniture.performed += ctx => OnMoveFurniture();
        controls.Furniture.DeleteFurniture.performed += ctx => OnDeleteFurniture();
    }


    void OnLeftClick()
    {
        Vector2 screenPosition;
      
        screenPosition = (Input.mousePosition);
        Ray ray = topCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50000))
        {
            //Player clicked on a furniture item
            if (hit.collider.gameObject.TryGetComponent<Furniture>(out furniture))
            {
                if (!furniture.isPending && buildingManager.pendingObject == null)
                {
                    Select(hit.collider.gameObject);
                }
            }
            //Player clicked on a non furniture object
            else if (selectedObject != null)
            {
                Deselect();
            }
        }
    }


    void OnMoveFurniture()
    {
        if (selectedObject != null)
            Move();
    }


    void OnDeleteFurniture()
    {
        if (selectedObject != null && !furniture.isPending)
            Delete();
    }



    private void Select(GameObject furniture)
    {
        moveDelete.SetActive(true);

        if (furniture == selectedObject) return;
        if (selectedObject != null) Deselect();

        //Outline outline 
        //    = furniture.GetComponent<Outline>();

        if (!furniture.TryGetComponent<Outline>(out Outline outline)) furniture.AddComponent<Outline>();
        else outline.enabled = true;

        selectedObject = furniture;
    }

    private void Deselect()
    {
        moveDelete.SetActive(false);

        selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
    }


    public void Move()
    {
        moveDelete.SetActive(false);
        selectedFurniture = selectedObject.GetComponent<Furniture>();
        selectedFurniture.isPending = true;  

        buildingManager.initialMaterial = selectedObject.GetComponent<MeshRenderer>().material;
        buildingManager.pendingObject = selectedObject;
        
    }

    public void Delete()
    {
        Furniture selectedFurniture = selectedObject.GetComponent<Furniture>();

        PlayerStats.Instance.AddMoiney(selectedFurniture.furnitureSO.value);
        PlayerStats.Instance.RemoveReputation(selectedFurniture.furnitureSO.reputation);

        GameObject objToDestroy = selectedObject;
        Deselect();
        Destroy(objToDestroy);

       furnitureUI.RefreshInteraction();
    }

 
}
