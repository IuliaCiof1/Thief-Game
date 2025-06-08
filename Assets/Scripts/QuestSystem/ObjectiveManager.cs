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
     int activeObjectivesAtOnce;

   // public List<MemberObjectives> familyMembers;
    //public ObjectiveUI objectiveUI;
    private int currentObjectiveIndex;


    [Header(header: "Objectives UI")]
    [SerializeField] GameObject objectivePrefab;
    [SerializeField] Transform objectiveContainer;
    TMP_Text description;

    string currentObjectiveKey = "currentObjective";

    

    //Make sure that displaying objectives is made after game load
    private void Start()
    {

        activeObjectivesAtOnce = 0;
        //foreach (MemberObjectives familyMember in familyMembers)
        //{
           
        

        //    objectives.AddRange(familyMember.possibleObjectives);
        //    print("objectives added to objective manager");
        //}

        //StartNextObjective();

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
        //<S> WITH </S> DOES NOT WORK DINAMICALLY. IT DOES NOT UPDATE THE UI TEXT 
        //string initialText = description.text; //we need this because <s> modifies the length of the string at run time
        //string textBeforeIndex = "";
        //for (int i = 0; i < initialText.Length; i++)
        //{

        //    //string textBeforeIndex = initialText.Substring(0, i);
        //    textBeforeIndex += initialText[i];
        //    string textAfterIndex = initialText.Substring(i);

        //    description.text = "<s>" + textBeforeIndex + "</s>" + textAfterIndex;
        //    print(description.text);

        //    yield return new WaitForSeconds(0.5f);
        //}

       
        yield return new WaitForSeconds(2.5f);
        listedObjective.SetActive(false);
        print(currentObjectiveIndex);
        completedObjective.isCompleted = false;

    }



    public void StartNextObjective(List<Objective> inactiveObjectives)
    {

        //List<Objective> inactiveObjectives = new List<Objective>();
        //foreach (Objective obj in memberObjectives)
        //    if (!obj.isActive)
        //        inactiveObjectives.Add(obj);

        if (SceneManager.GetActiveScene().name == "Quarters" && inactiveObjectives.Any() && activeObjectivesAtOnce<2)
        {
            print("active objectives at once" + activeObjectivesAtOnce);
            int objectiveIndex = Random.Range(0, inactiveObjectives.Count);

         
            currentObjectiveIndex = objectiveIndex;

            Objective objective = inactiveObjectives[objectiveIndex];

           
                DisplayObjective(objective);
             
                activeobjectives.Add(objective);
                objective.ActivateObjective();

           
        }
        

    }


    private void DisplayObjective(Objective objective)
    {
        print("display obj " + objective.title + " " + activeObjectivesAtOnce);
        GameObject objectiveUI =  Instantiate(objectivePrefab, objectiveContainer);
        objectiveUI.transform.SetAsFirstSibling();


        description =  objectiveUI.transform.GetChild(0).GetComponent<TMP_Text>();
        description.text = objective.description;
        //

        TMP_Text title = objectiveUI.transform.GetChild(1).GetComponent<TMP_Text>();
        title.text = objective.title;

        objective.isActive = true;
        activeObjectivesAtOnce++;
    }

  

    public void LoadData(ObjectiveDataToSave data)
    {
    

        foreach (Objective obj in objectives)
        {
            obj.isActive = false;
            obj.leftCooldown = 0;
        }


        if (data is null)
        {
            print("no data recovered for obecjtives");
            return;
        }

        //if (data.activeoOjectivesID is null)
        //    print("no data recovered for obecjtives objectives");

        Debug.Log("ObjectiveManager:: Loading data objectives");

        for (int i = 0; i < data.objectives.Count; i++)
        {
            Debug.Log("ObjectiveManager:: active objective: "+data.objectives[i]);
            foreach (Objective obj in objectives)
            {
                
                if (obj.id == data.objectives[i].id)
                {
                    Debug.Log("ObjectiveManager:: objective: " + obj.name +" "+ data.objectives[i].leftCooldown);
                    DisplayObjective(obj);
                    obj.isActive = data.objectives[i].isActive;
                    obj.leftCooldown = data.objectives[i].leftCooldown;

                    if(obj.isActive)
                        activeobjectives.Add(obj);

                    print("objective " + obj.name + " " + obj.isActive);
                    break;
                }
            }
        }
    }
}
