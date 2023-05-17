using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private MouseControls mouseControls;
    private bool mouseDown = false;

    public static event Action<Vector2> OnMouseMoved;

    private void Awake()
    {
        mouseControls = new();
    }

    private void Start()
    {
        mouseControls.Mouse.OrbitClick.started += _ => ActivateOrbit(true);
        mouseControls.Mouse.OrbitClick.canceled += _ => ActivateOrbit(false);
    }

    private void ActivateOrbit(bool active)
    {
        mouseDown = active;

        if (mouseDown) { return; }

        OnMouseMoved?.Invoke(Vector2.zero);
    }

    private void Update()
    {
        if (!mouseDown) { return; }

        OnMouseMoved?.Invoke(mouseControls.Mouse.OrbitDelta.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        mouseControls.Enable();
    }

    private void OnDisable()
    {
        mouseControls.Disable();
    }
}
