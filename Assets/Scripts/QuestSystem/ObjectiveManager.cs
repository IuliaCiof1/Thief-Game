using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> objectives;
    public List<Objective> activeobjectives;

    public List<MemberObjectives> familyMembers;
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

    //public static ObjectiveManager Instance;

  

    private void Awake()
    {
        //LoadData();
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
   
        foreach (MemberObjectives familyMember in familyMembers)
        {
           
            if(familyMember.possibleObjectives is null)
            {
                print("possible objectives is null");
            }
            //foreach (Objective objective in familyMember.possibleObjectives)
            //{
            //    if (objective.isActive)
            //    {
            //        activeobjectives.Add(objective);
            //        print("take health " + objective.healthTaken);
            //        familyMember.TakeHealth(objective.healthTaken);

            //        //DisplayObjective(objective);
            //    }
            //}
            objectives.AddRange(familyMember.possibleObjectives);
            foreach (Objective obj in objectives)
            {
                obj.isActive = false;
            }
            print("objectives added to objective manager");
        }

        
        

    }

    //Make sure that displaying objectives is made after game load
    private void Start()
    {


        foreach (MemberObjectives familyMember in familyMembers)
        {
           
            if (familyMember.possibleObjectives is null)
            {
                print("possible objectives is null");
            }
            foreach (Objective objective in familyMember.possibleObjectives)
            {
                if (objective.isActive)
                {
                    //activeobjectives.Add(objective);
                    print("take health " + objective.healthTaken);
                    familyMember.TakeHealth(objective.healthTaken);

                    //DisplayObjective(objective);
                }
            }

            objectives.AddRange(familyMember.possibleObjectives);
            print("objectives added to objective manager");
        }

        StartNextObjective();

        //foreach (Objective objective in activeobjectives)
        //{
        //    print("display objective "+objective.name);
        //    DisplayObjective(objective);
        //}


    }

    public void HandleObjectiveCompleted(Objective completedObjective)
    {
        completedObjective.DeactivateObjective(); // Clean up
        activeobjectives.Remove(completedObjective);

        foreach (Transform objectiveUI in objectiveContainer)
        {
            description = objectiveUI.transform.GetChild(0).GetComponent<TMP_Text>();
            if (description.text == completedObjective.description)
            {
                //description = objectiveContainer.GetChild(activeobjectives.Count - 1 - activeIndex).transform.GetChild(0).GetComponent<TMP_Text>();
                description.text = "<s>" + description.text;
                //completedObjective.DeactivateObjective(); // Clean up
                                                          //Invoke("DeleteObjectiveFromUIList", 0.5f);
                StartCoroutine(UndisplayObjective(description.transform.parent.gameObject, completedObjective));
                //currentObjectiveIndex++;
                print("completed quest");

                //activeobjectives.Remove(completedObjective);
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

        List<Objective> inactiveObjectives = new List<Objective>();
        foreach (Objective obj in objectives)
            if (!obj.isActive)
                inactiveObjectives.Add(obj);

        if (SceneManager.GetActiveScene().name == "Quarters" && inactiveObjectives.Any())
        {

            int objectiveIndex = Random.Range(0, inactiveObjectives.Count);

            print("start new objective " + objectiveIndex);
            currentObjectiveIndex = objectiveIndex;

            Objective objective = inactiveObjectives[objectiveIndex];

            if (!objective.isActive)
            {
                DisplayObjective(objective);
                print("activate objective");
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

        objective.isActive = true;
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


    //private void OnDisable()
    //{
    //    SaveSystem.SaveObjectives(this);
    //}



    public void LoadData(ObjectiveDataToSave data)
    {
        //ObjectiveDataToSave data = SaveSystem.LoadData<ObjectiveDataToSave>();
        //foreach (Objective obj in objectives)
        //    obj.isActive = false;

        if (data is null)
            print("no data recovered for obecjtives");

        if (data.activeoOjectivesID is null)
            print("no data recovered for obecjtives objectives");



        for (int i = 0; i < data.activeoOjectivesID.Count; i++)
        {
            foreach (Objective obj in objectives)
            {
                if (obj.id == data.activeoOjectivesID[i])
                {
                    DisplayObjective(obj);
                    obj.isActive = true;
                    activeobjectives.Add(obj);
                    print("objective " + obj.name + " " + obj.isActive);
                    break;
                }
            }
        }
    }
}
