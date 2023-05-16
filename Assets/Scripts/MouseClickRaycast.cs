using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseClickRaycast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private LayerMask jengaBlockMask;

    private Camera mainCamera;

    public static event Action<BlockData> OnBlockClicked;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        ProcessMouseClick(ray);
    }

    private void ProcessMouseClick(Ray ray)
    {
        if (!Physics.Raycast(ray, out RaycastHit hit, rayDistance, jengaBlockMask)) { return; }

        if (IsMouseOverUIObject()) { return; }

        if (!hit.transform.TryGetComponent(out JengaBlock block)) { return; }

        BlockData blockData = block.GetBlockData();

        OnBlockClicked?.Invoke(blockData);
    }

    private bool IsMouseOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
