using UnityEngine;

public class GlassBlock : MonoBehaviour
{
    private void Start()
    {
        TestStackReceiver.OnStackTested += HandleStackTested;
        TestStackReceiver.OnStackReset += ResetBlock;
    }

    private void HandleStackTested()
    {
        gameObject.SetActive(false);
    }

    private void ResetBlock()
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        TestStackReceiver.OnStackTested -= HandleStackTested;
        TestStackReceiver.OnStackReset -= ResetBlock;
    }
}
