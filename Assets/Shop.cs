using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject goodsPrefab;
    [SerializeField] int goodsPrice;

    public void BuyGoods()
    {
        

        if (PlayerStats.BuyWithMoney(goodsPrice))
        {
            Transform inventory = FindAnyObjectByType<Inventory>().transform;
            GameObject goods = Instantiate(goodsPrefab,inventory);

            //DontDestroyOnLoad(goods);
        }

        
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
