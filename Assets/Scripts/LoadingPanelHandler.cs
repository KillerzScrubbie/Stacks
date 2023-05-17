using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelHandler : MonoBehaviour
{
    private void Start()
    {
        JsonReader.OnDataFullyLoaded += DisablePanel;
        JsonReader.OnDataFailedToLoad += HandleError;
    }

    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    private void HandleError(string message)
    {

    }

    private void OnDestroy()
    {
        JsonReader.OnDataFullyLoaded -= DisablePanel;
        JsonReader.OnDataFailedToLoad -= HandleError;
    }
}
