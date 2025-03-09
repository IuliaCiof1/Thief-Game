using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingRandomizer : MonoBehaviour
{
    public void RandomizeBuilding()
    {
        int randomNumber = UnityEngine.Random.Range(0, transform.childCount-1);

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        transform.GetChild(randomNumber).gameObject.SetActive(true);

        int spawnNPC = UnityEngine.Random.Range(0, 2);
        if(spawnNPC==1)
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
    }
}
