using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; } //singleton lazy instantion
    public static int money { get; private set; }

    //private void Awake()
    //{
    //    if (Instance is null)
    //    {
    //        Instance = this;
    //        //DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //        Destroy(gameObject);
    //}

    public void AddMoiney(int amount)
    {
        money += amount;
    }

    //Return true if player has enough money or false otherwise
    public static bool RemoveMoney(int amount)
    {
        if (money - amount > 0)
        {
            money -= amount;
            return true;
        }

        return false;

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            AddMoiney(100);
        }
    }
}
