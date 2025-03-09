using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isCollapsed;
    public List<Prototype> possiblePrototypes;
    public List<int> prototypeWeights;
    public Vector2 coords = new Vector2();
    public Cell posXneighbour;
    public Cell negXneighbour;
    public Cell posZneighbour;
    public Cell negZneighbour;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;



    public void GenerateWeight(Weights weights)
    {
        if (possiblePrototypes == null || possiblePrototypes.Count == 0)
        {
           
            return;
        }

        prototypeWeights = new List<int>(new int[possiblePrototypes.Count]);
        int i = 0;

        foreach (Prototype p in possiblePrototypes)
        {
            if (p.attributes == null || p.attributes.Count == 0)
            {
                
                prototypeWeights[i] = 5;
            }
            else
            {

                foreach (Attribute_ attribute in p.attributes)
                {
                    prototypeWeights[i] += weights.GetWeight(attribute);
                }

                prototypeWeights[i] = (int)((float)prototypeWeights[i] / (float)p.attributes.Count);
            }
            i++;
        }

       
    }

}