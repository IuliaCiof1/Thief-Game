using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Objective")]
public class Objective: ScriptableObject
{
    public string title;
    public string description;
    public bool isActive;
    public bool itemRequierd;
    public InventoryItem objectNeeded;
    public int moneyNeeded;
    public float healthTaken;

    public string objectiveEventName;
    public string id;
    public string member;
    public int objectiveCooldown;
    public int leftCooldown;


    public void Activate()
    {
        isActive = true;
    }

    public void Complete()
    {
        isActive = false;
        leftCooldown = objectiveCooldown;
    }

    public void Reset()
    {
        isActive = false;
        leftCooldown = 0;
    }
}
