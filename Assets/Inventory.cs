using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    //private static Inventory instance;
    InventoryUI inventoryUI;
    [SerializeField] InventoryItem[] allItems;
    [SerializeField] public List<InventoryItem> ownedItems { get; private set; }
    [SerializeField]
    List<InventoryItem> ownedItems_;
    private void Awake()
    {
        print("initialise ownedItems");
        if(ownedItems is null)
            ownedItems = new List<InventoryItem>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        inventoryUI = FindObjectOfType<InventoryUI>();
    }


    public bool CheckIfItemExists(string itemName)
    {
        foreach (InventoryItem item in ownedItems)
        {
            if (item.id.Contains(itemName))
                return true;
        }

        return false;
    }

    public void AddToInventory(InventoryItem item) { ownedItems.Add(item); }

    public void RemoveFromInventory(InventoryItem item){ ownedItems.Remove(item);}

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            ownedItems_ = ownedItems;
            inventoryUI.DisplayInventory(this);
        }
        else
            inventoryUI.UndisplayInventory();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void LoadData(InventoryDataToSave data)
    {
        if (data is null)
        {
            print("no data recovered for obecjtives");
            return;
        }

        ownedItems = new List<InventoryItem>();
        foreach (InventoryItem item in allItems)
        {
            foreach (string id in data.inventoryIdList)
                if (id == item.id)
                    ownedItems.Add(item);
        }
    }
}