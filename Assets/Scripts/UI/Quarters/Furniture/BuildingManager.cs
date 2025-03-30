using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] FurnitureSO[] ownedObjects;
    [SerializeField] Material[] ownedWallMaterials;
    [SerializeField] Material[] ownedFloorMaterials;
    public GameObject pendingObject { get; set; }
    private Vector3 position;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] Camera topCamera;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] int rotateAmount = 45;
    public bool CanPlace { get; set; }

    [Tooltip("Materials for valid and invalid object placement.")]
    [SerializeField] private Material[] materials;
    Material initialMaterial;

    VirtualMouseInput virtualMouse;
    // Start is called before the first frame update

    ChangeInputMaps changeInputMaps;
    PlayerControls controls;

    void Start()
    {
        CanPlace = true;
        virtualMouse = FindObjectOfType<VirtualMouseInput>();

        changeInputMaps = FindObjectOfType<ChangeInputMaps>();
        controls = changeInputMaps.controls;

        controls.Furniture.Click.performed += ctx => OnLeftClick();
        controls.Furniture.RightClick.performed += ctx => OnRightClick();

        controls.BaseGameplay.Disable();
        controls.Quarters.Disable();
    }


    void OnLeftClick()
    {
        if (pendingObject != null)
        {

            PlaceObject();

        }
    }


    void OnRightClick()
    {

        RotateObject();
    }

    public void UpdateMaterials()
    {
        if (CanPlace)
            pendingObject.GetComponent<MeshRenderer>().material = materials[0];
        else if (!CanPlace)
            pendingObject.GetComponent<MeshRenderer>().material = materials[1];
    }

    public FurnitureSO[] GetOwnedObjects()
    {
        return ownedObjects;
    }

    public Material[] GetOwnedFloorMaterials()
    {
        return ownedFloorMaterials;
    }

    public Material[] GetOwnedWallMaterials()
    {
        return ownedWallMaterials;
    }

    private void Update()
    {
        if (pendingObject != null)
        {

            pendingObject.transform.position = position;
            pendingObject.GetComponent<MeshCollider>().isTrigger = true;
            UpdateMaterials();

        }

    }


    //Update is called once per frame
    void FixedUpdate()
    {
        Vector2 screenPosition;
        //if (playerInput.currentControlScheme == "Gamepad")
        //{

        //    screenPosition = playerInput.gameObject.GetComponent<GamepadCursor>().virtualMouse.position.value;

        //}
        //else { screenPosition = (Input.mousePosition); }

        screenPosition = (Input.mousePosition);

        Ray ray = topCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            position = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        if (ownedObjects[index].value <= PlayerStats.money)
        {
            pendingObject = Instantiate(ownedObjects[index].objectPrefab, position, transform.rotation, gameObject.transform);
            initialMaterial = pendingObject.GetComponent<MeshRenderer>().material;
            pendingObject.SetActive(true);

            PlayerStats.RemoveMoiney(ownedObjects[index].value);
        }

    }



    public void PlaceObject()
    {
        pendingObject.GetComponent<MeshCollider>().isTrigger = false;
        pendingObject.GetComponent<MeshRenderer>().material = initialMaterial;

        pendingObject = null;
    }

    public void RotateObject()
    {
        pendingObject.transform.Rotate(Vector3.up, rotateAmount);
    }
}
