using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Collections;

[System.Serializable]
public class GameData
{
    public FurnitureDataToSave furnitureData;
    public ObjectiveDataToSave objectiveData;
    public InventoryDataToSave inventoryData;

    public GameData(BuildingManager buildingManager, ObjectiveManager objectiveManager, Inventory inventory)
    {
        //if(buildingManager!=null)
        //    furnitureData = new FurnitureDataToSave(buildingManager);
        //objectiveData = new ObjectiveDataToSave(objectiveManager);
    }
}

public static class SaveSystem
{
    const string saveFileName = "saveddata.bin";


    static ObjectiveManager objectiveManager_;
    public static void Save(BuildingManager buildingManager, ObjectiveManager objectiveManager, Inventory inventory)
    {
       

        objectiveManager_ = objectiveManager;
   

        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        BinaryFormatter formatter = new BinaryFormatter();

        GameData data;

        // Load existing file if it exists
        if (File.Exists(path))
        {
            FileStream loadStream = new FileStream(path, FileMode.Open);
            data = formatter.Deserialize(loadStream) as GameData;
            loadStream.Close();
        }
        else
        {
            data = new GameData(null, null, null); // Start with empty data if no save exists
        }

        // Only update parts that were passed
        if (buildingManager != null)
            data.furnitureData = new FurnitureDataToSave(buildingManager);

        if (objectiveManager != null)
        {
            Debug.Log("SaveSystem:: going to save the objectives");
            data.objectiveData = new ObjectiveDataToSave(objectiveManager);
        }
        else
            Debug.Log("SaveSystem:: objective manager is null");

        if (inventory != null)
            data.inventoryData = new InventoryDataToSave(inventory);

        // Save everything back to file
        FileStream saveStream = new FileStream(path, FileMode.Create);
        formatter.Serialize(saveStream, data);
        saveStream.Close();
        
        Debug.LogWarning("Saved game data (partial or full) to: " + Application.persistentDataPath+ Path.Combine(Application.persistentDataPath, saveFileName));
        
    }

   

    public static GameData Load()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(path))
        {
            FileInfo info = new FileInfo(path);
            Debug.Log($"File size: {info.Length} bytes");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            Debug.LogWarning("Game loaded from: " + Application.persistentDataPath + path);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static bool DoesFileExist()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(path))
            return true;

        return false;
    }

    public static void DeleteSaveFile()
    {
        PlayerPrefs.DeleteAll();
        File.Delete(Path.Combine(Application.persistentDataPath, saveFileName));
        
        //ResetObjectivesSO();
        Debug.LogWarning("save file deleted");
    }
}

