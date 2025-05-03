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

    public GameData(BuildingManager buildingManager, ObjectiveManager objectiveManager)
    {
        //if(buildingManager!=null)
        //    furnitureData = new FurnitureDataToSave(buildingManager);
        //objectiveData = new ObjectiveDataToSave(objectiveManager);
    }
}

public static class SaveSystem
{
    const string saveFileName = "/saveddata.bin";

    //public static void SaveFurniture(BuildingManager buildingManager)
    //{
    //    FurnitureDataToSave data = new FurnitureDataToSave(buildingManager);
    //    //ObjectiveDataToSave objectiveData = new ObjectiveDataToSave()

    //    //BinaryFormatter formatter = new BinaryFormatter();
    //    //string path = Path.Combine(Application.persistentDataPath, saveFileName);  //gets a safe path that won't change with different OS

    //    //FileStream stream = new FileStream(path, FileMode.Append);



    //    //formatter.Serialize(stream, furnitureData); //format data to binary  and write it to binary file

    //    //Debug.LogWarning("save file created in "+ Application.persistentDataPath + path);

    //    //stream.Close();

    //    Save<FurnitureDataToSave>(data);
    //}

    //public static void SaveObjectives(ObjectiveManager objectiveManager)
    //{
    //    ObjectiveDataToSave data = new ObjectiveDataToSave(objectiveManager);

    //    Save(data);
    //}

    //private static void Save<T>(T data)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();
    //    string path = Path.Combine(Application.persistentDataPath, saveFileName);  //gets a safe path that won't change with different OS

    //    FileStream stream = new FileStream(path, FileMode.Create);



    //    formatter.Serialize(stream, data); //format data to binary  and write it to binary file

    //    Debug.LogWarning("save file created in " + Application.persistentDataPath + path);

    //    stream.Close();
    //}

    ////public static FurnitureDataToSave LoadFurniture()
    ////{
    ////    string path = Path.Combine(Application.persistentDataPath, saveFileName);  //gets a safe path that won't change with different OS

    ////    if (File.Exists(path))
    ////    {
    ////        BinaryFormatter formatter = new BinaryFormatter();
    ////        FileStream stream = new FileStream(path, FileMode.Open);

    ////        FurnitureDataToSave furnitureData = formatter.Deserialize(stream) as FurnitureDataToSave; //format data back to normal

    ////        stream.Close();
    ////        return furnitureData;
    ////    }
    ////    else
    ////    {
    ////        Debug.LogError("Save file not found in " + path);
    ////        return null;
    ////    }
    ////}


    ////public static ObjectiveDataToSave LoadObjectives()
    ////{
    ////    string path = Path.Combine(Application.persistentDataPath, saveFileName);  //gets a safe path that won't change with different OS

    ////    if (File.Exists(path))
    ////    {
    ////        BinaryFormatter formatter = new BinaryFormatter();
    ////        FileStream stream = new FileStream(path, FileMode.Open);

    ////        ObjectiveDataToSave objectiveData = formatter.Deserialize(stream) as ObjectiveDataToSave; //format data back to normal

    ////        stream.Close();
    ////        return objectiveData;
    ////    }
    ////    else
    ////    {
    ////        Debug.LogError("Save file not found in " + path);
    ////        return null;
    ////    }
    ////}


    //public static T LoadData<T>() where T:class
    //{
    //    string path = Path.Combine(Application.persistentDataPath, saveFileName);

    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream stream = new FileStream(path, FileMode.Open);

    //        T data = formatter.Deserialize(stream) as T;

    //        stream.Close();
    //        return data;
    //    }
    //    else
    //    {
    //        Debug.LogError($"Save file not found in {path}");
    //        return null;
    //    }
    //}

    //public static void DeleteSaveFile()
    //{
    //    File.Delete(Path.Combine(Application.persistentDataPath, saveFileName));
    //    Debug.LogWarning("save file deleted");
    //}


    // private static string fileName = "gameData.sav";
    static ObjectiveManager objectiveManager_;
    public static void Save(BuildingManager buildingManager, ObjectiveManager objectiveManager)
    {
        objectiveManager_ = objectiveManager;
        //string path = Path.Combine(Application.persistentDataPath, saveFileName);
        //BinaryFormatter formatter = new BinaryFormatter();
        //FileStream stream = new FileStream(path, FileMode.Create);

        //GameData data = new GameData(buildingManager, objectiveManager);
        //formatter.Serialize(stream, data);
        //FileInfo info = new FileInfo(path);
        //Debug.Log($"File size: {info.Length} bytes");

        //stream.Close();
        //Debug.LogWarning("Saved game to: " + path);



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
            data = new GameData(null, null); // Start with empty data if no save exists
        }

        // Only update parts that were passed
        if (buildingManager != null)
            data.furnitureData = new FurnitureDataToSave(buildingManager);

        if (objectiveManager != null)
            data.objectiveData = new ObjectiveDataToSave(objectiveManager);

        // Save everything back to file
        FileStream saveStream = new FileStream(path, FileMode.Create);
        formatter.Serialize(saveStream, data);
        saveStream.Close();
        //foreach (Objective obj in objectiveManager.objectives)
        //    obj.isActive = false;
        Debug.LogWarning("Saved game data (partial or full) to: " + Application.persistentDataPath+ Path.Combine(Application.persistentDataPath, saveFileName));
        //Application.OpenURL("file://" + Application.persistentDataPath);

        //Task.Delay(100000).ContinueWith(t => ResetObjectivesSO(objectiveManager));
        //DelayHelper.Instance.ResetObjectives(objectiveManager, 0.3f);
    }

    //static void ResetObjectivesSO()
    //{
    //    //yield return new WaitForSeconds(0.5f);
    //    //Now you can safely reset anything in memory if needed
    //    foreach (Objective obj in objectiveManager.objectives)
    //    {
    //        obj.isActive = false;
    //    }
    //}

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

    public static void DeleteSaveFile()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, saveFileName));
        PlayerPrefs.DeleteAll();
        //ResetObjectivesSO();
        Debug.LogWarning("save file deleted");
    }
}


//public static class CoroutineRunner
//{
//    private class CoroutineHelper : MonoBehaviour
//    {
//        private void OnDestroy()
//        {
//            // This will ensure that when the CoroutineHelper is destroyed, 
//            // we clean up any lingering coroutines or references
//            if (_runner != null)
//            {
//                _runner = null;
//            }
//        }
//    }

//    private static CoroutineHelper _runner;

//    public static void RunCoroutine(IEnumerator coroutine)
//    {
//        if (_runner == null)
//        {
//            GameObject go = new GameObject("CoroutineRunner");
//            GameObject.DontDestroyOnLoad(go);
//            _runner = go.AddComponent<CoroutineHelper>();
//        }

//        _runner.StartCoroutine(coroutine);
//    }
//}
