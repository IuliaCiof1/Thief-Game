using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
   
    public NavMeshAgent agent;
    protected Animator animator;
    public bool isInspecting;/*{ get; protected set; }*/

protected GameObject inspectionPoint;


    public CrowdManager crowdManager;
    public List<GameObject> goalLocations = null; // Walking destinations
    [SerializeField] List<GameObject> inspectionPoints = null; // Inspection locations (e.g., windows)
    [SerializeField] List<GameObject> initialInspectionPoints = null;
    Vector2 walkOffsetRange;
    Vector2 speedMultiplier;


    public List<PocketItemSO> ItemsInPocket;
    //public List<Sprite> ItemsInPocket { get; set; }

    public bool deadzoneStarted { get; set; }

    float defaultSpeed;

    protected GameObject player;

    public bool GetIsInspecting()
    {
        
        return isInspecting;
    }

    protected void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();

        defaultSpeed = agent.speed;
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



        ////Change walk offset parameter so the agents' steps varies
        //animator.SetFloat("walkOffset", Random.Range(walkOffsetRange.x, walkOffsetRange.y));

        ////Change the speed of the walking
        //float randomSpeedMult = Random.Range(speedMultiplier.x, speedMultiplier.y);
        //animator.SetFloat("speedMultiplier", randomSpeedMult);
        //agent.speed *= randomSpeedMult;

        ResetAgentSpeed();
        

        SetNewDestination();

        InvokeRepeating("InspectionPointSearch", 2.0f, Random.Range(crowdManager.InspectionCooldown, crowdManager.InspectionCooldown+2));
    }

    protected void ResetAgentSpeed()
    {
        agent.speed = defaultSpeed;

        //Change walk offset parameter so the agents' steps varies
        animator.SetFloat("walkOffset", Random.Range(walkOffsetRange.x, walkOffsetRange.y));

        //Change the speed of the walking
        float randomSpeedMult = Random.Range(speedMultiplier.x, speedMultiplier.y);
        animator.SetFloat("speedMultiplier", randomSpeedMult);
        agent.speed *= randomSpeedMult;
        InvokeRepeating("InspectionPointSearch", 2.0f, Random.Range(crowdManager.InspectionCooldown, crowdManager.InspectionCooldown + 2));

    }


    protected void Update()
    {
        

        if (inspectionPoint != null)
        {
            print("inspection point not null");
            //ResetAgentSpeed();
            agent.SetDestination(inspectionPoint.transform.position);

            if (agent.remainingDistance < 1.5f)
            {
                ResetAgentSpeed();
                // Make NPC look in the same direction as the inspection point's Z-axis
                transform.rotation = Quaternion.LookRotation(inspectionPoint.transform.forward, Vector3.up);

                //Remove the visited Inspection Point from the list
                inspectionPoints.Remove(inspectionPoint);
                inspectionPoint = null;
                //isInspecting = true;
                StartCoroutine(InspectEnvironment());
            }
        }



        Walk();


        //if (deadzoneStarted)
        //{
        //    if (crowdManager.DeadzoneObject.transform.localScale.x >= 8)
        //    {
        //        deadzoneStarted = false;
        //        crowdManager.DeadzoneObject.transform.localScale = Vector3.zero;
        //    }
        //    else
        //        crowdManager.DeadzoneObject.transform.localScale += Vector3.one * Time.deltaTime * crowdManager.GrowingSpeed;
        //}
    }


    protected virtual void Walk()
    {
        
        if (!isInspecting && agent.remainingDistance < 0.7f && !agent.pathPending)
        {

            ResetAgentSpeed();
            SetNewDestination();
        }

    }

    void InspectionPointSearch()
    {
        inspectionPoint = GetClosestInspectionPoint();

        if (inspectionPoint != null)
        {
            int stoppingChance = inspectionPoint.GetComponent<InspectionPoint>().StoppingChange;


            //to do: random.range(0, 100). daca value e mai mic decat stoppingChance, obtin sansa de a se opri
            int value = Random.Range(0, stoppingChance);

            if (value != 0)
            {
                inspectionPoint = null;
            }


        }
    }

    IEnumerator InspectEnvironment()
    {
       
        agent.isStopped = true; // Stop walking
        isInspecting = true;
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

    protected void SetNewDestination()
    {
        //isInspecting = false;
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


    //public void StartDeadzoe()
    //{
    //    //crowdManager.DeadzoneObject.SetActive(true);
    //    crowdManager.DeadzoneObject.transform.SetParent(transform);
    //    crowdManager.DeadzoneObject.transform.localPosition = Vector3.zero;

    //    deadzoneStarted = true;
    //}




    //public void FleeFromPosition(Vector3 position)
    //{

    //    if (Vector3.Distance(position, transform.position) <= crowdManager.DetectionRadius)
    //    {
    //        //print(gameObject.name + " i detectio radius");
    //        agent.ResetPath();

    //        Vector3 fleeDirection = (transform.position - position).normalized;
    //        Vector3 newGoal = transform.position + fleeDirection * crowdManager.FleeRadius;

    //        // Ensure the goal is on NavMesh
    //        NavMeshHit hit;
    //        if (NavMesh.SamplePosition(newGoal, out hit, 2f, NavMesh.AllAreas))
    //        {
    //            newGoal = hit.position;
    //        }
    //        else
    //        {
    //            Debug.LogWarning(gameObject.name + " couldn't find a valid flee position!");
    //            return;
    //        }


    //        NavMeshPath path = new NavMeshPath();
    //        agent.CalculatePath(newGoal, path);

    //        if (path.status != NavMeshPathStatus.PathInvalid) //checks if the goal is on navmesh surface
    //        {
                
    //            //print(gameObject.name + " fleeeeeeeeeee");
    //            CancelInvoke("InspectionPointSearch");

    //            StopAllCoroutines();
    //            inspectionPoint = null;
    //            isInspecting = false;
    //            animator.SetBool("Inspect", false);

    //            agent.SetDestination(path.corners[path.corners.Length - 1]);
                
    //            //animator.SetTrigger("Walk");
    //            agent.speed = 3.4f;
    //            agent.angularSpeed = 500;
    //        }
    //    }
    //}


}
