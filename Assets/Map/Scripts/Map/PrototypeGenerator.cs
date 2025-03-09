using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
public class PrototypeGenerator : MonoBehaviour
{

    [SerializeField] private List<Prototype> protoypePrefabs;
    public List<Prototype> prototypes = new List<Prototype>();
    public string path = "Assets/Data/Prototypes";
    WFC_Socket posXHolder;
    WFC_Socket negXHolder;
    WFC_Socket posZHolder;
    WFC_Socket negZHolder;
    List<GameObject> prototypeHolder = new List<GameObject>();

    [SerializeField] private GameObject prototypeHolderPrefab;

    [ContextMenu("Generate Prototypes")]
    public void GeneratePrototypes()
    {
        prototypes.Clear();

#if UNITY_EDITOR
        if (Directory.Exists(path))
            Directory.Delete(path);

        Directory.CreateDirectory(path);
#endif

        // Generate rotations for all prototypes
        for (int i = 0; i < protoypePrefabs.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Prototype newProto = CreateMyAsset(path, protoypePrefabs[i].name, j.ToString().Replace(" ", ""));
                prototypes.Add(newProto);
            }
        }
        UpdatePrototypes();

        prototypeHolderPrefab.GetComponent<Cell>().possiblePrototypes = prototypes;


        //this helps to keep the references after unity restart
#if UNITY_EDITOR
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }

    public void UpdatePrototypes()
    {
        // Generate rotations for all prototypes
        for (int i = 0; i < protoypePrefabs.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int index = i * 4 + j;
                prototypes[index].prefab = protoypePrefabs[i].prefab;

                prototypes[index].validNeighbours = new NeighbourList();
                prototypes[index].meshRotation = j;
                prototypes[index].attributes = protoypePrefabs[i].attributes;

                prototypes[index].posX = protoypePrefabs[i].posX;
                prototypes[index].negX = protoypePrefabs[i].negX;
                prototypes[index].posZ = protoypePrefabs[i].posZ;
                prototypes[index].negZ = protoypePrefabs[i].negZ;

                if (j == 0)
                {
                    posXHolder = prototypes[index].posX;
                    negXHolder = prototypes[index].negX;
                    posZHolder = prototypes[index].posZ;
                    negZHolder = prototypes[index].negZ;
                }
                else
                {
                    prototypes[index].negZ = posXHolder;
                    prototypes[index].negX = negZHolder;
                    prototypes[index].posZ = negXHolder;
                    prototypes[index].posX = posZHolder;

                    posXHolder = prototypes[index].posX;
                    negXHolder = prototypes[index].negX;
                    posZHolder = prototypes[index].posZ;
                    negZHolder = prototypes[index].negZ;
                }

                //this helps as well to keep references
#if UNITY_EDITOR
                EditorUtility.SetDirty(prototypes[index]);
#endif
            }
        }

        // Generate valid neighbors
        for (int i = 0; i < prototypes.Count; i++)
            prototypes[i].validNeighbours = GetValidNeighbors(prototypes[i]);
    }

    public static Prototype CreateMyAsset(string assetFolder, string name, string j)
    {
        Prototype asset = ScriptableObject.CreateInstance<Prototype>();

#if UNITY_EDITOR
        AssetDatabase.CreateAsset(asset, assetFolder + "/" + name + "_" + j + ".asset");
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
#endif

        return asset;
    }

    private NeighbourList GetValidNeighbors(Prototype proto)
    {
        NeighbourList neighbourList = new NeighbourList();
        foreach (Prototype p in prototypes)
        {
            if (proto.posX == p.negX)
                neighbourList.posX.Add(p);
            if (proto.negX == p.posX)
                neighbourList.negX.Add(p);
            if (proto.posZ == p.negZ)
                neighbourList.posZ.Add(p);
            if (proto.negZ == p.posZ)
                neighbourList.negZ.Add(p);
        }
        return neighbourList;
    }

    public void DisplayPrototypes()
    {
        if (prototypeHolder.Count != 0)
        {
            foreach (GameObject p in prototypeHolder)
                DestroyImmediate(p);

            prototypeHolder = new List<GameObject>();
        }

        for (int i = 0; i < protoypePrefabs.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject protoObj = Instantiate(protoypePrefabs[i].prefab, new Vector3(i * 1.5f, 0f, j * 1.5f), Quaternion.identity, this.transform);
                protoObj.transform.Rotate(new Vector3(0f, j * 90, 0f), Space.Self);
                protoObj.name = (protoypePrefabs[i].prefab.name + "_" + j.ToString());
                prototypeHolder.Add(protoObj);
            }
        }
    }
}
