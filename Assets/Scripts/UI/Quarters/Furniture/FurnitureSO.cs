using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu (menuName = "Furniture")]
public class FurnitureSO : ScriptableObject
{
    //public string id;
    public Sprite previewImage;
    public GameObject objectPrefab;
    public int value;
    public int reputation;
    //public string previewsFolder = "Assets/Data/ObjectPreviews/";
    
}
