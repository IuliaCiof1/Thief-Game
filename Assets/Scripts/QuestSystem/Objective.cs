using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Objective")]
public class Objective: ScriptableObject
{
    //public string Title;
    //public string Description;
    //public bool RequiresItem;
    //public InventoryItem RequiredItem;
    //public int MoneyCost;
    //public float HealthReward;
    //public int leftCooldown;

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


    //[NonSerialized]
    //public bool isActive;

    public void Activate()
    {
        isActive = true;
        Debug.Log($"Objective Started: {title}");
    }

    public void Complete()
    {
        isActive = false;
        leftCooldown = objectiveCooldown;
        Debug.Log($"Objective Completed: {title}");
    }

    public void Reset()
    {
        isActive = false;
        leftCooldown = 0;
        Debug.Log($"Objective Reset: {title}");
    }


    //public string title;
    //public string description;
    //public bool isCompleted;
    //public bool isActive;

    ////public FamilyMember familyMember;
    //public bool itemRequierd;
    //public InventoryItem objectNeeded;
    //public int moneyNeeded;
    //public float healthTaken;

    //public string objectiveEventName;
    //public string id;
    //public string member;
    //public int objectiveCooldown;
    //public int leftCooldown;

    //private void OnDisable()
    //{
    //    isCompleted = false;
    //}

    //private void OnEnable()
    //{
    //    isCompleted = false;
    //}

    ////public abstract void ActivateObjective();
    ////public abstract void DeactivateObjective();

    ////public void Complete()
    ////{
    ////    isCompleted = true;
    ////    isActive = false;
    ////    ObjectiveEvents.ObjectiveCompleted(this); // Notify system that this objective is complete
    ////}



    //public void ActivateObjective()
    //{
    //    //ObjectiveEvents.SubscribeEvent(objectiveEventName, Complete);
    //    isActive = true;

    //   // familyMember.GetComponent<FamilyMember>().currentObjective = this;
    //    //ObjectiveEvents.OnOpenSpookyMail += Complete;
    //    Debug.Log("Objective started: " + description);

    //}

    //public void DeactivateObjective()
    //{
    //    //ObjectiveEvents.OnOpenSpookyMail -= Complete;
    //    // ObjectiveEvents.UnsubscribeEvent(objectiveEventName, Complete);
    //    leftCooldown = objectiveCooldown;
    //    isCompleted = true;
    //    isActive = false;
    //    //familyMember.GetComponent<FamilyMember>().currentObjective = null;

    //    Debug.Log("Objective completed: " + description);
    //}
}
