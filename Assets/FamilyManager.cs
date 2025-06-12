using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FamilyManager : MonoBehaviour
{
    [SerializeField] List<MemberObjectives> familyMembers;



    public void LoadData()
    {
        int deadMembers = 0;

        foreach (MemberObjectives familyMember in familyMembers)
        {
            print(familyMember.gameObject.name);

            familyMember.GetComponent<Health>().LoadHealth();

            if (familyMember.memberDead)
            {
                print("dead members " + deadMembers +familyMember.gameObject.name);
                deadMembers++;
            }
        }
        print("dead members " + deadMembers);

        CheckEnding(deadMembers);
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

        //if (SceneManager.GetActiveScene().name == "Quarters")
        //{

        //}
    }
 
}
