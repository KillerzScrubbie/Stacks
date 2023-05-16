using System;
using UnityEngine;

public class TestStackReceiver : MonoBehaviour
{
    public static event Action OnStackTested;
    public static event Action OnStackReset;

    public void TestStack()
    {
        OnStackTested?.Invoke();
    }

    public void ResetStack()
    {
        OnStackReset?.Invoke();
    }
}
