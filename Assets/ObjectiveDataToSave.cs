using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectiveDataToSave
{

    [System.Serializable]
    public struct ObjectiveData
    {
        public string id;
        public bool isActive;
        public int leftCooldown;
    }

    //public List<string>activeoOjectivesID;
    public List<ObjectiveData>objectives;


    public ObjectiveDataToSave(ObjectiveManager objectiveManager)
    {
        objectives = new List<ObjectiveData>();

       
        foreach (Objective obj in objectiveManager.objectives)
        {

            ObjectiveData objectiveData;
            objectiveData.isActive = obj.isActive;
            objectiveData.id = obj.id;
            objectiveData.leftCooldown = obj.leftCooldown;
            Debug.Log("ObjectiveDataToSave:: " + obj.id + " " + obj.name);

            objectives.Add(objectiveData);
            //obj.isActive = false;
        }
    }
}
