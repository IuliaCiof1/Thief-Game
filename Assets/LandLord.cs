using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LandLord:AIControl
{
    [SerializeField] GameObject enterLocation;
    [SerializeField] GameObject waitLocation;

    ThirdPersonController player;
    [SerializeField] float lookAtPlayerSpeed = 5f;
    [SerializeField] float lookAtPlayerDistance = 5f;

    bool enteredBuilding;

    [SerializeField] AudioClip[] sounds;
    [Range(0f,1f)]
    [SerializeField] float volume;

    //NavMeshAgent agent;
    //bool isInspecting;
    //Animator animator;
    public List<Objective> objectives;
    private void Awake()
    {
        player = FindObjectOfType<ThirdPersonController>();
        agent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        //agent.SetDestination(enterLocation.transform.position);
    }

    private void Start()
    {
        base.Start();

        objectives = GetComponent<MemberObjectives>().activeObjectives;
        agent.SetDestination(enterLocation.transform.position);
    }

    private void EnterBuilding()
    {
        if (!enteredBuilding)
        {
            enteredBuilding = true;
            print("landlord enters");
            
            isInspecting = false;
            animator.SetBool("Inspect", false);
            agent.transform.position = enterLocation.transform.position;
            agent.SetDestination(waitLocation.transform.position);
            ResetAgentSpeed();

            
        }
    }


    private void Update()
    {
       // base.Update();
        if (objectives.Count>0 && objectives[0].isActive)
        {
            EnterBuilding();
        }

        Walk();
    }

    protected override void Walk()
    {
        if (!isInspecting && agent.remainingDistance < 0.1f && !agent.pathPending)
        {
           if(enteredBuilding)
                SoundFXManager.instance.PlayRandomSoundFXClip(sounds, transform, volume);

            agent.speed = 0;
            isInspecting = true;
            animator.SetBool("Inspect", true);
        }

        //Look towards player
        if (isInspecting && Vector3.Distance(player.transform.position, transform.position) < lookAtPlayerDistance)
        {

            Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            targetRotation.x = 0f; // Ignore vertical difference
            targetRotation.z = 0f;
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtPlayerSpeed * Time.deltaTime);
        }
    }


    public void Leave()
    {
        enteredBuilding = false;
        print("wait");
        
        ResetAgentSpeed();
        agent.SetDestination(enterLocation.transform.position);
        isInspecting = false;
        animator.SetBool("Inspect", false);
        print("stop wait");

        SoundFXManager.instance.PlayRandomSoundFXClip(sounds, transform, volume);
    }
   

}
