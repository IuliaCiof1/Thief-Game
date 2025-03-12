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

    public static void RemoveMoiney(int amount)
    {
        money -= amount;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            AddMoiney(100);
        }
    }
}
