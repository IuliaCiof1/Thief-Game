using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FamilyManager : MonoBehaviour
{
    [SerializeField] List<MemberObjectives> familyMembers;



    public void LoadData()
    {
        ObjectiveManager objectiveManager = GetComponentInParent<ObjectiveManager>();
        int deadMembers = 0;

        foreach (Objective activeObj in objectiveManager.activeObjectives)
        {
            foreach (MemberObjectives familyMember in familyMembers)
            {
                if (activeObj.member.Contains(familyMember.name, StringComparison.OrdinalIgnoreCase))
                {
                    familyMember.activeObjectives.Add(activeObj);
                }
            }
        }


        foreach (MemberObjectives familyMember in familyMembers)
        {
           
                print(familyMember.gameObject.name);

                familyMember.GetComponent<Health>().LoadHealth();
                //familyMember.InitializeMemberObjectives();

                if (familyMember.IsDead)
                {
                    print("dead members " + deadMembers + familyMember.gameObject.name);
                    //familyMember.OnMemberDeath();
                    deadMembers++;
                }
            
        }
        print("dead members " + deadMembers);

        CheckEnding(deadMembers);
    }


    public void AddActiveObjectiveToMember(Objective activeObj)
    {
        foreach (MemberObjectives familyMember in familyMembers)
        {
            if (activeObj.member.Contains(familyMember.name, StringComparison.OrdinalIgnoreCase))
            {
                familyMember.activeObjectives.Add(activeObj);
            }
        }
    }

    private void CheckEnding(int deadMembers)
    {
        if (deadMembers == 3)
            EndingManager.Trigger(EndingManager.EndingType.allDead);

        if (PlayerStats.Instance.reputation >= PlayerStats.maxReputation_) {
            if (deadMembers > 0)
                EndingManager.Trigger(EndingManager.EndingType.reputationWithDeaths);
            else
                EndingManager.Trigger(EndingManager.EndingType.bestEnding);

        }
    }
 
}
