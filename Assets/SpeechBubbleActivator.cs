using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Speech
{
    public string Line;
}

[System.Serializable]
public class SpeechList
{
    public List<Speech> Speech = new List<Speech>();
}


public class SpeechBubbleActivator : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] int showChance;
    ThirdPersonController player;


    SpeechList speechList = new SpeechList();


// Start is called before the first frame update
void Start()
    {

        TextAsset asset = Resources.Load("Speech") as TextAsset; //Loads from Resources folder the file named "Speech.json"
        if (asset != null)
        {
            speechList = JsonUtility.FromJson<SpeechList>(asset.text);

        }
        else
            Debug.LogError("Could not load \"Speech.json\" file");

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
                        random = Random.Range(0, speechList.Speech.Count);
                        SpeechBubble speechBubble = npc.GetComponentInChildren<SpeechBubble>();
                        if (speechBubble != null)
                            speechBubble.ShowBubble(speechList.Speech[random].Line);
                    }
                }
            }
        }
    }



}
