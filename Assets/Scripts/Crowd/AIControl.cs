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

    //stores last 2 visited goals
    [SerializeField]GameObject[] VisitedGoals;
    GameObject lastGoalVisited;

    bool onRoad;
    [SerializeField] float avoidDistance;
    float avoidTimer = 0f;



    public bool GetIsInspecting()
    {
        
        return isInspecting;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!transform.GetChild(0).TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("No Animator found on first child of crowd agent");
        }
    }

    protected void Start()
    {
        VisitedGoals = new GameObject[2];
        //agent = GetComponent<NavMeshAgent>();

        defaultSpeed = agent.speed;
        //Get values from CrowdManager
        crowdManager = FindAnyObjectByType<CrowdManager>();
        //crowdManager = GetComponentInParent<CrowdManager>();
        if (crowdManager == null)
        {
            Debug.LogError("No CrowdManager found in the scene!");
            return; // Exit the function early to avoid null reference errors
        }


        initialInspectionPoints = new List<GameObject>(crowdManager.InspectionPoints);
        inspectionPoints.AddRange(initialInspectionPoints);

       goalLocations = new List<GameObject>(crowdManager.GoalLocations);
       
        //if (!transform.GetChild(0).TryGetComponent<Animator>(out animator))
        //{
        //    Debug.LogError("No Animator found on first child of crowd agent");
        //}

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
                transform.rotation = Quaternion.LookRotation(inspectionPoint.transform.forward);

                //Remove the visited Inspection Point from the list
                inspectionPoints.Remove(inspectionPoint);
                inspectionPoint = null;
                //isInspecting = true;
                StartCoroutine(InspectEnvironment());
            }
        }

        


        Walk();

        if (onRoad)
            Avoid();

        if (isInspecting)
            animator.SetBool("Inspect", true);
        //AvoidPlayer();
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Road"))
        {
            onRoad = true;
            
            gameObject.layer = LayerMask.NameToLayer("AvoidedByVehicles");
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Road"))
        {
            onRoad = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }


    void Avoid()
    {
        bool goBack = false;

        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 1), transform.forward * avoidDistance, Color.yellow, 1f);
        //Debug.DrawRay(transform.position + new Vector3(0, 1), transform.forward, Color.yellow, avoidDistance);
        if (!goBack && Physics.Raycast(transform.position + new Vector3(0, 1), transform.forward, out hit, avoidDistance))
        {
            //avoid vehicles
            if (hit.transform.TryGetComponent<VehicleAI>(out VehicleAI vehicleAI))
            {
                
                agent.isStopped = true;
                isInspecting = true;
                //
              //if the npc stays too much time in front of a vehicle, go back to the preveous goal, to avoid stuck traffic
                    avoidTimer += Time.deltaTime;
                if (avoidTimer >= 4f)
                {
                    Debug.Log("Inspection took too long. Returning to previous goal.");
                    isInspecting = false;
                    agent.isStopped = false;
                    avoidTimer = 0f;
                    agent.SetDestination(VisitedGoals[1].transform.position);
                    goBack = true;
                }
                   
                

            }
            else
            {
                isInspecting = false;
                agent.isStopped = false;
            }
        }
        else
        {
            isInspecting = false;
            agent.isStopped = false;
        }
    }

    void AvoidPlayer()
    {
        float playerDistance = Vector3.Distance(crowdManager.player.transform.position, this.transform.position);
        if (playerDistance <= crowdManager.AvoidDistance)
        {
            if (!isInspecting)
            {

                //vAvoid += (this.transform.position - crowdManager.player.transform.position).normalized / playerDistance;
                //agentToAvoid = crowdManager.transform;
                //groupSize++;
                //isInspecting = true;
                StartCoroutine(Wait());
            }
        }
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
        agent.isStopped = true;
        animator.SetBool("Inspect", true);
        //agent.enabled = false;
       // GetComponent<NavMeshObstacle>().enabled = true;
        yield return new WaitForSeconds(3);
        isInspecting = false;
        //animator.SetBool("Inspect", false);
        // GetComponent<NavMeshObstacle>().enabled = false;
        //StartCoroutine(Delay());
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
        float playerDistance = Vector3.Distance(crowdManager.player.transform.position, this.transform.position);
        if (playerDistance <= crowdManager.AvoidDistance)
        {
            if (!isInspecting)
            {

                //vAvoid += (this.transform.position - crowdManager.player.transform.position).normalized / playerDistance;
                //agentToAvoid = crowdManager.transform;
                //groupSize++;
               // isInspecting = true;
                StartCoroutine(Wait());
            }
        }

        else if (agent.enabled && !isInspecting && agent.remainingDistance < 0.7f && !agent.pathPending)
        {
            agent.isStopped = false;
            animator.SetBool("Inspect", false);
            // ResetAgentSpeed();
            SetNewDestination();
        }
        else if (agent.pathPending)
        {
            animator.SetBool("Inspect", true); //trigger the inspecting animation with the path of the agent is still calculating
        }
        else if (!isInspecting)
        {
            agent.isStopped = false;
            animator.SetBool("Inspect", false);
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

 

    //Because choosing a random goal destination, would make the npc only walk on the side of the road if the the detination was not in a straight line with the npc,
    //I choose instead to take the random destination of the next 2 closest destination, as the final set next destination of the agent.
    //it also keeps the last 2 visited points to avoid loops between 2 points
    protected void SetNewDestination()
    {
        isInspecting = false;

        float firstMaxDistance = 1000f;
        float secondMaxDistance = 1000f;
     
        GameObject goal = null;
        GameObject[]  possibleGoals = new GameObject [] { goalLocations[0],goalLocations[1] };

        NavMeshPath path = new NavMeshPath();

        NavMeshHit hit;
        foreach (GameObject point in goalLocations)
        {
            
            if (point != VisitedGoals[0] && point != VisitedGoals[1] )
            {
                
               //calculate the distance of the path the agent would take to another points, not just the distance between two points which can be shorter.
                if(NavMesh.CalculatePath(transform.position, point.transform.position, agent.areaMask, path))
                {
                    float distance = Vector3.Distance(transform.position, path.corners[0]);

                    for(int j=1; j<path.corners.Length; j++)
                    {
                        distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                    }

                    if (distance < firstMaxDistance && point)
                    {
                        goal = point;
                        possibleGoals[0] = goal;
                       
                       // print(goal.name + "is first close");
                        firstMaxDistance = distance;
                    }
                    else if (distance < secondMaxDistance && point!= possibleGoals[0]) 
                    {
                        goal = point;
                        possibleGoals[1] = goal;
                        //print(goal.name + "is second close");
                        secondMaxDistance = distance;
                    }

                }

              
            }
          
        }

        if (goal != null)
        {
            int goalIndex = Random.Range(0, 2);
            //keep only the last 2 visited goals
            GameObject temp = VisitedGoals[0];
            VisitedGoals[0] = possibleGoals[goalIndex];
            VisitedGoals[1] = temp;

            Vector3 offset = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f)); //add an offset to spread the walking path
            agent.SetDestination(possibleGoals[goalIndex].transform.position+offset);
           // print("destination set");
        }
      
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
