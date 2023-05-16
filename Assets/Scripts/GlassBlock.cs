using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBlock : MonoBehaviour
{
    private void Start()
    {
        TestStackReceiver.OnStackTested += HandleStackTested;
    }

    private void HandleStackTested()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        TestStackReceiver.OnStackTested -= HandleStackTested;
    }
}
