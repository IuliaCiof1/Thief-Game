using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> objectives;
    public List<Objective> activeobjectives;

    public List<FamilyMember> familyMembers;
    //public ObjectiveUI objectiveUI;
    private int currentObjectiveIndex;


    [Header(header: "Objectives UI")]
    [SerializeField] GameObject objectivePrefab;
    [SerializeField] Transform objectiveContainer;
    TMP_Text description;

    string currentObjectiveKey = "currentObjective";

    //private void OnEnable()
    //{
    //    ObjectiveEvents.OnObjectiveCompleted += HandleObjectiveCompleted;
    //}

    //private void OnDisable()
    //{
    //    ObjectiveEvents.OnObjectiveCompleted -= HandleObjectiveCompleted;
    //}

    private void Start()
    {
        foreach(FamilyMember familyMember in familyMembers)
        {
            print(familyMember.name);
            if(familyMember.possibleObjectives is null)
            {
                print("possible objectives is null");
            }
            foreach(Objective objective in familyMember.possibleObjectives)
            {
                if (objective.isActive)
                {
                    activeobjectives.Add(objective);
                    print("take health "+ objective.healthTaken);
                   familyMember.TakeHealth(objective.healthTaken);

                    DisplayObjective(objective);
                }
            }
            objectives.AddRange(familyMember.possibleObjectives);
        }

        
        StartNextObjective();

    }

    public void HandleObjectiveCompleted(Objective completedObjective, int activeIndex)
    {

        foreach (Transform objectiveUI in objectiveContainer)
        {
            description = objectiveUI.transform.GetChild(0).GetComponent<TMP_Text>();
            if (description.text == completedObjective.description)
            {
                //description = objectiveContainer.GetChild(activeobjectives.Count - 1 - activeIndex).transform.GetChild(0).GetComponent<TMP_Text>();
                description.text = "<s>" + description.text;
                completedObjective.DeactivateObjective(); // Clean up
                                                          //Invoke("DeleteObjectiveFromUIList", 0.5f);
                StartCoroutine(UndisplayObjective(description.transform.parent.gameObject, completedObjective));
                //currentObjectiveIndex++;
                print("completed quest");

                activeobjectives.Remove(completedObjective);
            }
        }
        //objective.Complete();
        //StartNextObjective();
    }

    IEnumerator UndisplayObjective(GameObject listedObjective, Objective completedObjective)
    {
        yield return new WaitForSeconds(0.5f);
        listedObjective.SetActive(false);
        print(currentObjectiveIndex);
        completedObjective.isCompleted = false;

    }



    private void StartNextObjective()
    {
  
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {

            int objectiveIndex = Random.Range(0, objectives.Count);

            print("start new objective " + objectiveIndex);
            currentObjectiveIndex = objectiveIndex;

            Objective objective = objectives[objectiveIndex];

            if (!objective.isActive)
            {
                DisplayObjective(objective);
                activeobjectives.Add(objective);
                objective.ActivateObjective();
            }
        }
        

    }

    //public List<Objective> GetActiveObjectivesOf(FamilyMember familyMember)
    //{
    //    List<Objective> objectivesOfMember = new List<Objective>();


    //}

    //private void StartNextObjective()
    //{
    //    //Check if there is a completed objective saved in PlayerPrefs. If it is, start a new random objective
    //    int lastSavedCurrentObjective = PlayerPrefs.GetInt(currentObjectiveKey,-1);
    //    print("lastSavedCurrentObjective " + lastSavedCurrentObjective);




    //    if (lastSavedCurrentObjective < 0 && SceneManager.GetActiveScene().buildIndex == 1)
    //    {

    //        int objectiveIndex = Random.Range(0, objectives.Count);

    //        print("start new objective " + objectiveIndex);
    //        currentObjectiveIndex = objectiveIndex;
    //        PlayerPrefs.SetInt(currentObjectiveKey, currentObjectiveIndex);
    //    }
    //    else 
    //        currentObjectiveIndex = lastSavedCurrentObjective;

    //    print(currentObjectiveIndex + " " + lastSavedCurrentObjective);

    //    if (currentObjectiveIndex > -1 && currentObjectiveIndex < objectives.Count)
    //    {

    //        Objective nextObjective = objectives[currentObjectiveIndex];

    //        DisplayObjective(nextObjective);

    //        nextObjective.ActivateObjective();
    //        //Debug.Log($"Objective Started: {nextObjective.description}");
    //    }
    //    else
    //    {
    //        Debug.Log("All objectives completed!");
    //    }
    //}

    private void DisplayObjective(Objective objective)
    {
        GameObject objectiveUI =  Instantiate(objectivePrefab, objectiveContainer);
        objectiveUI.transform.SetAsFirstSibling();


        description =  objectiveUI.transform.GetChild(0).GetComponent<TMP_Text>();
        description.text = objective.description;
        //

        TMP_Text title = objectiveUI.transform.GetChild(1).GetComponent<TMP_Text>();
        title.text = objective.title;
    }

    //void Start()
    //{
    //    objectives[objectiveIndex].ActivateObjective();

    //    //foreach (var objective in objectives)
    //    //{
    //    //    objective.StartObjective();
    //    //    //objectiveUI.DisplayObjective(objective);
    //    //}
    //}

//    public void CompleteObjective(Objective objective)
//    {
//        if (!objective.isCompleted)
//        {
//            objective.DeactivateObjective();
//            objectiveIndex++;

//            //Start the next objective
//            objectives[objectiveIndex].ActivateObjective();
//            //objectiveUI.RemoveObjective(objective);
//        }
//    }
}
