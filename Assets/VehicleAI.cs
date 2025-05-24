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

    [SerializeField] float rayOffset;
    [SerializeField] Vector3 distance;

    public bool obstacleDetected { get; set; }

    Collider[] obstacles;

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

            Transform closestObstacle = obstacles[0].transform;
            float distance = Mathf.Infinity;
            foreach(Collider obstacle in obstacles)
            {


                if (!obstacle.transform.IsChildOf(gameObject.transform) && Vector3.Distance(obstacle.transform.position, transform.position) < distance)
                {
                    //print("avoid " + obstacle.gameObject.name);
                    closestObstacle = obstacle.transform;
                }
            }

            float normalizedDistance = (Vector3.Distance(closestObstacle.position, transform.position) - obstacleMinDistance) / (obstacleMaxDistance - obstacleMinDistance);
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
        ////Draw rays
        //Debug.DrawRay(transform.position + rayOffset, transform.TransformDirection(Vector3.forward) * obstacleMaxDistance, Color.yellow);
        //Debug.DrawRay(transform.position + rayOffset, Quaternion.AngleAxis(obstacleDetectionAngle, transform.up) * transform.forward * obstacleMaxDistance, Color.yellow);
        //Debug.DrawRay(transform.position + rayOffset, Quaternion.AngleAxis(-obstacleDetectionAngle, transform.up) * transform.forward * obstacleMaxDistance, Color.yellow);

        //if (Physics.Raycast(transform.position+rayOffset, transform.TransformDirection(Vector3.forward), out hit, obstacleMaxDistance, avoidMask) //forward ray
        //   || Physics.Raycast(transform.position + rayOffset, Quaternion.AngleAxis(obstacleDetectionAngle, transform.up) * transform.forward, out hit, obstacleMaxDistance, avoidMask) //angled rays
        //   || Physics.Raycast(transform.position + rayOffset, Quaternion.AngleAxis(-obstacleDetectionAngle, transform.up) * transform.forward, out hit, obstacleMaxDistance, avoidMask))
        //{
        //    return true;
        //}

        obstacles = Physics.OverlapBox(transform.position + transform.forward * rayOffset, distance, transform.rotation, avoidMask);
        
        //Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawWireCube(Vector3.zero, distance*2);

        if (obstacles.Length > 0)
            return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.matrix = transform.localToWorldMatrix;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + transform.forward * rayOffset, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, distance * 2);
        //Gizmos.DrawWireCube(transform.position + rayOffset, distance*2);
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
