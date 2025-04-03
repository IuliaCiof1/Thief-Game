using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    //Make the inventory persist between scenes
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance is null)
        {
            print("inventory instance is null");
            instance = this;
        }
        else
        {

            print("inventory instance is NOT null");

            Destroy(gameObject);
        }
    }

}
