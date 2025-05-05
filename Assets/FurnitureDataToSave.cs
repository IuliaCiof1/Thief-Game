using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FurnitureDataToSave
{
    [System.Serializable]
    public struct FurnitureTransformData
    {
        public string id;
        public float[] furniturePosition;
        public float[] furnitureRotation;
    }

    public List<FurnitureTransformData> furnitureDatasList { get; private set; }

    public FurnitureDataToSave(BuildingManager buildingManager)
    {
        Debug.Log("in furnitureedatatosave ");
        furnitureDatasList = new List<FurnitureTransformData>();

        foreach(Transform furniture in buildingManager.transform)
        {
            FurnitureTransformData furnitureData;
        
            furnitureData.furniturePosition = new float[] {furniture.position.x,  furniture.position.y, furniture.position.z};
            furnitureData.furnitureRotation = new float[] {furniture.rotation.eulerAngles.x,  furniture.rotation.eulerAngles.y, furniture.rotation.eulerAngles.z};

            Debug.Log("FurnitureDataToSave:: furniture y rotation: " + furniture.rotation.y);
            furnitureData.id = furniture.GetComponent<Furniture>().idSO;

            furnitureDatasList.Add(furnitureData);
        }
    }

  
}
