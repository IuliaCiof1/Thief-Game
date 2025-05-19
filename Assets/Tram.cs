using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tram : MonoBehaviour
{
    List<NavMeshAgent> agents;
    NavMeshAgent tramAgent;
    bool alreadyStopped;
    [SerializeField] TramStation tramStation;
    [SerializeField] GameObject tramRoof;


    // Start is called before the first frame update
    void Start()
    {
        tramAgent = GetComponent<NavMeshAgent>();

        foreach(Transform npc in transform)
        {
            NavMeshAgent agent;
            if (npc.TryGetComponent<NavMeshAgent>(out agent))
            {
                //agents.Add(agent);
                agent.updatePosition = false;
                NPC aIControl = npc.GetComponent<NPC>();
                aIControl.isInspecting = true;
                //agent.transform.position -= transform.position;
                //agent.Warp(npc.localPosition);
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
        print("train enter");
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
       // alreadyStopped = false;
        tramAgent.isStopped = false;
    }

    private void OnTriggerExit(Collider other)
    {
        print("train exit");
        if (other.TryGetComponent<TramStation>(out TramStation tramStation_))
        {
            tramStation = null;
            alreadyStopped = false;
        }
    }

    //private void FixedUpdate()
    //{
    //    foreach(NavMeshAgent agent in agents)
    //        agent.transform.position -= transform.position;
    //}
}
