using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu (menuName = "Furniture")]
public class FurnitureSO : ScriptableObject
{
    public Sprite previewImage;
    public GameObject objectPrefab;
    //public string previewsFolder = "Assets/Data/ObjectPreviews/";
    
}
