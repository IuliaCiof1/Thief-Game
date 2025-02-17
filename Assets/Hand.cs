using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject handPalm, handFist;
    [SerializeField] bool isGrabbing;
    [SerializeField] float smoothing;
    [SerializeField] Canvas canvas;

    private void Start()
    {
       
    }

    void Update()
    {
        //Move Hand UI position with the mouse cursor.
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = Vector3.Lerp(transform.position, canvas.transform.TransformPoint(pos), smoothing * Time.deltaTime);

       // transform.position = Vector3.Lerp(transform.position, Input.mousePosition, smoothing*Time.deltaTime);
        if (Input.GetMouseButton(0))
        {
            isGrabbing = true;
        }
        else
            isGrabbing = false;

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
