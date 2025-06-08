using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObjectPlacement : MonoBehaviour
{
    BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Furniture>(out Furniture furniture)
            || other.TryGetComponent<Wall>(out Wall wall))
        {
            
            buildingManager.CanPlace = false;
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Furniture>(out Furniture furniture)
            || other.TryGetComponent<Wall>(out Wall wall))
        {
         
            buildingManager.CanPlace = true;
           
        }
    }
}
