using UnityEngine;
using TMPro;

public class DomainDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI domainNameText;
    [SerializeField] private TextMeshProUGUI clusterText;
    [SerializeField] private TextMeshProUGUI standardIdDescText;
    [SerializeField] private GameObject domainDisplayPanel;

    // Start is called before the first frame update
    private void Start()
    {
        MouseClickRaycast.OnBlockClicked += DisplayText;

        domainDisplayPanel.SetActive(false);
    }

    private void DisplayText(BlockData blockData)
    {
        domainDisplayPanel.SetActive(true);

        domainNameText.text = $"{blockData.Grade}: {blockData.Domain}";
        clusterText.text = $"{blockData.Cluster}";
        standardIdDescText.text = $"{blockData.StandardId}: {blockData.StandardDescription}";
    }

    private void OnDestroy()
    {
        MouseClickRaycast.OnBlockClicked -= DisplayText;
    }
}
