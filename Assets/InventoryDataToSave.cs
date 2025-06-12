using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryDataToSave
{
    

    public List<string> inventoryIdList { get; private set; }

    public InventoryDataToSave(Inventory inventory)
    {
        //Debug.Log("in furnitureedatatosave ");
        inventoryIdList = new List<string>();

        foreach (InventoryItem item in inventory.ownedItems)
        {
            inventoryIdList.Add(item.id);

          
        }

      
    }
}
