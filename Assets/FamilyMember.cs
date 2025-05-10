using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FamilyMember : AIControl
{
    //public Objective currentObjective;

    //public List<Objective> possibleObjectives;
    //[SerializeField] float maxHealth;
    //float Health;
    

    //[SerializeField]FamilyMemberHealthUI healthUI;

    //[SerializeField] GameObject tombStone;
    ThirdPersonController player;
    [SerializeField] float lookAtPlayerSpeed = 5f;
    [SerializeField] float lookAtPlayerDistance = 5f;
    

    private void Awake()
    {
        //healthUI = gameObject.GetComponentInChildren<FamilyMemberHealthUI>();
        //healthUI.SetMaxSliderValueUI(maxHealth);

        ////get's the last saved health for this family member. If no health is saved, return maxHealth
        //Health = PlayerPrefs.GetFloat(gameObject.name, maxHealth);
        //TakeHealth(0);

        player = FindObjectOfType<ThirdPersonController>();
        agent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        //Health = maxHealth;
    }

    protected override void Walk()
    {
       
        if (!isInspecting && agent.remainingDistance < 0.7f && !agent.pathPending)
        {
            agent.speed = 0;
            isInspecting = true;
            animator.SetBool("Inspect", true);
            //ResetAgentSpeed();
            StartCoroutine(Wait());
        }
       // print(Vector3.Distance(player.transform.position, transform.position));

        if (isInspecting && Vector3.Distance(player.transform.position, transform.position) < lookAtPlayerDistance)
        {
            
            Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            targetRotation.x = 0f; // Ignore vertical difference
            targetRotation.z = 0f;
            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookAtPlayerSpeed * Time.deltaTime);
        }
    }


    IEnumerator Wait()
    {
      
        yield return new WaitForSeconds(7);
        ResetAgentSpeed();
        SetNewDestination();
        isInspecting = false;
        animator.SetBool("Inspect", false);
      
    }

    //public void GiveHealth(float amount)
    //{
    //    Health += amount;
    //    if (Health > maxHealth)
    //        Health = maxHealth;
        
    //    healthUI.SetSliderValueUI(Health);
    //}

    //public void TakeHealth(float amount)
    //{
    //    //if the family member dies, make a tombstone appear
    //    if (Health <= 0)
    //    {
    //        foreach(Transform child in transform.parent)
    //        {
               
    //              child.gameObject.SetActive(false);
    //        }

    //        tombStone.SetActive(true);
    //    }
    //    else
    //    {

    //        Health -= amount;

    //        print("health " + Health);
    //        healthUI.SetSliderValueUI(Health);
    //    }

    //}

    //private void OnDisable()
    //{
    //    PlayerPrefs.SetFloat(gameObject.name, Health);
    //}
}
