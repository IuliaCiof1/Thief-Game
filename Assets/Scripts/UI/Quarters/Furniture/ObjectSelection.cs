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

    private void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
        virtualMouse = FindObjectOfType<VirtualMouseInput>();

        changeInputMaps = FindObjectOfType<ChangeInputMaps>();

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

        if (Physics.Raycast(ray, out hit, 1000000000000000))
        {
            print("place furniture");

            if (hit.collider.gameObject.TryGetComponent<Furniture>(out Furniture furniture))
            {

                Select(hit.collider.gameObject);
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
        Move();
    }


    void OnDeleteFurniture()
    {
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

        buildingManager.pendingObject = selectedObject;
    }

    public void Delete()
    {
        GameObject objToDestroy = selectedObject;
        Deselect();
        Destroy(objToDestroy);
    }

 
}
