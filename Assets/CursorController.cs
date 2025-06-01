using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
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


    public void EnableVirtualCursor(RectTransform backgroundRect, RectTransform cursorRect , Vector2 cursorOffset, Camera camera)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;

        // Get the mouse position in screen space
        Vector2 mousePos = Input.mousePosition;

        // Convert the screen space mouse position to local space of the monitor
        RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundRect, mousePos, camera, out Vector2 localMousePos);

        // Get the bounds of the monitor RectTransform in local coordinates
        Vector3[] corners = new Vector3[4];
        backgroundRect.GetLocalCorners(corners);

        // Clamp mouse position to stay within monitor bounds
        float clampedX = Mathf.Clamp(localMousePos.x, corners[0].x, corners[2].x);
        float clampedY = Mathf.Clamp(localMousePos.y, corners[0].y, corners[1].y);

        // Update cursorRect position to the clamped position plus offset
        cursorRect.localPosition = new Vector3(clampedX + cursorOffset.x, clampedY + cursorOffset.y, cursorRect.localPosition.z);
    }
}
