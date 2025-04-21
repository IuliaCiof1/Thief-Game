using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveDataToSave
{
   public List<string>activeoOjectivesID;


    public ObjectiveDataToSave(ObjectiveManager objectiveManager)
    {
        activeoOjectivesID = new List<string>();
        Debug.Log("in objectivedatatosave ");
        foreach (Objective obj in objectiveManager.activeobjectives)
        {
            Debug.Log("in objectivedatatosave " + obj.isActive);
            activeoOjectivesID.Add(obj.id);
            //obj.isActive = false;
        }
    }
}
