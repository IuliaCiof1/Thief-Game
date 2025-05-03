using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    public float baseRotation = 80f;  // Rotation when camera is right above
    public float rotationMultiplier = 2f;  // Adjust sensitivity
    [SerializeField]TextMeshPro text;
    [SerializeField] SpriteRenderer bubbleBackround;

    bool showBubble;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showBubble)
        {
            //transform.GetChild(0).gameObject.SetActive(true);
          
            float cameraZ = Camera.main.transform.position.z;

            // Get the difference in Z position
            float zDifference = cameraZ - transform.position.z;
            float xRotation = baseRotation + (zDifference * rotationMultiplier);

            // Clamp the rotation to prevent extreme tilting
            xRotation = Mathf.Clamp(xRotation, 50f, 180f);


            // Apply only X-axis rotation
            transform.rotation = Quaternion.Euler(xRotation, 0, 0);
            //showBubble = false;
            //StartCoroutine(HideBubble());
            // transform.GetChild(0).gameObject.SetActive(false);
        }
        //else
        //{
        //    showBubble = false;
        //    transform.GetChild(0).gameObject.SetActive(false);
        //}
    }

    public void ShowBubble(string textLine)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        showBubble = true;

        text.SetText(textLine);
        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues(false);
        print(textSize);
        bubbleBackround.size = textSize;
        bubbleBackround.transform.localPosition = new Vector3(bubbleBackround.size.x / 2f, 0f);
        Vector2 pading = new Vector2(0.2f, 0.3f);
        bubbleBackround.size += pading;
        

        StartCoroutine(HideBubble());
    }

    IEnumerator HideBubble()
    {
        yield return new WaitForSeconds(5);
        showBubble = false;
        transform.GetChild(0).gameObject.SetActive(false);

        bubbleBackround.size = new Vector2(0.1f, 0.1f);
        bubbleBackround.transform.localPosition = Vector3.zero;
    }
}
