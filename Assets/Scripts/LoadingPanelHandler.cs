using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanelHandler : MonoBehaviour
{
    private void Start()
    {
        JsonReader.OnDataFullyLoaded += DisablePanel;
    }

    private void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        JsonReader.OnDataFullyLoaded -= DisablePanel;
    }
}
