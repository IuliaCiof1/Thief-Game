using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public GameObject[] goalLocations; // Walking destinations
    public List<GameObject> inspectionPoints; // Inspection locations (e.g., windows)
    NavMeshAgent agent;
    Animator animator;
    bool isInspecting = false;

    GameObject inspectionPoint;

    [SerializeField] int inspectionCooldown;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        //Change walk offset parameter so the agents' steps varies
            animator.SetFloat("walkOffset", Random.Range(0.0f, 2.0f));

        //Change the speed of the walking
        float speedMult = Random.Range(0.5f, 2.0f);
        animator.SetFloat("speedMultiplier", speedMult);
        agent.speed *= speedMult;

        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        inspectionPoints.AddRange(GameObject.FindGameObjectsWithTag("inspection"));

        SetNewDestination();

        InvokeRepeating("InspectionPointSearch", 2.0f, inspectionCooldown);
    }

    void Update()
    {


        //transform.Rotate(0, transform.rotation.y, 0);
        if (inspectionPoint != null)
        {

            agent.SetDestination(inspectionPoint.transform.position);

            if (agent.remainingDistance < 1.5f)
            {

                //transform.LookAt(new Vector3(inspectionPoint.transform.position.x, transform.position.y, inspectionPoint.transform.position.z));
                //Vector3 lookDirection = new Vector3(inspectionPoint.transform.position.x, transform.position.y, inspectionPoint.transform.position.z);
                //transform.LookAt(lookDirection);

                // Make NPC look in the same direction as the inspection point's Z-axis
                transform.rotation = Quaternion.LookRotation(inspectionPoint.transform.forward, Vector3.up);

                //transform.LookAt(inspectionPoint.transform);
                //transform.Rotate(0, transform.rotation.y, transform.rotation.z);
                print("start coroutin");

                inspectionPoints.Remove(inspectionPoint);
                inspectionPoint = null;
                StartCoroutine(InspectEnvironment());
            }
        }
    


        if (!isInspecting && agent.remainingDistance < 0.7f && !agent.pathPending)
        {
          
            SetNewDestination();
            //StartCoroutine(InspectEnvironment());
        }
    }


    void InspectionPointSearch()
    {
        inspectionPoint = GetClosestInspectionPoint();
        if (inspectionPoint != null)
        {
            int stoppingChance = inspectionPoint.GetComponent<InspectionPoint>().StoppingChange;

            int value = Random.Range(0, stoppingChance);

            print(value + " " + gameObject.name);

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

        // Pick a random inspection point near the destination


        CancelInvoke("InspectionPointSearch");

        animator.SetTrigger("Inspect"); // Play inspection animation
        yield return new WaitForSeconds(Random.Range(5, 10)); // Inspect for 2-5 seconds

        agent.isStopped = false; // Resume movement
       
        SetNewDestination();
        isInspecting = false;
        
        
        inspectionPoint = null;

        InvokeRepeating("InspectionPointSearch", 2.0f, inspectionCooldown);
    }

    void SetNewDestination()
    {
        int i = Random.Range(0, goalLocations.Length);
        agent.SetDestination(goalLocations[i].transform.position);
    }

    GameObject GetClosestInspectionPoint()
    {
        float closestDistance = 15f;
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
