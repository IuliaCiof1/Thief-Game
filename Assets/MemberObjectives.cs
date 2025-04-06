using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberObjectives : MonoBehaviour
{
    public List<Objective> possibleObjectives;
    [SerializeField] float maxHealth;
    float Health;
    [SerializeField] FamilyMemberHealthUI healthUI;

    [SerializeField] GameObject tombStone;

    private void Awake()
    {
        healthUI.SetMaxSliderValueUI(maxHealth);

        //get's the last saved health for this family member. If no health is saved, return maxHealth
        Health = PlayerPrefs.GetFloat(gameObject.name, maxHealth);
        TakeHealth(0);
    }

    public void GiveHealth(float amount)
    {
        Health += amount;
        if (Health > maxHealth)
            Health = maxHealth;

        healthUI.SetSliderValueUI(Health);
    }

    public void TakeHealth(float amount)
    {
        //if the family member dies, make a tombstone appear
        if (Health <= 0)
        {
            foreach (Transform child in transform.parent)
            {

                child.gameObject.SetActive(false);
            }

            tombStone.SetActive(true);
        }
        else
        {

            Health -= amount;

            print("health " + Health);
            healthUI.SetSliderValueUI(Health);
        }

    }


    private void OnDisable()
    {
        PlayerPrefs.SetFloat(gameObject.name, Health);
    }

}
