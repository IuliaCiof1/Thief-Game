using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleActivator : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] int showChance;
    [SerializeField] string[] textLines;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DetectNPC", 0, 3);
    }

    // Update is called once per frame
    void DetectNPC()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
        
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent<NPC>(out NPC npc))
            {
                int random = Random.Range(0, 100);

                if (random <= showChance)
                {
                    random = Random.Range(0, textLines.Length);
                    npc.GetComponentInChildren<SpeechBubble>().ShowBubble(textLines[random]);
                }
            }
        }
    }
}
