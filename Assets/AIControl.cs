using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        animator = transform.GetChild(0).GetComponent<Animator>();

        //Change walk offset parameter so the agents' steps varies
        animator.SetFloat("walkOffset", Random.Range(0.0f, 2.0f));

        //Change the speed of the walking
        float speedMult = Random.Range(0.5f, 2.0f);
        animator.SetFloat("speedMultiplier", speedMult);
        agent.speed *= speedMult;

        goalLocations = GameObject.FindGameObjectsWithTag("goal");

        int i = Random.Range(0, goalLocations.Length);

        agent.SetDestination(goalLocations[i].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < 0.7)
        {
            int i = Random.Range(0, goalLocations.Length);

            agent.SetDestination(goalLocations[i].transform.position);
        }
    }
}
