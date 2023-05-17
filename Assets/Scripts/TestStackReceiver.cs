using System;
using UnityEngine;

public class TestStackReceiver : MonoBehaviour
{
    public static event Action<int> OnStackTested;
    public static event Action<int> OnStackReset;

    public void TestStack()
    {
        OnStackTested?.Invoke(CameraSystem.StackNumber);
    }

    public void ResetStack()
    {
        OnStackReset?.Invoke(CameraSystem.StackNumber);
    }
}
