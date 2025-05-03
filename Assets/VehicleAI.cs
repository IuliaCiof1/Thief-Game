using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VehicleAI : MonoBehaviour
{
    [SerializeField] public GameObject lane;
    [SerializeField] List<Transform> goals;
    NavMeshAgent agent;

    int currentGoalIndex;
    [SerializeField] LayerMask avoidMask;
    [SerializeField] LayerMask playerMask;
  

    float agentMaxSpeed;
    RaycastHit hit;

    [SerializeField] int obstacleMaxDistance = 10;
    [SerializeField] int obstacleMinDistance = 6;
    [SerializeField] int obstacleDetectionAngle = 15;

    // Start is called before the first frame update
    void Start()
    {
        avoidMask = avoidMask | playerMask; //combine avoid mask with player mask

        foreach (Transform goal in lane.transform)
        {
            goals.Add(goal);
        }
        
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(goals[0].position);

        agentMaxSpeed = agent.speed;
    }

   

    // Update is called once per frame
    void Update()
    {

       
        if (ObstacleDetected())
        {
            float normalizedDistance = (Vector3.Distance(hit.transform.position, transform.position) - obstacleMinDistance) / (obstacleMaxDistance - obstacleMinDistance);
            agent.speed = agentMaxSpeed * normalizedDistance;
        }
        else
            agent.speed = Mathf.Lerp(agent.speed, agentMaxSpeed, Time.deltaTime * 1); //slowly accelerate

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            SetNewDestination();
        }
    }

    bool ObstacleDetected()
    {
        //Draw rays
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * obstacleMaxDistance, Color.yellow);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(obstacleDetectionAngle, transform.up) * transform.forward * obstacleMaxDistance, Color.yellow);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-obstacleDetectionAngle, transform.up) * transform.forward * obstacleMaxDistance, Color.yellow);

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, obstacleMaxDistance, avoidMask) //forward ray
           || Physics.Raycast(transform.position, Quaternion.AngleAxis(obstacleDetectionAngle, transform.up) * transform.forward, out hit, obstacleMaxDistance, avoidMask) //angled rays
           || Physics.Raycast(transform.position, Quaternion.AngleAxis(-obstacleDetectionAngle, transform.up) * transform.forward, out hit, obstacleMaxDistance, avoidMask))
        {
            return true;
        }

        return false;
    }

    void SetNewDestination()
    {
        if (currentGoalIndex == goals.Count - 1)
            currentGoalIndex = 0;
        else
            currentGoalIndex++;

      
        agent.SetDestination(goals[currentGoalIndex].position);
    }

}
