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


        //foreach (Objective obj in objectiveManager.allObjectives)
        //{

        //    ObjectiveData objectiveData;
        //    objectiveData.isActive = obj.isActive;
        //    objectiveData.id = obj.id;
        //    objectiveData.leftCooldown = obj.leftCooldown;
        //    Debug.Log("ObjectiveDataToSave:: " + obj.id + " " + obj.name);

        //    objectives.Add(objectiveData);
        //    //obj.isActive = false;
        //}

        Debug.Log("ObjectiveDataToSave:: number of active objects " + objectiveManager.activeObjectives.Count);
        //Add active objectives in the activation order
        foreach (Objective obj in objectiveManager.activeObjectives)
        {
            Debug.Log("ObjectiveDataToSave:: should be ACTIVE " + obj.name + obj.isActive);
            if (!obj.isActive) continue;

            ObjectiveData objectiveData;
            objectiveData.id = obj.id;
            objectiveData.isActive = obj.isActive;
            objectiveData.leftCooldown = obj.leftCooldown;

            Debug.Log("ObjectiveDataToSave:: ACTIVE " + obj.id + " " + obj.name);
            objectives.Add(objectiveData);
        }

        //Add the rest of the inactive objectives
        foreach (Objective obj in objectiveManager.allObjectives)
        {
            if (obj.isActive) continue;

            ObjectiveData objectiveData;
            objectiveData.id = obj.id;
            objectiveData.isActive = obj.isActive;
            objectiveData.leftCooldown = obj.leftCooldown;

            Debug.Log("ObjectiveDataToSave:: INACTIVE " + obj.id + " " + obj.name);
            objectives.Add(objectiveData);
        }

    }
}
