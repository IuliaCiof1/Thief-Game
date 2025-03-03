using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConfinePointer :MonoBehaviour
{
    Vector3 lastMousePosition;

    public void OnPointerExit(PointerEventData eventData)
    {
        //Input.mousePosition = lastMousePosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastMousePosition = Input.mousePosition;
    }
}
