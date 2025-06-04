using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    InventoryUI inventoryUI;

    //Make the inventory persist between scenes
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        print("look for inventory");
        inventoryUI = FindObjectOfType<InventoryUI>();

        DontDestroyOnLoad(gameObject);

        if (instance is null)
        {
          
            instance = this;
        }
        else
        {

            
            Destroy(gameObject);
        }

       
    }


    public bool CheckIfItemExists(string itemName)
    {
        foreach (Transform item in transform)
        {
            if (item.name.Contains(itemName))
                return true;
        }

        return false;
    }

    public void AddToInventory(GameObject item)
    {
        Instantiate(item, transform);
    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            inventoryUI.DisplayInventory(this);
        }
        else
            inventoryUI.UndisplayInventory();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }
}
