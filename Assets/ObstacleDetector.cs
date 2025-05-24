using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour
{
    VehicleAI vehicleAI;

    private void Start()
    {
        vehicleAI = GetComponentInParent<VehicleAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<NPC>(out NPC npc) || other.TryGetComponent<ThirdPersonController>(out ThirdPersonController player))
        {
            vehicleAI.obstacleDetected = true;
        }
    }
}
