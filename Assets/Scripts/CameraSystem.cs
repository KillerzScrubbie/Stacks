using Cinemachine;
using DG.Tweening;
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

    private List<Transform> cameraPositions = new();

    private void Start()
    {
        InputManager.OnMouseMoved += ReadMouseInput;
        JengaBlockSpawner.OnCameraAnchorCreated += AddCameraAnchorPosition;
        JsonReader.OnDataFullyLoaded += SetCameraDefaultPosition;
        StackControlButton.OnStackViewChanged += ChangeCameraView;

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
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * rotateSpeed * 0.1f);
    }

    private void ReadMouseInput(Vector2 delta)
    {
        moveDirection = delta;
    }

    private void AddCameraAnchorPosition(Transform lookAtTransform)
    {
        cameraPositions.Add(lookAtTransform);
    }

    private void SetCameraDefaultPosition()
    {
        ChangeCameraView(0);
    }

    private void ChangeCameraView(int stackNumber)
    {
        transform.DOMove(cameraPositions[stackNumber].position, 0.5f).SetEase(Ease.OutCubic);
    }

    private void OnDestroy()
    {
        InputManager.OnMouseMoved -= ReadMouseInput;
        JengaBlockSpawner.OnCameraAnchorCreated -= AddCameraAnchorPosition;
        JsonReader.OnDataFullyLoaded -= SetCameraDefaultPosition;
        StackControlButton.OnStackViewChanged -= ChangeCameraView;
    }
}
