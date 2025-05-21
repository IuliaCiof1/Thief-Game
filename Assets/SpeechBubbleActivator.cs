using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleActivator : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] int showChance;
    [SerializeField] string[] textLines;
    ThirdPersonController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<ThirdPersonController>();
        InvokeRepeating("DetectNPC", 0, 3);
    }

    // Update is called once per frame
    void DetectNPC()
    {
        if (!player.inTram)
          
        {
           
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<NPC>(out NPC npc))
                {
                    int random = Random.Range(0, 100);

                    if (random <= showChance)
                    {
                        random = Random.Range(0, textLines.Length);
                        SpeechBubble speechBubble = npc.GetComponentInChildren<SpeechBubble>();
                        if (speechBubble != null)
                            speechBubble.ShowBubble(textLines[random]);
                    }
                }
            }
        }
    }
}
