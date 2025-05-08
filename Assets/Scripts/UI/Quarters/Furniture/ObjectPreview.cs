using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;


public class ObjectPreview : MonoBehaviour
{
    //This script creates images at filePath of the Game Objects inside ObjectsContainer
    //These images are used as preview images for the furniture inventory

    string filePath = "Assets/Data/ObjectPreviews/";
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] GameObject objectsContainer;
    [SerializeField] Camera photographer;

    public bool generatePreviews;
   

    //When the Generate Previws is checked, start creating the previews
    void OnValidate()
    {
        if (generatePreviews)
        {
            photographer.gameObject.SetActive(true);

            generatePreviews = false; // Reset the flag to prevent continuous generation
            EditorCoroutineUtility.StartCoroutineOwnerless(CaptureAllPreviews());
        }
    }
    
    

    private IEnumerator CaptureAllPreviews()
    {
        foreach (Transform child in objectsContainer.transform)
        {
            yield return EditorCoroutineUtility.StartCoroutineOwnerless(CreatePreview(child));
        }

        photographer.gameObject.SetActive(false);

    }

    private IEnumerator CreatePreview(Transform child)
    {
        // Activate the game object
        child.gameObject.SetActive(true);

        // Wait for the end of the frame to ensure the object is fully rendered
        yield return new WaitForEndOfFrame();

        // Create a new texture with the same dimensions as the render texture
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        // Set the active RenderTexture and read the pixels into the texture
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null; // Deactivate the RenderTexture



        child.gameObject.SetActive(false);
        // Save the texture as a PNG file
        System.IO.File.WriteAllBytes(filePath + child.name + ".png", texture.EncodeToPNG());


        // Wait for a bit to ensure the file is written and imported
        yield return new WaitForSeconds(2f);
        string fullPath = filePath + child.name + ".png";
#if UNITY_EDITOR
        // Import the texture as a Sprite
        AssetDatabase.ImportAsset(fullPath, ImportAssetOptions.ForceUpdate);
        yield return new WaitForSeconds(0.5f); // Wait for the asset database to update

        TextureImporter importer = AssetImporter.GetAtPath(fullPath) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            AssetDatabase.WriteImportSettingsIfDirty(fullPath);
            AssetDatabase.ImportAsset(fullPath, ImportAssetOptions.ForceUpdate);
            yield return new WaitForSeconds(0.5f); // Wait for the asset database to update again
        }

        // Load the sprite from the file
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(fullPath);

        // Create and assign the scriptable object
        CreateFurnitureSO(child, sprite);

#endif

        // Deactivate the game object
        //child.gameObject.SetActive(false);
    }

   

#if UNITY_EDITOR
    private void CreateFurnitureSO(Transform child, Sprite sprite)
    {
        // Create a new instance of Furniture ScriptableObject
        FurnitureSO furnitureSO = ScriptableObject.CreateInstance<FurnitureSO>();

        // Assign the sprite to the ScriptableObject
        furnitureSO.previewImage = sprite;

        // Create and assign the prefab
        GameObject prefab = CreatePrefab(child.gameObject);
        furnitureSO.objectPrefab = prefab;

        // Optionally, save the ScriptableObject asset if you want to persist it
        string assetPath = "Assets/Data/Furnitures/SO/" + child.name + ".asset";

        // Create the directory if it doesn't exist
        string directory = Path.GetDirectoryName(assetPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Save the ScriptableObject asset
        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(assetPath)))
            AssetDatabase.CreateAsset(furnitureSO, assetPath);
        else
        {
           FurnitureSO foundFurnitureSO = (FurnitureSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(FurnitureSO));
            foundFurnitureSO.previewImage = furnitureSO.previewImage;
            foundFurnitureSO.objectPrefab = furnitureSO.objectPrefab;
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject saved at: " + assetPath);
    }
#endif
   

#if UNITY_EDITOR
    private GameObject CreatePrefab(GameObject obj)
    {
        string prefabPath = "Assets/Data/Furnitures/Prefabs/" + obj.name + ".prefab";

        // Create the directory if it doesn't exist
        string directory = Path.GetDirectoryName(prefabPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Create the prefab
        BoxCollider col;
        if (!obj.TryGetComponent<BoxCollider>(out BoxCollider c) && !obj.TryGetComponent<MeshCollider>(out MeshCollider m))
        {
            col = obj.AddComponent<BoxCollider>();

        }
        else {col = obj.GetComponent<BoxCollider>(); }

        //col.convex = true;
        //col.isTrigger = true;

        Rigidbody rg;
        if (!obj.TryGetComponent<Rigidbody>(out Rigidbody r))
        {
           rg = obj.AddComponent<Rigidbody>();
        }
        else { rg = obj.GetComponent<Rigidbody>(); }

        rg.isKinematic = true;
        rg.useGravity = false;
        rg.collisionDetectionMode = CollisionDetectionMode.Continuous;

        if (!obj.TryGetComponent<Furniture>(out Furniture f))
        {
            obj.AddComponent<Furniture>();
        }

        if (!obj.TryGetComponent<CheckObjectPlacement>(out CheckObjectPlacement p))
        {
            obj.AddComponent<CheckObjectPlacement>();
        }

        if (!obj.TryGetComponent<Outline>(out Outline o))
        {
            obj.AddComponent<Outline>();
        }

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(obj, prefabPath);
        Debug.Log("Prefab created at: " + prefabPath);
        prefab.gameObject.SetActive(true);

        return prefab;
    }
#endif

}
#endif