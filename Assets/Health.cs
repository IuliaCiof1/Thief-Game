using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float health;
    [SerializeField] FamilyMemberHealthUI healthUI;

    [SerializeField] GameObject tombStone;




    private void Awake()
    {
        healthUI.SetMaxSliderValueUI(maxHealth);

        //get's the last saved health for this family member. If no health is saved, return maxHealth
        health = PlayerPrefs.GetFloat(gameObject.name, maxHealth);
        TakeHealth(0);
    }

    private void Start()
    {
        List <Objective> objectives = GetComponent<MemberObjectives>().possibleObjectives;

        foreach (Objective objective in objectives)
        {
            if (objective.isActive)
            {
                //activeobjectives.Add(objective);
                print("take health " + objective.healthTaken);
                TakeHealth(objective.healthTaken);

                //DisplayObjective(objective);
            }
        }
    }

    public void GiveHealth(float amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;

        healthUI.SetSliderValueUI(health);
    }

    public void TakeHealth(float amount)
    {
        //if the family member dies, make a tombstone appear
        if (health <= 0)
        {
            foreach (Transform child in transform.parent)
            {

                child.gameObject.SetActive(false);
            }

            GetComponent<MemberObjectives>().enabled = false;
            tombStone.SetActive(true);

            if (gameObject.TryGetComponent<LandLord>(out LandLord landLord))
            {
                print(gameObject.name);
                EndingManager.Trigger(EndingManager.EndingType.rentDue);
            }
        }
        else
        {

            health -= amount;

            healthUI.SetSliderValueUI(health);
        }

    }


    private void OnDisable()
    {
        PlayerPrefs.SetFloat(gameObject.name, health);
    }

}
