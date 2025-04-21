using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayHelper : MonoBehaviour
{
     public static DelayHelper Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //public void ResetObjectives(ObjectiveManager objectiveManager, float delay)
        //{
        //    StartCoroutine(ResetAfterDelay(objectiveManager, delay));
        //}

        //private IEnumerator ResetAfterDelay(ObjectiveManager manager, float seconds)
        //{
        //    yield return new WaitForSeconds(seconds);

        //    foreach (var obj in manager.objectives)
        //    {
        //        obj.isActive = false;
        //    }

        //    Debug.Log("Objectives reset after save.");
        //}
    
}
