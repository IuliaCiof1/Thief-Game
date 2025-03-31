using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> objectives;
    //public ObjectiveUI objectiveUI;
    private int currentObjectiveIndex;


    [Header(header: "Objectives UI")]
    [SerializeField] GameObject objectivePrefab;
    [SerializeField] Transform objectiveContainer;
    TMP_Text description;

    string currentObjectiveKey = "currentObjective";

    private void OnEnable()
    {
        ObjectiveEvents.OnObjectiveCompleted += HandleObjectiveCompleted;
    }

    private void OnDisable()
    {
        ObjectiveEvents.OnObjectiveCompleted -= HandleObjectiveCompleted;
    }

    private void Start()
    {
        
        StartNextObjective();

    }

    private void HandleObjectiveCompleted(Objective completedObjective)
    {
        description.text = "<s>" + description.text;
        completedObjective.DeactivateObjective(); // Clean up
        //Invoke("DeleteObjectiveFromUIList", 0.5f);
        StartCoroutine(UndisplayObjective(description.transform.parent.gameObject));
        //currentObjectiveIndex++;
        print("completed quest");
        
        //StartNextObjective();
    }

    IEnumerator UndisplayObjective(GameObject listedObjective)
    {
        yield return new WaitForSeconds(0.5f);
        listedObjective.SetActive(false);
        print(currentObjectiveIndex);
        objectives[currentObjectiveIndex].isCompleted = false;
        PlayerPrefs.SetInt(currentObjectiveKey, -1);
    }

    private void StartNextObjective()
    {
        //Check if there is a completed objective saved in PlayerPrefs. If it is, start a new random objective
        int lastSavedCurrentObjective = PlayerPrefs.GetInt(currentObjectiveKey,-1);
        print("lastSavedCurrentObjective " + lastSavedCurrentObjective);


       

        if (lastSavedCurrentObjective < 0 && SceneManager.GetActiveScene().buildIndex == 1)
        {

            int objectiveIndex = Random.Range(0, objectives.Count);

            print("start new objective " + objectiveIndex);
            currentObjectiveIndex = objectiveIndex;
            PlayerPrefs.SetInt(currentObjectiveKey, currentObjectiveIndex);
        }
        else 
            currentObjectiveIndex = lastSavedCurrentObjective;

        print(currentObjectiveIndex + " " + lastSavedCurrentObjective);

        if (currentObjectiveIndex > -1 && currentObjectiveIndex < objectives.Count)
        {
           
            Objective nextObjective = objectives[currentObjectiveIndex];

            DisplayObjective(nextObjective);

            nextObjective.ActivateObjective();
            //Debug.Log($"Objective Started: {nextObjective.description}");
        }
        else
        {
            Debug.Log("All objectives completed!");
        }
    }

    private void DisplayObjective(Objective objective)
    {
        GameObject objectiveUI =  Instantiate(objectivePrefab, objectiveContainer);
        objectiveUI.transform.SetAsFirstSibling();


        description =  objectiveUI.transform.GetChild(0).GetComponent<TMP_Text>();
        description.text = objective.description;

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
