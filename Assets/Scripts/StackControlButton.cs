using System;
using TMPro;
using UnityEngine;

public class StackControlButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;

    private int stackNumber;

    public static event Action<int> OnStackViewChanged;

    public void Setup(string text, int stackNumber)
    {
        this.stackNumber = stackNumber;
        buttonText.text = text;
    }

    public void SwapStack()
    {
        OnStackViewChanged?.Invoke(stackNumber);
    }
}
