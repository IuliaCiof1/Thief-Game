using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
   
    NavMeshAgent agent;
    Animator animator;
    public bool isInspecting { get; private set; }

    GameObject inspectionPoint;


    CrowdManager crowdManager;
    [SerializeField] List<GameObject> goalLocations = null; // Walking destinations
    [SerializeField] List<GameObject> inspectionPoints = null; // Inspection locations (e.g., windows)
    [SerializeField] List<GameObject> initialInspectionPoints = null;
    Vector2 walkOffsetRange;
    Vector2 speedMultiplier;



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();


        //Get values from CrowdManager
        crowdManager = FindAnyObjectByType<CrowdManager>();
        if (crowdManager == null)
        {
            Debug.LogError("No CrowdManager found in the scene!");
            return; // Exit the function early to avoid null reference errors
        }

        initialInspectionPoints = new List<GameObject>(crowdManager.InspectionPoints);
        inspectionPoints.AddRange(initialInspectionPoints);

        goalLocations = new List<GameObject>(crowdManager.GoalLocations);


        if (!transform.GetChild(0).TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("No Animator found on first child of crowd agent");
        }

        walkOffsetRange = crowdManager.WalkOffsetRange;
        speedMultiplier = crowdManager.SpeedMultiplier;



        //Change walk offset parameter so the agents' steps varies
        animator.SetFloat("walkOffset", Random.Range(walkOffsetRange.x, walkOffsetRange.y));

        //Change the speed of the walking
        float randomSpeedMult = Random.Range(speedMultiplier.x, speedMultiplier.y);
        animator.SetFloat("speedMultiplier", randomSpeedMult);
        agent.speed *= randomSpeedMult;

        

        SetNewDestination();

        InvokeRepeating("InspectionPointSearch", 2.0f, crowdManager.InspectionCooldown);
    }

    void Update()
    {
        

        if (inspectionPoint != null)
        {

            agent.SetDestination(inspectionPoint.transform.position);

            if (agent.remainingDistance < 1.5f)
            { 
                // Make NPC look in the same direction as the inspection point's Z-axis
                transform.rotation = Quaternion.LookRotation(inspectionPoint.transform.forward, Vector3.up);

                //Remove the visited Inspection Point from the list
                inspectionPoints.Remove(inspectionPoint);
                inspectionPoint = null;

                StartCoroutine(InspectEnvironment());
            }
        }
    


        if (!isInspecting && agent.remainingDistance < 0.7f && !agent.pathPending)
        {
          
            SetNewDestination();
        }
    }


    void InspectionPointSearch()
    {
        inspectionPoint = GetClosestInspectionPoint();

        if (inspectionPoint != null)
        {
            int stoppingChance = inspectionPoint.GetComponent<InspectionPoint>().StoppingChange;

            int value = Random.Range(0, stoppingChance);

            if (value != 0)
            {
                inspectionPoint = null;
            }
        }
    }

    IEnumerator InspectEnvironment()
    {
        isInspecting = true;
        agent.isStopped = true; // Stop walking

        CancelInvoke("InspectionPointSearch"); //cancel the invocation of the search while agent is inspecting

        animator.SetBool("Inspect", true); // Play inspection animation
        yield return new WaitForSeconds(Random.Range(crowdManager.TimeRangeAtInspectionPoint.x, crowdManager.TimeRangeAtInspectionPoint.y)); // Inspect the place for a random time
        animator.SetBool("Inspect", false);
        agent.isStopped = false; // Resume movement
       
        SetNewDestination();
        isInspecting = false;
        
        
        inspectionPoint = null;

        if(inspectionPoints.Count == 0)
        {
            inspectionPoints.AddRange(initialInspectionPoints);
        }

        InvokeRepeating("InspectionPointSearch", 2.0f, crowdManager.InspectionCooldown);
    }

    void SetNewDestination()
    {
        int i = Random.Range(0, goalLocations.Count);
        agent.SetDestination(goalLocations[i].transform.position);
    }

    GameObject GetClosestInspectionPoint()
    {
        float closestDistance = crowdManager.MinDistanceInspectionPoint;
        GameObject closestPoint = null;

        foreach (GameObject point in inspectionPoints)
        {
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

}
