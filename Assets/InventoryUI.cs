using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] RectTransform inventorySlot;
    [SerializeField] RectTransform slotContainer;
    [SerializeField] RectTransform emptyContainer;
    [SerializeField] GameObject inventoryDisplay;
    [SerializeField] float offset;

    private void Start()
    {
        inventoryDisplay.SetActive(false);
    }

    public void DisplayInventory(Inventory inventory)
    {
        int itemIndex = 0;
        RectTransform newSlot;

        inventoryDisplay.SetActive(true);

        //Add the slots to ui inventory
        foreach (InventoryItem item in inventory.ownedItems)
        {
            itemIndex++;

            if (slotContainer.childCount - 1 < inventory.ownedItems.IndexOf(item))
            {
                newSlot = Instantiate(inventorySlot, slotContainer);
                slotContainer.sizeDelta = new Vector2(slotContainer.sizeDelta.x, slotContainer.sizeDelta.y + newSlot.sizeDelta.y + offset);
            }
            else
                newSlot = slotContainer.GetChild(inventory.ownedItems.IndexOf(item)) as RectTransform;

            newSlot.GetChild(0).GetComponent<Image>().sprite = item.sprite;
        }

        if(itemIndex>0)
            emptyContainer.gameObject.SetActive(false);
        else
            emptyContainer.gameObject.SetActive(true);

        //Check for any unused inventory slots
        if (slotContainer.childCount > inventory.transform.childCount)
        {
            for (int i = slotContainer.childCount - 1; i >= itemIndex; i--)
            {
                print(i);
                slotContainer.sizeDelta = new Vector2(slotContainer.sizeDelta.x, slotContainer.sizeDelta.y - inventorySlot.sizeDelta.y - offset);
                Destroy(slotContainer.GetChild(i).gameObject);
            }
        }

    }

    public void UndisplayInventory()
    {
        if (inventoryDisplay != null)
            inventoryDisplay.SetActive(false);
        else
            print("slot container null"+gameObject.name);
    }
}
