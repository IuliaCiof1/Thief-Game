using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    public float health;
    [SerializeField] FamilyMemberHealthUI healthUI;

    [SerializeField] GameObject tombStone;

    bool healtLoaded;


    public void LoadHealth()
    {
       
        healthUI.SetMaxSliderValueUI(maxHealth);
        
       
        //get's the last saved health for this family member. If no health is saved, return maxHealth
        health = PlayerPrefs.GetFloat(gameObject.name, maxHealth);
        print("health" + health);
        TakeHealth(0);
        healtLoaded = true;
        //healthUI.SetSliderValueUI(health);
    }

    private void Start()
    {
        //Set health to maxHealth if no data loaded
        if (!healtLoaded)
            health = maxHealth;

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
        print("give health " + amount);
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
            //foreach (Transform child in transform.parent)
            //{

            //    child.gameObject.SetActive(false);
            //}

            
            tombStone.SetActive(true);

            if (gameObject.TryGetComponent<LandLord>(out LandLord landLord))
            {
                print(gameObject.name);
                EndingManager.Trigger(EndingManager.EndingType.rentDue);
            }

            //transform.GetChild(0).gameObject.SetActive(false);

            GetComponent<MemberObjectives>().DisableObjectivesOfMember();
            
        }
        else
        {
            print("take health " + amount);
            health -= amount;

            healthUI.SetSliderValueUI(health);
        }
        print("health" + health);
    }


    private void OnDisable()
    {
        PlayerPrefs.SetFloat(gameObject.name, health);
    }

}
