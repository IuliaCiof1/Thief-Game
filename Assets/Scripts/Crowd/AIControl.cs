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

    GameObject[] otherAgents;
    //[SerializeField] Transform agentsContainer;

    //[SerializeField]protected GameObject player;

    float inspectionTime;


    GameObject lastGoalVisited;

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

        inspectionTime = crowdManager.InspectionCooldown;
        lastGoalVisited = goalLocations[0];
        ResetAgentSpeed();
        //NavMesh.CalculatePath(Vector3.zero, Vector3.forward * 5, NavMesh.AllAreas, new NavMeshPath());
       // StartCoroutine(DelayedStart(Random.Range(0f, 1.5f)));
        //SetNewDestination();
        
        //InvokeRepeating("InspectionPointSearch", 2.0f, Random.Range(crowdManager.InspectionCooldown, crowdManager.InspectionCooldown+2));
    }

    IEnumerator DelayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetNewDestination();
    }

    protected void Update()
    {
        inspectionTime -= Time.deltaTime;
       
        if (inspectionTime <= 0)
        {
            
            inspectionTime = crowdManager.InspectionCooldown;
            InspectionPointSearch();
        }
        if (inspectionPoint != null && !isInspecting)
        {
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
        //AvoidOthers();
    }

    void AvoidOthers()
    {
        Transform agentToAvoid=null;
        Vector3 vAvoid = Vector3.zero;
        int groupSize = 0;

        foreach (Transform ag in crowdManager.AgentsContainer)
        {

            if (ag.gameObject != this.gameObject)
            {
                float agDistance = Vector3.Distance(ag.position, this.transform.position);
                if (agDistance <= crowdManager.AvoidDistance)
                {
                    if (!ag.GetComponent<AIControl>().isInspecting)
                    {
                        agentToAvoid = ag;
                        vAvoid += (this.transform.position - ag.position).normalized / agDistance;
                        groupSize++;
                    }
                }
            }
        }

        if (groupSize > 0)
        {
            Vector3 avoidanceOffset = vAvoid / groupSize;
            Vector3 newTarget = agent.destination + avoidanceOffset;

            //crowdManager.StopAgentsFromColliding(this,agentToAvoid );
            Vector3 toOther = agentToAvoid.transform.position - transform.position;
            float dot = Vector3.Dot(transform.forward, toOther.normalized);
            if (this.GetInstanceID() < agentToAvoid.GetInstanceID())

            { //this agent is behind the agent to avoid
                print(gameObject.name);
                isInspecting = true;
                StartCoroutine(Wait());
            }
        }

        //If player is too close to the agent, agent will stop
        float playerDistance = Vector3.Distance(crowdManager.player.transform.position, this.transform.position);
        if (playerDistance <= crowdManager.AvoidDistance)
        {
            if (!isInspecting)
            {
                
                vAvoid += (this.transform.position - crowdManager.player.transform.position).normalized / playerDistance;
                agentToAvoid = crowdManager.transform;
                groupSize++;
                isInspecting = true;
                StartCoroutine(Wait());
            }
        }

    }


    public IEnumerator Wait()
    {
        //agent.speed = 0;
        animator.SetBool("Inspect", true);
        agent.enabled = false;
        GetComponent<NavMeshObstacle>().enabled = true;
        yield return new WaitForSeconds(1);
        GetComponent<NavMeshObstacle>().enabled = false;
        StartCoroutine(Delay());
        //agent.enabled = true;
        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(newTarget, out hit, 1.0f, NavMesh.AllAreas))
        //{
        //    //agent.avoidancePriority = Random.Range(30, 70);
        //    //agent.SetDestination(hit.position);

        //}
    }

    IEnumerator Delay()
    {
        yield return null;
        agent.enabled = true;
        isInspecting = false;
        agent.SetDestination(agent.destination);
        animator.SetBool("Inspect", false);

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
        //InvokeRepeating("InspectionPointSearch", 2.0f, Random.Range(crowdManager.InspectionCooldown, crowdManager.InspectionCooldown + 2));

    }




    protected virtual void Walk()
    {

  
            if (agent.enabled && !isInspecting && agent.remainingDistance < 0.7f && !agent.pathPending)
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
            int value = Random.Range(0, 100);
           //print(gameObject.name+" "+inspectionPoint.name + " " + value);

            if (value > stoppingChance)
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
        //inspectionTime = crowdManager.InspectionCooldown;
        //InvokeRepeating("InspectionPointSearch", 2.0f, crowdManager.InspectionCooldown);
    }

    protected void SetNewDestination()
    {
        isInspecting = false;

        List<GameObject> forwardPoints = new List<GameObject>();
        float maxDistance = 50f;
        float minDotThreshold = 0.2f; // Higher = stricter forward filtering (1 = perfectly ahead)

        foreach (GameObject point in goalLocations)
        {
            //if (point == lastGoalVisited) continue;

            //Vector3 toPoint = (point.transform.position - transform.position).normalized;
            //float dot = Vector3.Dot(transform.forward, toPoint);

            float distance = Vector3.Distance(transform.position, point.transform.position);

            if (/*dot > minDotThreshold*/ point != lastGoalVisited  && distance < maxDistance)
            {
                //print("goalsss"+point.name + " " + Vector3.Distance(transform.position, point.transform.position));
                forwardPoints.Add(point);
            }
        }

        // Fallback if no forward points found
        if (forwardPoints.Count == 0)
        {
            print("no close goals found");
            forwardPoints = new List<GameObject>(goalLocations);
        }

        int i = Random.Range(0, forwardPoints.Count);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(forwardPoints[i].transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            //print(gameObject.name + " " + forwardPoints[i].name + " " + Vector3.Distance(transform.position, forwardPoints[i].transform.position));
            lastGoalVisited = forwardPoints[i];
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)); // Sidewalk spread
            agent.SetDestination(forwardPoints[i].transform.position + offset);
        }

        //isInspecting = false;

        //List<GameObject> closestPoints = new List<GameObject>();
        //float maxDistance = 20;

        //foreach (GameObject point in goalLocations)
        //{
        //    float distance = Vector3.Distance(transform.position, point.transform.position);
        //    if (distance < maxDistance && point != lastGoalVisited)
        //    {
        //        //closestDistance = distance;
        //        //closestPoint = point;
        //        closestPoints.Add(point);
        //    }
        //}
        //int i = Random.Range(0, closestPoints.Count);
        ////print(closestPoint.name + " " + 
        ////    lastGoalVisited.name);

        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(closestPoints[i].transform.position, out hit, 1.0f, NavMesh.AllAreas))
        //{
        //    //agent.avoidancePriority = Random.Range(30, 70);
        //    //agent.SetDestination(hit.position);
        //    lastGoalVisited = closestPoints[i];
        //    Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)); //adds an offest to the goal to avoid congestion on the side of the sidewalk
        //    agent.SetDestination(closestPoints[i].transform.position + offset);
        //}


        //=============
        //int i = Random.Range(0, goalLocations.Count);

        //NavMeshHit hit;
        //if (NavMesh.SamplePosition(goalLocations[i].transform.position, out hit, 1.0f, NavMesh.AllAreas))
        //{
        //    //agent.avoidancePriority = Random.Range(30, 70);
        //    //agent.SetDestination(hit.position);
        //    Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)); //adds an offest to the goal to avoid congestion on the side of the sidewalk
        //    agent.SetDestination(goalLocations[i].transform.position + offset);
        //}

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

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, 10);
    //}

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
