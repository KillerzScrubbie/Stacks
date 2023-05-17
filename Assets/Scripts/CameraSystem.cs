using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float camPositionMinY = 2f;
    [SerializeField] private float camPositionMaxY = 11f;

    private Vector2 moveDirection = Vector2.zero;
    private Vector3 followOffset;
    private CinemachineTransposer cinemachineTransposer;

    private void Start()
    {
        InputManager.OnMouseMoved += ReadMouseInput;
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        followOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float rotateDirection = moveDirection.x;
        float camOffsetDirection = moveDirection.y;

        transform.eulerAngles += new Vector3(0f, rotateDirection * rotateSpeed * Time.deltaTime, 0f);

        followOffset.y = Mathf.Clamp(followOffset.y + camOffsetDirection * 0.1f, camPositionMinY, camPositionMaxY);
        //cinemachineTransposer.m_FollowOffset += new Vector3(0f, 0.1f * rotateSpeed * Time.deltaTime * followOffset.y, 0f);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * rotateSpeed * 0.1f);
        /*cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * rotateSpeed * 0.1f);*/
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
