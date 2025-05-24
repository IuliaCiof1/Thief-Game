using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Police : MonoBehaviour
{
    [SerializeField] CrowdManager crowdManager;
    NavMeshAgent agent;
    [SerializeField] float minimumDistanceFromPlayer;
    [SerializeField] ThirdPersonController player;
    [SerializeField] float catchSpeed = 4;

   bool catchPlayer;
    GameObject policeModel;
    [SerializeField] GameObject gotCaughtUI;

    Vector3 initialPolicePosition;

     Animator animator;

    CursorController cursorController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        cursorController = FindFirstObjectByType<CursorController>();

        initialPolicePosition = transform.position;
        ResetPolice();
    }

    void ResetPolice()
    {
        transform.position = initialPolicePosition;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 0;

        policeModel = transform.GetChild(0).gameObject;
        policeModel.SetActive(false);
        agent.enabled = false;
        catchPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //print(agent.remainingDistance);
        if (catchPlayer)
        {
            agent.SetDestination(player.transform.position);

            if (Vector3.Distance(agent.transform.position, player.transform.position) < 1.5f)
            {
                agent.isStopped = true;
                animator.SetBool("Inspect", true);
                //player.enabled = false;
                player.stopMovement = true;
                cursorController.CursorVisibility(true);
                gotCaughtUI.SetActive(true);
            }
        }
    }

    public void CallPolice()
    {
        //Get the closest goal location to the player
        float shortestDistance = Mathf.Infinity;
        float thisDistance;

        animator.SetBool("Inspect", false);

        GameObject closestGoal = null;

        foreach(GameObject goalLocation in crowdManager.GoalLocations)
        {
          
            thisDistance = Vector3.Distance(goalLocation.transform.position, player.transform.position);

            if (thisDistance > minimumDistanceFromPlayer && thisDistance<shortestDistance)
            {
                shortestDistance = thisDistance;
                closestGoal = goalLocation;
            }
        }

        if (closestGoal)
        {
            
            print(closestGoal.name);
            transform.position = closestGoal.transform.position;
            catchPlayer = true;
            //the navmesg agent component needs to be disabled before repoisitioning the agent, otherwise the position won't be affected 
            agent.enabled = true;
            GetComponent<AIControl>().enabled = false;
            agent.speed = catchSpeed;
            policeModel.SetActive(true);
        }
        else
            Debug.LogWarning("Police.cs: CallPolice: Couldn't find closest goal to the player");
    }

    public void PayFines()
    {
        print("fines paid");
        ResetPolice();
        PlayerStats.RemoveMoney(PlayerStats.money);
        PlayerStats.Instance.RemoveReputation();

        gotCaughtUI.SetActive(false);
        //player.enabled = true;
        player.stopMovement = false;
        cursorController.CursorVisibility(false);
    }
}
