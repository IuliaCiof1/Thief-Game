using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] RectTransform inventorySlot;
    [SerializeField] RectTransform slotContainer;

    private void Start()
    {
        slotContainer.gameObject.SetActive(false);
    }

    public void DisplayInventory(Inventory inventory)
    {
        RectTransform newSlot;

        slotContainer.gameObject.SetActive(true);


        int itemIndex = 0;

        foreach (Transform item in inventory.transform)
        {
            itemIndex++;

            if (slotContainer.childCount - 1 < item.GetSiblingIndex())
            {
                newSlot = Instantiate(inventorySlot, slotContainer);
                slotContainer.sizeDelta = new Vector2(slotContainer.sizeDelta.x, slotContainer.sizeDelta.y + newSlot.sizeDelta.y);
            }
            else
                newSlot = slotContainer.GetChild(item.GetSiblingIndex()) as RectTransform;

            newSlot.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<InventoryItem>().sprite;
            print("rect sizes " + slotContainer.sizeDelta.y + " " + newSlot.sizeDelta.y);

        }
        print("itemindex " + itemIndex);
        //Check for any unused inventory slots
        if (slotContainer.childCount > inventory.transform.childCount)
        {
            for (int i = slotContainer.childCount - 1; i >= itemIndex; i--)
            {
                print(i);
                slotContainer.sizeDelta = new Vector2(slotContainer.sizeDelta.x, slotContainer.sizeDelta.y - inventorySlot.sizeDelta.y);
                Destroy(slotContainer.GetChild(i).gameObject);
            }
        }

    }

    public void UndisplayInventory()
    {
        if (slotContainer != null)
            slotContainer.gameObject.SetActive(false);
        else
            print("slot container null"+gameObject.name);
    }
}
