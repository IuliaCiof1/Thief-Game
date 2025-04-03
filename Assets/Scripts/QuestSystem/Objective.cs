using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objective")]
public class Objective: ScriptableObject
{
    public string title;
    public string description;
    public bool isCompleted;
    public bool isActive;

    //public FamilyMember familyMember;
    public GameObject objectNeeded;
    public int moneyNeeded;
    public float healthTaken;

    public string objectiveEventName;

    private void OnDisable()
    {
        isCompleted = false;
    }

    private void OnEnable()
    {
        isCompleted = false;
    }

    //public abstract void ActivateObjective();
    //public abstract void DeactivateObjective();

    //public void Complete()
    //{
    //    isCompleted = true;
    //    isActive = false;
    //    ObjectiveEvents.ObjectiveCompleted(this); // Notify system that this objective is complete
    //}

  

    public void ActivateObjective()
    {
        //ObjectiveEvents.SubscribeEvent(objectiveEventName, Complete);
        isActive = true;

       // familyMember.GetComponent<FamilyMember>().currentObjective = this;
        //ObjectiveEvents.OnOpenSpookyMail += Complete;
        Debug.Log("Objective started: " + description);

    }

    public void DeactivateObjective()
    {
        //ObjectiveEvents.OnOpenSpookyMail -= Complete;
       // ObjectiveEvents.UnsubscribeEvent(objectiveEventName, Complete);
        isCompleted = true;
        isActive = false;
        //familyMember.GetComponent<FamilyMember>().currentObjective = null;

        Debug.Log("Objective completed: " + description);
    }
}
