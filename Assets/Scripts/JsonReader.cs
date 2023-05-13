using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class JsonReader : MonoBehaviour
{
    [SerializeField] private string jsonUrl;

    private void Start()
    {
        RequestData();
    }

    [ContextMenu("Try Fetch")]
    public async void RequestData()
    {
        using var webData = UnityWebRequest.Get(jsonUrl);

        webData.SetRequestHeader("Content-Type", "application/json");

        var operation = webData.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        var jsonResponse = webData.downloadHandler.text;

        if (webData.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed: {webData.error}");
        }

        try
        {
            List<BlockData> result = JsonConvert.DeserializeObject<List<BlockData>>(jsonResponse);
            Debug.Log($"Success: {webData.downloadHandler.text}");

            List<BlockData> sortedList = SortList(result);
            
            foreach ( BlockData block in sortedList )
            {
                Debug.Log(block.Id);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not parse response {jsonResponse}. {e.Message}");
        }
    }

    private List<BlockData> SortList(List<BlockData> blockDatas)
    {
        List<BlockData> sortedList = new();

        sortedList = blockDatas;

        return sortedList.OrderBy(x => x.Domain).ThenBy(x => x.Cluster).ThenBy(x => x.StandardId).ToList();
    }
}
