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
        print("left click");
        Vector2 screenPosition;
        //if (playerInput.currentControlScheme == "Gamepad")
        //{


        //    //screenPosition = playerInput.gameObject.GetComponent<GamepadCursor>().virtualMouse.position.value;

        //}
        //else { screenPosition = (Input.mousePosition); }

        screenPosition = (Input.mousePosition);
        Ray ray = topCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50000)) //include colliders set as trigger true
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green, 2f);
            print("place furniture" + hit.collider.gameObject.name);

            if (hit.collider.gameObject.TryGetComponent<Furniture>(out furniture))
            {
                print("furniture found");
                if (!furniture.isPending && buildingManager.pendingObject == null)
                {
                    print("furniture not pending");
                    Select(hit.collider.gameObject);
                }
            }
            //if you want to move/delete object by clicking on a button, delete this if branch
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
        //PlayerStats.Instance.AddMoiney(selectedFurniture.furnitureSO.value);
        //PlayerStats.Instance.RemoveReputation(selectedFurniture.furnitureSO.reputation);

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
