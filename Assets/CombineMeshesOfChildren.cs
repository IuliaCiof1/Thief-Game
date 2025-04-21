using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMeshesOfChildren : MonoBehaviour
{
    [SerializeField]
    MeshFilter[] meshFilters;
    [SerializeField]
    CombineInstance[] combine;
    [SerializeField] MeshFilter thismeshFilter;
    [ContextMenu("Combine Meshes")]
    private void CombineMesh()
    {
       meshFilters = GetComponentsInChildren<MeshFilter>();
         combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            //meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        SaveMesh(thismeshFilter.sharedMesh, gameObject.name, false, true);

        //transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        //transform.gameObject.SetActive(true);
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
