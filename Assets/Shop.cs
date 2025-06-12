using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] InventoryItem goods;
    [SerializeField] int goodsPrice;
    Inventory inventory;

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    public void BuyGoods()
    {
        if (!inventory.CheckIfItemExists(goods.id))
        {

            if (PlayerStats.Instance.BuyWithMoney(goodsPrice))
            {
                inventory.AddToInventory(goods);
               // GameObject goods = Instantiate(goodsPrefab, inventory);


                //DontDestroyOnLoad(goods);
            }
        }
        
    }

    //public bool CheckIfItemExists(string itemName)
    //{
    //    foreach (Transform item in inventory.transform)
    //    {
    //        if (item.name.Contains(itemName))
    //            return true;
    //    }

    //    return false;
    //}


    public string GetGoodsName()
    {
        if (!goods)
        {
            Debug.LogError("No goodsPrefab assigned to " + gameObject.name);
            return "";
        }

        return goods.id;

    }

    public int GetGoodsPrice()
    {
        if (!goods)
        {
            Debug.LogError("No goodsPrefab assigned to " + gameObject.name);
            return 0;
        }

        return goodsPrice;

    }
}
