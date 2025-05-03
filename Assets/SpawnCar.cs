using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] float delay;
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject lane;

    int currentAmount;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 0, delay);
    }

   void Spawn()
    {
        if (currentAmount >= amount)
        {
            print("canceeeeeeeeeeeeeeel");
            CancelInvoke();
        }
        else
        {
            currentAmount++;
            GameObject car = Instantiate(prefab, transform);
            car.GetComponent<VehicleAI>().lane = lane;
          
            car.transform.localPosition = Vector3.zero;
            car.transform.SetParent(transform.root);
            car.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
