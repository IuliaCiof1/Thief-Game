using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tram : MonoBehaviour
{
    List<NavMeshAgent> agents;
    [SerializeField]  Transform agentsParent;
    NavMeshAgent tramAgent;
    bool alreadyStopped;
    [SerializeField] TramStation tramStation;
    [SerializeField] GameObject tramRoof;

    void Start()
    {
        tramAgent = GetComponent<NavMeshAgent>();

        foreach(Transform npc in agentsParent)
        {
            NavMeshAgent agent;
            if (npc.TryGetComponent<NavMeshAgent>(out agent))
            {
                agent.updatePosition = false;
                NPC aIControl = npc.GetComponent<NPC>();
                aIControl.isInspecting = true; 
            }
        }
    }

    public void SwitchTramRoof()
    {
        tramRoof.SetActive(!tramRoof.activeSelf);
    }

    public TramStation GetTramStation()
    {
        return tramStation;
    }

    private void OnTriggerEnter(Collider other)
    {
   
        if(other.TryGetComponent<TramStation>(out TramStation tramStation_) && !alreadyStopped)
        {
            tramStation = tramStation_;
            alreadyStopped = true;
            tramAgent.isStopped = true;
            Invoke("LeaveStation", 8);
        }
    }

    void LeaveStation()
    {
        tramAgent.isStopped = false;
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.TryGetComponent<TramStation>(out TramStation tramStation_))
        {
            tramStation = null;
            alreadyStopped = false;
        }
    }

}
