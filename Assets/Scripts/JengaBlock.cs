using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaBlock : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private BlockData blockData;

    private void Start()
    {
        TestStackReceiver.OnStackTested += HandleStackTested;
    }

    public void Setup(BlockData blockData)
    {
        this.blockData = blockData;
    }

    private void HandleStackTested()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void OnDestroy()
    {
        TestStackReceiver.OnStackTested -= HandleStackTested;
    }
}
