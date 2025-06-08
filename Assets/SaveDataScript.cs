using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataScript : MonoBehaviour
{
    BuildingManager buildingManager;
    ObjectiveManager objectiveManager;
    FamilyManager familyManager;

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
        buildingManager = FindAnyObjectByType<BuildingManager>();
        objectiveManager = FindAnyObjectByType<ObjectiveManager>();
        familyManager = FindAnyObjectByType<FamilyManager>();
        //SaveSystem.Load();

        GameData loadedData = SaveSystem.Load();
        if (loadedData != null)
        {
            FurnitureDataToSave furnitureData = loadedData.furnitureData;
            ObjectiveDataToSave objectiveData = loadedData.objectiveData;

            if (buildingManager != null)
                buildingManager.LoadFurniture(furnitureData);
            if (familyManager != null)
                familyManager.LoadData();
            if (objectiveManager != null)
                objectiveManager.LoadData(objectiveData);

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

        SaveSystem.Save(buildingManager, objectiveManager);
        yield return new WaitForSeconds(1.5f); //wait until file is written
        
        //foreach(Objective obj in objectiveManager.objectives)
        //    obj.isActive = false;
    }
}
