using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 2f;

    private Vector2 moveDirection = Vector2.zero;

    private void Start()
    {
        InputManager.OnMouseMoved += ReadMouseInput;
    }

    private void Update()
    {
        float rotateDirection = moveDirection.x;

        transform.eulerAngles += new Vector3(0f, rotateDirection * rotateSpeed * Time.deltaTime);
    }

    private void ReadMouseInput(Vector2 delta)
    {
        moveDirection = delta;
    }

    private void OnDestroy()
    {
        InputManager.OnMouseMoved -= ReadMouseInput;
    }
}
