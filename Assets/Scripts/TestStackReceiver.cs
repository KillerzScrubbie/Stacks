using System;
using UnityEngine;

public class TestStackReceiver : MonoBehaviour
{
    public static event Action OnStackTested;

    public void TestStack()
    {
        OnStackTested?.Invoke();
    }
}
