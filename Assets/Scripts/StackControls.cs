using UnityEngine;

public class StackControls : MonoBehaviour
{
    [SerializeField] private GameObject stackControlButton;
    [SerializeField] private Transform buttonGroupLayout;

    public void CreateButton(string gradeLevel, int stackNumber)
    {
        StackControlButton button = Instantiate(stackControlButton, buttonGroupLayout).GetComponent<StackControlButton>();
        button.Setup(gradeLevel, stackNumber);
    }
}
