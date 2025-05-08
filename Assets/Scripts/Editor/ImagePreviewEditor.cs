using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FurnitureSO))] //Used for FurnitureSO scriptable objects
public class ImagePreviewEditor : Editor
{
    FurnitureSO furnitureSO;

    private void OnEnable()
    {
        furnitureSO = target as FurnitureSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (furnitureSO.previewImage == null)
            return;

        //Get sprite
        Texture2D sprite = AssetPreview.GetAssetPreview(furnitureSO.previewImage);

        //Set image size
        GUILayout.Label("", GUILayout.Height(120), GUILayout.Width(120));

        //Draw the image
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), sprite);
    }
}
