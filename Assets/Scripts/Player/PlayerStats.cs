using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; } //singleton lazy instantion
    public static int money { get; private set; }
    public static int reputation { get; private set; }

    [SerializeField] int maxReputation;
    public static int maxReputation_{ get; private set; }
    [SerializeField] int reputationLoss = 20;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    public int GetReputationLoss()
    {
        return reputationLoss;
    }

    public static int GetMaxReputation()
    {
        return maxReputation_;
    }

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
    public static bool BuyWithMoney(int amount)
    {
        if (money - amount >= 0)
        {
            money -= amount;
            return true;
        }

        return false;

    }

    public static void AddReputation(int amount)
    {
        reputation += amount;
    }

    //Return true if player has enough money or false otherwise
    public void RemoveReputation()
    {
        print("REMOVE REPUTATION");
        reputation -= reputationLoss;

       
    }
    public static void RemoveMoney(int amount)
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

    private void OnEnable()
    {
        maxReputation_ = maxReputation;

        money = PlayerPrefs.GetInt("money", money);
        reputation = PlayerPrefs.GetInt("reputation", reputation);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("reputation", reputation);
    }
}
