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
    private int stackNumber = 0;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    private void Start()
    {
        TestStackReceiver.OnStackTested += HandleStackTested;
        TestStackReceiver.OnStackReset += ResetPosition;
    }

    public void Setup(BlockData blockData, Vector3 spawnPostion, Quaternion spawnRotation, int stackNumber)
    {
        this.blockData = blockData;
        originalPosition = spawnPostion;
        originalRotation = spawnRotation;
        this.stackNumber = stackNumber;
        transform.SetLocalPositionAndRotation(spawnPostion, spawnRotation);
    }

    private void HandleStackTested(int stack)
    {
        if (stack != stackNumber) { return; }

        DOTween.Kill(transform);
        rb.useGravity = true;
        rb.isKinematic = false;

        if (blockData.Mastery > 0) { return; } // Check for glass blocks

        gameObject.SetActive(false);
    }

    private void ResetPosition(int stack)
    {
        if (stack != stackNumber) { return; }

        rb.useGravity = false;
        rb.isKinematic = true;

        transform.DOMove(originalPosition, Random.Range(minTime, maxTime)).SetEase(Ease.InOutCubic);
        transform.DORotate(originalRotation.eulerAngles, Random.Range(minTime, maxTime)).SetEase(Ease.InOutCubic);

        if (blockData.Mastery > 0) { return; } // Check for glass blocks

        gameObject.SetActive(true);
    }

    public BlockData GetBlockData() => blockData;

    private void OnDestroy()
    {
        DOTween.Kill(transform);
        TestStackReceiver.OnStackTested -= HandleStackTested;
        TestStackReceiver.OnStackReset -= ResetPosition;
    }
}
