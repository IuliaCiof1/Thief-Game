using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour
{
    // public List<Objective> objectives;
    // public List<Objective> activeobjectives;
    //  int activeObjectivesAtOnce;

    //// public List<MemberObjectives> familyMembers;
    // //public ObjectiveUI objectiveUI;
    // private int currentObjectiveIndex;


    // [Header(header: "Objectives UI")]
    // [SerializeField] GameObject objectivePrefab;
    // [SerializeField] Transform objectiveContainer;
    // TMP_Text description;

    // string currentObjectiveKey = "currentObjective";



    // //Make sure that displaying objectives is made after game load
    // //private void Start()
    // //{

    // //    activeObjectivesAtOnce = 0;
    // //    //foreach (MemberObjectives familyMember in familyMembers)
    // //    //{



    // //    //    objectives.AddRange(familyMember.possibleObjectives);
    // //    //    print("objectives added to objective manager");
    // //    //}

    // //    //StartNextObjective();

    // //    //foreach (Objective objective in activeobjectives)
    // //    //{
    // //    //    print("display objective "+objective.name);
    // //    //    DisplayObjective(objective);
    // //    //}


    // //}
    // void OnEnable()
    // {
    //     activeObjectivesAtOnce = 0;
    //     activeobjectives.Clear();
    // }


    //public void HandleObjectiveCompleted(Objective completedObjective)
    //{
    //    completedObjective.DeactivateObjective(); // Clean up
    //    activeobjectives.Remove(completedObjective);

    //    foreach (Transform objectiveUI in objectiveContainer)
    //    {
    //        description = objectiveUI.transform.GetChild(0).GetComponent<TMP_Text>();
    //        if (description.text == completedObjective.description)
    //        {
    //            //description = objectiveContainer.GetChild(activeobjectives.Count - 1 - activeIndex).transform.GetChild(0).GetComponent<TMP_Text>();
    //            description.text = "<s>" + description.text;
    //            //completedObjective.DeactivateObjective(); // Clean up
    //            //Invoke("DeleteObjectiveFromUIList", 0.5f);
    //            StartCoroutine(UndisplayObjective(description.transform.parent.gameObject, completedObjective));
    //            //currentObjectiveIndex++;
    //            print("completed quest");

    //            //activeobjectives.Remove(completedObjective);
    //        }
    //    }
    //    //objective.Complete();
    //    //StartNextObjective();
    //}


    //IEnumerator UndisplayObjective(GameObject listedObjective, Objective completedObjective)
    //{
    //    //<S> WITH </S> DOES NOT WORK DINAMICALLY. IT DOES NOT UPDATE THE UI TEXT 
    //    //string initialText = description.text; //we need this because <s> modifies the length of the string at run time
    //    //string textBeforeIndex = "";
    //    //for (int i = 0; i < initialText.Length; i++)
    //    //{

    //    //    //string textBeforeIndex = initialText.Substring(0, i);
    //    //    textBeforeIndex += initialText[i];
    //    //    string textAfterIndex = initialText.Substring(i);

    //    //    description.text = "<s>" + textBeforeIndex + "</s>" + textAfterIndex;
    //    //    print(description.text);

    //    //    yield return new WaitForSeconds(0.5f);
    //    //}


    //    yield return new WaitForSeconds(2.5f);
    //    listedObjective.SetActive(false);
    //    print(currentObjectiveIndex);
    //    completedObjective.isCompleted = false;

    //}



    // public void StartNextObjective(List<Objective> inactiveObjectives)
    // {

    //     if (SceneManager.GetActiveScene().name == "Quarters" && inactiveObjectives.Any() && activeObjectivesAtOnce<2)
    //     {
    //         print("active objectives at once" + activeObjectivesAtOnce);
    //         int objectiveIndex = Random.Range(0, inactiveObjectives.Count);


    //         currentObjectiveIndex = objectiveIndex;

    //         Objective objective = inactiveObjectives[objectiveIndex];


    //             DisplayObjective(objective);

    //             activeobjectives.Add(objective);
    //             objective.ActivateObjective();


    //     }


    // }


    // private void DisplayObjective(Objective objective)
    // {
    //     print("display obj " + objective.title + " " + activeObjectivesAtOnce);
    //     GameObject objectiveUI =  Instantiate(objectivePrefab, objectiveContainer);
    //     objectiveUI.transform.SetAsFirstSibling();


    //     description =  objectiveUI.transform.GetChild(0).GetComponent<TMP_Text>();
    //     description.text = objective.description;
    //     //

    //     TMP_Text title = objectiveUI.transform.GetChild(1).GetComponent<TMP_Text>();
    //     title.text = objective.title;

    //     objective.isActive = true;
    //     activeObjectivesAtOnce++;
    // }



    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private Transform objectiveContainer;
    [SerializeField] private int maxActiveObjectives = 2;
    [SerializeField] FamilyManager familyManager;

    public List<Objective> allObjectives;
    public List<Objective> activeObjectives = new List<Objective>();
    

    //private void OnEnable()
    //{
    //    activeObjectives.Clear();
    //}

    public void ActivateRandomObjective(List<Objective> pool)
    {
        if (activeObjectives.Count >= maxActiveObjectives || pool.Count == 0) return;

        var obj = pool[Random.Range(0, pool.Count)];
        obj.Activate();
        activeObjectives.Add(obj);
        familyManager.AddActiveObjectiveToMember(obj);
        Debug.Log("ObjectiveManager:: random objctive activated" + obj.name);
        DisplayObjectiveUI(obj);
    }

    private void DisplayObjectiveUI(Objective obj)
    {
        var go = Instantiate(objectivePrefab, objectiveContainer);
        var texts = go.GetComponentsInChildren<TMP_Text>();

        texts[0].text = obj.description;
        texts[1].text = obj.title;
        Debug.Log("ObjectiveManager:: display objctive activated " + obj.name);
    }

    public void CompleteObjective(Objective obj)
    {
        print("complete objectives " + obj.name);
        if (!activeObjectives.Remove(obj)) return;
        obj.Complete();

        foreach (Transform child in objectiveContainer)
        {
            //var txt = child.GetComponentInChildren<TMP_Text>();
            TMP_Text txt = child.transform.GetChild(0).GetComponent<TMP_Text>();
            print("complete objective " + txt.gameObject.name);
            if (txt.text == obj.description)
            {
                print("hide objective");
                //StartCoroutine(StrikethroughThenHide(child.gameObject));
                StartCoroutine(StrikethroughThenHide(txt));
                break;
            }
        }
    }

    private IEnumerator StrikethroughThenHide(TMP_Text text_)
    {
        //var txt = go.GetComponentInChildren<TMP_Text>(); 
        
        text_.text = $"<s>{text_.text}</s>";
        yield return new WaitForSeconds(2.5f);
        text_.transform.parent.gameObject.SetActive(false);
    }


    public void LoadData(ObjectiveDataToSave data)
    {
        activeObjectives.Clear();

        foreach (Objective obj in allObjectives)
        {
            obj.Reset();
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
            foreach (Objective obj in allObjectives)
            {
                
                if (obj.id == data.objectives[i].id)
                {
                    //Debug.Log("ObjectiveManager:: objective: " + obj.name +" "+ data.objectives[i].leftCooldown);
                    
                    obj.isActive = data.objectives[i].isActive;
                    obj.leftCooldown = data.objectives[i].leftCooldown;

                    if (obj.isActive)
                    {
                        Debug.Log("ObjectiveManager:: objective: " + obj.name + " " + obj.id);
                        activeObjectives.Add(obj);
                        DisplayObjectiveUI(obj);
                    }

                    print("objective " + obj.name + " " + obj.isActive);
                    break;
                }
            }
        }
    }
}
