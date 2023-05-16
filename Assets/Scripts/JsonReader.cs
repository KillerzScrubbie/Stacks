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

    private Dictionary<string, List<BlockData>> allGradesData = new();
    private List<string> gradeLevels = new();

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
            // Debug.Log($"Success: {webData.downloadHandler.text}");

            SplitList(result);
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

    private void SplitList(List<BlockData> blockDatas)
    {
        List<BlockData> splitList = new();

        splitList = blockDatas;

        foreach (var blockData in splitList.GroupBy(x => x.Grade)) 
        {
            string gradeLevelName = blockData.Key;

            allGradesData.Add(gradeLevelName, SortList(blockData.ToList())); // Split then sort
            gradeLevels.Add(gradeLevelName);

            Debug.Log($"Grade {blockData.Key}: {allGradesData[blockData.Key].Count}");
        }
    }

    public List<string> GetGradeLevelNames() => gradeLevels;
    public Dictionary<string, List<BlockData>> GetAllBlockData() => allGradesData;
}
