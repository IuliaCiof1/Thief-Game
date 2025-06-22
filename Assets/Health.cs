using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    public float health { get; private set; }

    [SerializeField] private FamilyMemberHealthUI healthUI;
    [SerializeField] private GameObject tombStone;

    private bool healthLoaded;

    private void Start()
    {
        if (!healthLoaded)
        {
            health = maxHealth;
            healthUI.SetMaxSliderValueUI(maxHealth);


            // Handle initial health reduction based on active objectives
            var memberObjectives = GetComponent<MemberObjectives>();
            if (memberObjectives != null)
            {
                foreach (Objective obj in memberObjectives.activeObjectives)
                {
                    if (obj.isActive)
                    {
                        Debug.Log($"Applying health penalty from active objective: {obj.name}, -{obj.healthTaken} HP");
                        TakeHealth(obj.healthTaken);
                    }
                }
            }
        }
    }

    public void LoadHealth()
    {
       
        health = PlayerPrefs.GetFloat(gameObject.name, maxHealth);
        print( health+"health from player prefs of " + gameObject.name);
        healthUI.SetMaxSliderValueUI(maxHealth);
        TakeHealth(0); // Update UI with current value
       
        var memberObjectives = GetComponent<MemberObjectives>();
        memberObjectives.InitializeMemberObjectives();
        if (memberObjectives != null)
        {
            foreach (Objective obj in memberObjectives.activeObjectives)
            {
                if (obj.isActive)
                {
                    Debug.Log($"Applying health penalty from active objective: {obj.name}, -{obj.healthTaken} HP");
                    TakeHealth(obj.healthTaken);
                }
            }
        }

        healthLoaded = true;
    }

    public void GiveHealth(float amount)
    {

        health += amount;
        health = Mathf.Min(health, maxHealth);
        healthUI.SetSliderValueUI(health);

        PlayerPrefs.SetFloat(gameObject.name, health);
    }

    public void TakeHealth(float amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);

        healthUI.SetSliderValueUI(health);
        PlayerPrefs.SetFloat(gameObject.name, health);

        if (health <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has died. Enabling tombstone.");

        if (tombStone != null)
            tombStone.SetActive(true);

        if (TryGetComponent(out LandLord landLord))
        {
            Debug.Log($"{gameObject.name} is a landlord. Triggering rentDue ending.");
            EndingManager.Trigger(EndingManager.EndingType.rentDue);
        }

        if (TryGetComponent(out MemberObjectives memberObjectives))
        {
            memberObjectives.OnMemberDeath();
            transform.parent.gameObject.SetActive(false);
           // memberObjectives.MemberDies();
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(gameObject.name, health);
    }

    //[SerializeField] float maxHealth;
    //public float health;
    //[SerializeField] FamilyMemberHealthUI healthUI;

    //[SerializeField] GameObject tombStone;

    //bool healtLoaded;


    //public void LoadHealth()
    //{

    //    healthUI.SetMaxSliderValueUI(maxHealth);


    //    //get's the last saved health for this family member. If no health is saved, return maxHealth
    //    health = PlayerPrefs.GetFloat(gameObject.name, maxHealth);
    //    print("health" + health);
    //    TakeHealth(0);
    //    healtLoaded = true;
    //    //healthUI.SetSliderValueUI(health);
    //}

    //private void Start()
    //{

    //    //Set health to maxHealth if no data loaded
    //    if (!healtLoaded)
    //    {
    //        healthUI.SetMaxSliderValueUI(maxHealth);
    //        health = maxHealth;
    //    }

    //    List <Objective> objectives = GetComponent<MemberObjectives>().activeObjective;

    //    foreach (Objective objective in objectives)
    //    {
    //        if (objective.isActive)
    //        {
    //            //activeobjectives.Add(objective);
    //            print("take health " + objective.healthTaken);
    //            TakeHealth(objective.healthTaken);

    //            //DisplayObjective(objective);
    //        }
    //    }

    //}

    //public void GiveHealth(float amount)
    //{
    //    print("give health " + amount);
    //    health += amount;
    //    if (health > maxHealth)
    //        health = maxHealth;

    //    healthUI.SetSliderValueUI(health);
    //    PlayerPrefs.SetFloat(gameObject.name, health);
    //}

    //public void TakeHealth(float amount)
    //{

    //    //if the family member dies, make a tombstone appear
    //    if (health <= 0)
    //    {
    //        //foreach (Transform child in transform.parent)
    //        //{

    //        //    child.gameObject.SetActive(false);
    //        //}


    //        tombStone.SetActive(true);

    //        if (gameObject.TryGetComponent<LandLord>(out LandLord landLord))
    //        {
    //            print(gameObject.name);
    //            EndingManager.Trigger(EndingManager.EndingType.rentDue);
    //        }

    //        //transform.GetChild(0).gameObject.SetActive(false);

    //        GetComponent<MemberObjectives>().DisableObjectivesOfMember();

    //    }
    //    else
    //    {
    //        print("take health " + amount);
    //        health -= amount;

    //        healthUI.SetSliderValueUI(health);
    //    }
    //    print("health" + health);
    //}


    //private void OnDisable()
    //{
    //    PlayerPrefs.SetFloat(gameObject.name, health);
    //}

}
