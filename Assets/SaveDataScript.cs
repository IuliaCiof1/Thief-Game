using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataScript : MonoBehaviour
{
   [SerializeField] BuildingManager buildingManager;
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] FamilyManager familyManager;
    [SerializeField] Inventory inventory;

    public static SaveDataScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        print("load");
        //buildingManager = FindAnyObjectByType<BuildingManager>();
        //objectiveManager = FindAnyObjectByType<ObjectiveManager>();
        //familyManager = FindAnyObjectByType<FamilyManager>();
        //inventory = FindAnyObjectByType<Inventory>();
        //SaveSystem.Load();

        GameData loadedData = SaveSystem.Load();
        if (loadedData != null)
        {
            FurnitureDataToSave furnitureData = loadedData.furnitureData;
            ObjectiveDataToSave objectiveData = loadedData.objectiveData;
            InventoryDataToSave inventoryData = loadedData.inventoryData;

            if (buildingManager != null)
                buildingManager.LoadFurniture(furnitureData);
            if (objectiveManager != null)
                objectiveManager.LoadData(objectiveData);
            if (familyManager != null)
                familyManager.LoadData();
            
            if (inventory != null)
                inventory.LoadData(inventoryData);

        }
        //else
        //{
        //    // No save file found, this is a new game — reset objectives manually
        //    Debug.Log("SaveDataScript: No save found. Starting new game — resetting objective state.");
        //    if (objectiveManager != null)
        //    {
        //        foreach (Objective obj in objectiveManager.objectives)
        //        {
        //            obj.isActive = false;
        //        }
        //        objectiveManager.activeobjectives.Clear();
        //    }
        //}
    }
    //private void OnDisable()
    //{
    //    Save();

    //}

    private void OnApplicationQuit()
    {
        StartCoroutine(Save());
    }

    public IEnumerator Save()
    {
        print("save");

        if (inventory is null)
            print("SaveDataScript:: inventory is null");
        else
            print("SaveDataScript:: inventory is not null");

        SaveSystem.Save(buildingManager, objectiveManager,inventory);
        yield return new WaitForSeconds(1.5f); //wait until file is written
        
        //foreach(Objective obj in objectiveManager.objectives)
        //    obj.isActive = false;
    }
}
