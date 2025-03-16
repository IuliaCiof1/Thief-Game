using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PocketItemsGeneraton : MonoBehaviour
{
    [SerializeField] GameObject pocket;
    //[SerializeField] Sprite[] itemSprites;
    [SerializeField] PocketItemSO[] pocketItemSOs;
    [SerializeField] GameObject agentsParent;
    [SerializeField] Vector2Int minMaxNumberOfItems;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform agent in agentsParent.transform)
        {
            int numberOfItems = Random.Range(minMaxNumberOfItems.x, minMaxNumberOfItems.y);

            List<PocketItemSO> randomSprites = new List<PocketItemSO>();

            for (int i = 0; i < numberOfItems; i++) {
                int spriteIndex = Random.Range(0, pocketItemSOs.Length);

                randomSprites.Add(pocketItemSOs[spriteIndex]);
            }

            agent.GetComponent<AIControl>().ItemsInPocket = randomSprites;

        }
    }


    public void CreateItems(AIControl agent)
    {
        int itemIndex = 0;

        foreach(Transform item in pocket.transform)
        {
            if (itemIndex == agent.ItemsInPocket.Count)
                item.gameObject.SetActive(false);
            else
            {
                print("create item");
                item.gameObject.SetActive(true);
                item.GetComponent<Image>().sprite = agent.ItemsInPocket[itemIndex].sprite;
                item.GetComponent<PocketItem>().Value = agent.ItemsInPocket[itemIndex].value;
                itemIndex++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
