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

    private void Update()
    {
        cursorController.EnableVirtualCursor(backgroundRect, cursorRect, cursorOffset, camera);
    }

    private void OnDisable()
    {
        cursorController.CursorVisibility(false);
    }
}
