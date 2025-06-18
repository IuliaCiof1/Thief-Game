using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberObjectives : MonoBehaviour
{
    public List<Objective> possibleObjectives;
    //[SerializeField] float maxHealth;
    //float Health;
    //[SerializeField] FamilyMemberHealthUI healthUI;

    //[SerializeField] GameObject tombStone;
    ObjectiveManager objManager;
    public List<Objective> activeObjective;

    public bool memberDead { get; private set; }

    private void Start()
    {
        objManager = GetComponentInParent<ObjectiveManager>();

        List<Objective> objectives = objManager.objectives;
        possibleObjectives.Clear();
        foreach (Objective obj in objectives)
        {
            

            if (gameObject.name.ToLower().Contains(obj.member.ToLower()))
            {
                print("cooldown lowered");

                if (!obj.isActive && obj.leftCooldown <= 0)
                {
                    print($"{obj.name} {obj.isActive} {obj.leftCooldown}");
                    possibleObjectives.Add(obj);
                }
                else if (!obj.isActive)
                    obj.leftCooldown--;

                if (obj.isActive)
                    activeObjective.Add(obj);
            }
        }

        int random = UnityEngine.Random.Range(0, 2);
        print("memberobjectives:: start " + gameObject.name);
        if (random == 0)
        {
            print("memberobjectives:: start next objective " + gameObject.name);
            objManager.StartNextObjective(possibleObjectives);
        }
    }

    public void DisableObjectivesOfMember()
    {
     
        memberDead = true;

        objManager = GetComponentInParent<ObjectiveManager>();
        List<Objective> objectives = objManager.objectives;

        foreach (Objective obj in objectives)
        {
            if (gameObject.name.ToLower().Contains(obj.member.ToLower()))
            {
                // obj.DeactivateObjective();
                objManager.HandleObjectiveCompleted(obj);
            }
        }
        // transform.GetChild(0).gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
        //gameObject.SetActive(false);
    }
        //private void Awake()
        //{
        //    healthUI.SetMaxSliderValueUI(maxHealth);

        //    //get's the last saved health for this family member. If no health is saved, return maxHealth
        //    Health = PlayerPrefs.GetFloat(gameObject.name, maxHealth);
        //    TakeHealth(0);
        //}

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
        //        foreach (Transform child in transform.parent)
        //        {

        //            child.gameObject.SetActive(false);
        //        }

        //        tombStone.SetActive(true);

        //        if(gameObject.TryGetComponent<LandLord>(out LandLord landLord))
        //        {
        //            EndingManager.Trigger(EndingManager.EndingType.rentDue);
        //        }
        //    }
        //    else
        //    {

        //        Health -= amount;

        //        healthUI.SetSliderValueUI(Health);
        //    }

        //}


        //private void OnDisable()
        //{
        //    PlayerPrefs.SetFloat(gameObject.name, Health);
        //}

    }
