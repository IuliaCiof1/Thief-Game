using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealCursor : MonoBehaviour
{
    CursorController cursorController;
    RectTransform cursorRect;

    [SerializeField] RectTransform backgroundRect;
    [SerializeField] Camera camera;
    [SerializeField] Vector2 cursorOffset;


    private void Awake()
    {
        cursorController = FindFirstObjectByType<CursorController>();
        cursorRect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        cursorController.EnableVirtualCursor();   
    }

    private void Update()
    {
        
        MoveVirtualCursor(backgroundRect, cursorRect, cursorOffset, camera);
    }

    private void OnDisable()
    {
        cursorController.CursorVisibility(false);
    }


    public void MoveVirtualCursor(RectTransform backgroundRect, RectTransform cursorRect, Vector2 cursorOffset, Camera camera)
    {

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
