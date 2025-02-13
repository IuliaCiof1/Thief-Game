using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchHandSprite : MonoBehaviour
{
    [SerializeField] GameObject handPalm, handFist;

    [SerializeField] bool isGrabbing;

    Image imageComp;

   // public void OnEnable()
   // {
   //     imageComp.sprite = handFist;
   // }

   //public void OnDisable()
   // {
   //     imageComp.sprite = handPalm;
   // }

    // Start is called before the first frame update
    void Start()
    {
        imageComp =  GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing)
        {
            handFist.SetActive(true);
            handPalm.SetActive(false);
        }
        else
        {
            handFist.SetActive(false);
            handPalm.SetActive(true);
        }
    }
}
