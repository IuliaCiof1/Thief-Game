using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    List<AIControl> collidingAgents;
    [SerializeField] Transform agentsContainer;
    [SerializeField] CrowdManager crowdManager;

    private void Start()
    {
        collidingAgents = new List<AIControl>();

        foreach (Transform agent in agentsContainer)
        {
            collidingAgents.Add(agent.GetComponent<AIControl>());

        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.TryGetComponent<AIControl>(out AIControl agent))
    //    {
    //        collidingAgents.Add(agent);
    //        print("deadzoe hit agent");
    //    }

       
    //}
    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent<AIControl>(out AIControl agent))
        //{
        //    collidingAgents.Add(agent);
        //    print("deadzoe hit agent");
        //}

        if (other.TryGetComponent<ThirdPersonController>(out ThirdPersonController player))
        {
            //collidingAgents.Add(transform.parent.GetComponent<AIControl>());

            print("deadzone hit player");
            foreach (AIControl agent in collidingAgents)
            {
                agent.FleeFromPosition(player.transform.position);
                agent.deadzoneStarted = false;
            }

           transform.localScale = Vector3.zero;
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, crowdManager.DetectionRadius);
    }
}
