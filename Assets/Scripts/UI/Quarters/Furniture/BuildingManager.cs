using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using System;


public class BuildingManager : MonoBehaviour
{
    [SerializeField] FurnitureSO[] ownedObjects;
    [SerializeField] PaintSO[] ownedWallMaterials;
    [SerializeField] Material[] ownedFloorMaterials;
    [SerializeField] Transform wallsParent;
    public string currentWallPaintID;
    public GameObject pendingObject { get; set; }
    private Vector3 position;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] Camera topCamera;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] float rotateAmount = 45f;
    public bool CanPlace { get; set; }

    [Tooltip("Materials for valid and invalid object placement.")]
    [SerializeField] private Material[] materials;
    public Material initialMaterial;

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
        if (CanPlace && pendingObject != null)
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

    public PaintSO[] GetOwnedWallMaterials()
    {
        return ownedWallMaterials;
    }

    private void Update()
    {
        
        if (pendingObject != null)
        {
            
           
            pendingObject.transform.position = position;
            pendingObject.GetComponent<Collider>().isTrigger = true;
            UpdateMaterials();

        }

    }

    public void DestroyPendingObject()
    {
        if (pendingObject != null)
        {
            Destroy(pendingObject.gameObject);
            pendingObject = null;
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

        if (Physics.Raycast(ray, out hit, 10000, layerMask))
        {
            position = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        print("trys to buy furniture");
        //if (ownedObjects[index].value <= PlayerStats.money)
        //{
            print("buys furnitre");
            pendingObject = Instantiate(ownedObjects[index].objectPrefab, position, transform.rotation, gameObject.transform);
        
            pendingObject.transform.Rotate(Vector3.up, rotateAmount + 0.1f);
            initialMaterial = pendingObject.GetComponent<MeshRenderer>().material;
        //pendingObject.GetComponent<Furniture>().idSO = index.ToString();
        Furniture furniture = pendingObject.GetComponent<Furniture>();
            furniture.furnitureSO = ownedObjects[index];
        furniture.isPending = true;
            pendingObject.SetActive(true);

        PlayerStats.Instance.BuyWithMoney(ownedObjects[index].value);
        PlayerStats.Instance.AddReputation(ownedObjects[index].reputation);
        //}

    }

    public bool CheckIfAffordFurniture(int index)
    {
        print("check if afford" + ownedObjects[index] + " " + ownedObjects[index].value);
        if (ownedObjects[index].value <= PlayerStats.Instance.money)
        {
            return true;
        }

        return false;
    }
    public void PlaceObject()
    {
        Furniture pendingFurniture = pendingObject.GetComponent<Furniture>();

        //if (pendingFurniture.furnitureSO.value <= PlayerStats.money)
        //{
            pendingObject.GetComponent<Collider>().isTrigger = false;
            pendingObject.GetComponent<MeshRenderer>().material = initialMaterial;

            pendingObject = null;

            pendingFurniture.isPending = false;

            //PlayerStats.BuyWithMoney(pendingFurniture.furnitureSO.value);
            //PlayerStats.AddReputation(pendingFurniture.furnitureSO.reputation);
        //}
    }

    public void RotateObject()
    {
        //round the rotation and then add 0.1 to y rotation because the toon shader makes the object have black sides;
        Vector3 eulerAngle = pendingObject.transform.rotation.eulerAngles;
        eulerAngle.y = Mathf.Ceil(pendingObject.transform.rotation.y);
        pendingObject.transform.Rotate(Vector3.up, rotateAmount+0.1f);
    }

    //private void OnDisable()
    //{
    //   SaveSystem.SaveFurniture(this);
    //}

    public void LoadFurniture(FurnitureDataToSave data)
    {
        //FurnitureDataToSave data = SaveSystem.LoadData<FurnitureDataToSave>();

        if (data is null)
        {
            Debug.LogError("data is null");
            return;
        }
        foreach (FurnitureDataToSave.FurnitureTransformData transformData in data.furnitureDatasList)
        {
            GameObject furniture;
            foreach (FurnitureSO furnitureSO in ownedObjects)
            {
                if (furnitureSO.id == transformData.id)
                {
                    furniture = Instantiate(
                       furnitureSO.objectPrefab,
                       new Vector3(transformData.furniturePosition[0], transformData.furniturePosition[1], transformData.furniturePosition[2]),
                       Quaternion.Euler(transformData.furnitureRotation[0], transformData.furnitureRotation[1] + 0.1f, transformData.furnitureRotation[2]),
                       gameObject.transform);

                    furniture.GetComponent<Furniture>().furnitureSO = furnitureSO;
                    furniture.SetActive(true);

                    furniture.GetComponent<Outline>().enabled = false;
                    furniture.GetComponent<Collider>().isTrigger = false;

                    break;
                }

            }

            //GameObject furniture = Instantiate(
            //    ownedObjects[Int32.Parse(transformData.id)].objectPrefab,
            //    new Vector3(transformData.furniturePosition[0], transformData.furniturePosition[1], transformData.furniturePosition[2]),
            //    Quaternion.Euler(transformData.furnitureRotation[0], transformData.furnitureRotation[1] + 0.1f, transformData.furnitureRotation[2]),
            //    gameObject.transform
            //    );

            print("loaded rotation " + transformData.furnitureRotation[1]);
            //furniture.GetComponent<Furniture>().idSO = transformData.id.ToString();
            //foreach (FurnitureSO furnitureSO in ownedObjects)
            //{
            //    if (furnitureSO.id == transformData.id.ToString())
            //        furniture.GetComponent<Furniture>().furnitureSO = furnitureSO;

            //}
              //  furniture.GetComponent<Furniture>().furnitureSO = transformData.;
            
            //furniture.SetActive(true);

            //furniture.GetComponent<Outline>().enabled = false;
            //furniture.GetComponent<Collider>().isTrigger = false;
        }

        //Load walls paint
        currentWallPaintID = data.wallPaintID;
        print(data.wallPaintID);
        List<Material> materialList = new List<Material>();

        foreach (PaintSO paint in ownedWallMaterials)
            if (paint.ID == currentWallPaintID)
            {
                print("found paintso");
                materialList.Add(paint.paintMaterial);
                break;
            }

        foreach (Transform wall in wallsParent)
        {
            wall.GetComponent<MeshRenderer>().SetMaterials(materialList);
        }
    }
}
