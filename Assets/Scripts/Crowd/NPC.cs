using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : AIControl
{
    CrowdManager crowdManager_;
    DeadZone deadzone;
    //// Start is called before the first frame update
    void Start()
    {
        base.Start();
        deadzone = FindObjectOfType<DeadZone>();

        //crowdManager_ = FindAnyObjectByType<CrowdManager>();
        //if (crowdManager_ is null)
        //{
        //    print("crowd manager _ is null");
        //}
        //crowdManager = crowdManager_;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        isInspecting = GetIsInspecting();
      
        if (deadzoneStarted)
        {
            
            if (deadzone.transform.localScale.x >= 8)
            {
                print("deadzone reached max size");
                deadzoneStarted = false;
                deadzone.transform.localScale = Vector3.zero;
            }
            else
            {
                print("deadzon change local scale");
                deadzone.transform.localScale += Vector3.one * Time.deltaTime * crowdManager.GrowingSpeed;
            }
        }
    }


    public void StartDeadzoe()
    {
        //crowdManager.DeadzoneObject.SetActive(true);
        if (crowdManager is null)
        {
            print("crowd manager is null");

        }

        if (deadzone is null)
        {
            print("deadzone is ull is null");

        }

        deadzone.transform.SetParent(transform);
        deadzone.gameObject.SetActive(true);
        deadzone.transform.localPosition = Vector3.zero;

        deadzoneStarted = true;
        print("deadzone started");
    }




    public void FleeFromPosition(Vector3 position)
    {
        // print("crowd manager "+crowdManager);
        if (crowdManager is null)
        {
            print("crowd manager is null");

            return;
        }

        if (Vector3.Distance(position, transform.position) <= crowdManager.DetectionRadius)
        {
            //print(gameObject.name + " i detectio radius");
            agent.ResetPath();

            Vector3 fleeDirection = (transform.position - position).normalized;
            Vector3 newGoal = transform.position + fleeDirection * crowdManager.FleeRadius;

            // Ensure the goal is on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newGoal, out hit, 2f, NavMesh.AllAreas))
            {
                newGoal = hit.position;
            }
            else
            {
                Debug.LogWarning(gameObject.name + " couldn't find a valid flee position!");
                return;
            }


            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid) //checks if the goal is on navmesh surface
            {

                //print(gameObject.name + " fleeeeeeeeeee");
                CancelInvoke("InspectionPointSearch");

                StopAllCoroutines();
                inspectionPoint = null;
                isInspecting = false;
                animator.SetBool("Inspect", false);

                agent.SetDestination(path.corners[path.corners.Length - 1]);

                //animator.SetTrigger("Walk");
                agent.speed = 3.4f;
                agent.angularSpeed = 500;
            }
        }
        
    }
}
