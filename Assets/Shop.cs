using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject goodsPrefab;
    [SerializeField] int goodsPrice;
    Transform inventory;

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>().transform;
    }

    public void BuyGoods()
    {
        if (!CheckIfItemExists(goodsPrefab.name))
        {

            if (PlayerStats.BuyWithMoney(goodsPrice))
            {

                GameObject goods = Instantiate(goodsPrefab, inventory);


                //DontDestroyOnLoad(goods);
            }
        }
        
    }

    public bool CheckIfItemExists(string itemName)
    {
        foreach (Transform item in inventory.transform)
        {
            if (item.name.Contains(itemName))
                return true;
        }

        return false;
    }


    public string GetGoodsName()
    {
        if (!goodsPrefab)
        {
            Debug.LogError("No goodsPrefab assigned to " + gameObject.name);
            return "";
        }

        return goodsPrefab.name;

    }

    public int GetGoodsPrice()
    {
        if (!goodsPrefab)
        {
            Debug.LogError("No goodsPrefab assigned to " + gameObject.name);
            return 0;
        }

        return goodsPrice;

    }
}
