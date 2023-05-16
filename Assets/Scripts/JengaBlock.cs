using UnityEngine;
using DG.Tweening;

public class JengaBlock : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float minTime = 1.5f;
    [SerializeField] private float maxTime = 3f;

    private BlockData blockData;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Start()
    {
        TestStackReceiver.OnStackTested += HandleStackTested;
        TestStackReceiver.OnStackReset += ResetPosition;
    }

    public void Setup(BlockData blockData, Vector3 spawnPostion, Quaternion spawnRotation)
    {
        this.blockData = blockData;
        originalPosition = spawnPostion;
        originalRotation = spawnRotation;
        transform.SetLocalPositionAndRotation(spawnPostion, spawnRotation);
    }

    private void HandleStackTested()
    {
        DOTween.Kill(transform);
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void ResetPosition()
    {
        rb.useGravity = false;
        rb.isKinematic = true;

        transform.DOMove(originalPosition, Random.Range(minTime, maxTime)).SetEase(Ease.InOutCubic);
        transform.DORotate(originalRotation.eulerAngles, Random.Range(minTime, maxTime)).SetEase(Ease.InOutCubic);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
        TestStackReceiver.OnStackTested -= HandleStackTested;
        TestStackReceiver.OnStackReset -= ResetPosition;
    }
}
