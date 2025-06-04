using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    CursorLockMode prevCursorLockMode;
    bool prevCursorVisible;

    // Start is called before the first frame update
    void Start()
    {
        CursorVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CursorVisibility(bool visibiliy)
    {
        prevCursorLockMode = Cursor.lockState;
        prevCursorVisible = Cursor.visible;
        print("cursor visibility " + visibiliy);
        if (!visibiliy)
        {
            print("cursor visibility off");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            print("cursor visibility on");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

    }


    public void SetToPreveousCursorState()
    {
        print("prev cursor " + Cursor.visible);
        Cursor.visible = prevCursorVisible;
        Cursor.lockState = prevCursorLockMode;
    }

    public void EnableVirtualCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
