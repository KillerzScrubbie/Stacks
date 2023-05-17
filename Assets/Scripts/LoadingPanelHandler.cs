using UnityEngine;
using TMPro;

public class LoadingPanelHandler : MonoBehaviour
{
    [SerializeField] private JsonReader jsonReader;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private GameObject retryButton;

    private readonly string loadingText = "Loading...";
    private readonly string errorText = "Connection Failed";

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
        statusText.text = errorText;
        errorMessageText.text = message;

        errorMessageText.gameObject.SetActive(true);
        retryButton.SetActive(true);
    }

    public void RetryConnection()
    {
        statusText.text = loadingText;

        errorMessageText.gameObject.SetActive(false);
        retryButton.SetActive(false);

        jsonReader.RequestData();
    }

    private void OnDestroy()
    {
        JsonReader.OnDataFullyLoaded -= DisablePanel;
        JsonReader.OnDataFailedToLoad -= HandleError;
    }
}
