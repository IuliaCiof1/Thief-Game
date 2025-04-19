using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    const string saveFileName = "/saveddata.bin";

    public static void SaveFurniture(BuildingManager buildingManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, saveFileName);  //gets a safe path that won't change with different OS

        FileStream stream = new FileStream(path, FileMode.Create);

        FurnitureData data = new FurnitureData(buildingManager);

        formatter.Serialize(stream, data); //format data to binary  and write it to binary file

        stream.Close();
    }

    public static FurnitureData LoadFurniture()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);  //gets a safe path that won't change with different OS

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FurnitureData data = formatter.Deserialize(stream) as FurnitureData; //format data back to normal

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSaveFile()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, saveFileName));
        Debug.LogWarning("save file deleted");
    }
}
