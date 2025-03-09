using System.Collections.Generic;
using UnityEngine;

public class Equipmentizer : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer TargetMeshRenderer;

    void Start()
    {
        SkinnedMeshRenderer myRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        //print(TargetMeshRenderer);
        if (TargetMeshRenderer == null)
        {
            print("null");
            return;
        }

        Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
        foreach (Transform bone in TargetMeshRenderer.bones)
        {
           
            boneMap[bone.gameObject.name] = bone;
            if(bone.position == Vector3.zero)
                Debug.Log(boneMap[bone.gameObject.name] + " " + bone.gameObject.name);
        }

        

        Transform[] newBones = new Transform[myRenderer.bones.Length];
        for (int i = 0; i < myRenderer.bones.Length; ++i)
        {
            GameObject bone = myRenderer.bones[i].gameObject;
            if (!boneMap.TryGetValue(bone.name, out newBones[i]))
            {
                Debug.Log("Unable to map bone \"" + bone.name + "\" to target skeleton.");
                break;
            }
        }
        myRenderer.bones = newBones;

    }
}
