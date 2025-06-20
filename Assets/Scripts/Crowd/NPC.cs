using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : AIControl
{
    CrowdManager crowdManager_;
    DeadZone deadzone;

     AudioClip[] audioClips;

    ThirdPersonController player;

    //bool onRoad;

    //// Start is called before the first frame update
    private void Start()
    {
        base.Start();

        player = FindFirstObjectByType<ThirdPersonController>();
        deadzone = FindObjectOfType<DeadZone>();
        gameObject.layer = LayerMask.NameToLayer("Default");
        audioClips = crowdManager.AudioClips;
        //crowdManager_ = FindAnyObjectByType<CrowdManager>();
        //if (crowdManager_ is null)
        //{
        //    print("crowd manager _ is null");
        //}
        //crowdManager = crowdManager_;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print("collision");
    //    if (other.gameObject.CompareTag("Road"))
    //    {
    //        onRoad = true;
    //        print("collision with road" + gameObject.name);
    //        gameObject.layer = LayerMask.NameToLayer("AvoidedByVehicles");
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Road"))
    //    {
    //        onRoad = false;
    //        gameObject.layer = LayerMask.NameToLayer("Default");
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{

    //    if (collision.gameObject.CompareTag("Road"))
    //    {
    //        print("collsision with road");
    //        gameObject.layer = LayerMask.NameToLayer("AvoidedByVehicles");
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Road"))
    //    {
    //        gameObject.layer = LayerMask.NameToLayer("Default");
    //    }
    //}



    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (deadzoneStarted)
        {
            if (deadzone.transform.localScale.x >= 8)
            {
                deadzoneStarted = false;
                deadzone.transform.localScale = Vector3.zero;
            }
            else
            {
                deadzone.transform.localScale += Vector3.one * Time.deltaTime * crowdManager.GrowingSpeed;
            }
        }
    }


    public void StartDeadzoe()
    {
        deadzone.transform.SetParent(transform);
        deadzone.gameObject.SetActive(true);
        deadzone.transform.localPosition = Vector3.zero;

        deadzoneStarted = true;
    }




    public void FleeFromPosition(Vector3 position)
    {
        if (crowdManager is null)
        {
            print(gameObject.name+" crowd manager is null"); return;
        }
        SoundFXManager.instance.PlayRandomSoundFXClip(audioClips, transform, 0.02f);

        //NPC should run when player is not in tram
        if (!player.inTram && Vector3.Distance(position, transform.position) <= crowdManager.DetectionRadius)
        {
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
