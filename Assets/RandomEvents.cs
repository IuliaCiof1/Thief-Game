using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RandomEvent
{
    public GameObject eventZone;
    public Sprite poster;
}

public class RandomEvents: MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] int eventChance = 8;

    [SerializeField] RandomEvent[] randomEvents;


    [SerializeField] Image posterUI;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("tutorialEnabled") == 1)
            return;

        int isEvent = Random.Range(0, 10);
        if (isEvent < eventChance)
        {
            int eventIndex = Random.Range(-5, randomEvents.Length);

            if (eventIndex > -1)
            {
                posterUI.transform.parent.gameObject.SetActive(true);
                posterUI.sprite = randomEvents[eventIndex].poster;
                randomEvents[eventIndex].eventZone.SetActive(true);
            }
            //eventZone[eventIndex].SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            posterUI.transform.parent.gameObject.SetActive(false);
        }
    }
}
