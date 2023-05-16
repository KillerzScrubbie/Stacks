using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaBlockSpawner : MonoBehaviour
{
    [Header("Spawnpoint Transforms")]
    [SerializeField] private Transform firstSpawnLocation;
    [SerializeField] private float towerOffset = 3f;
    [SerializeField] private float yOffset = 0.01f;

    [Space]
    [Header("Block Prefabs")]
    [SerializeField] private Transform parentBlockPrefab;
    [SerializeField] private GameObject glassBlockPrefab;
    [SerializeField] private GameObject woodBlockPrefab;
    [SerializeField] private GameObject stoneBlockPrefab;

    [Space]
    [SerializeField] private Transform parentObject;

    private float xDimension = 0f;
    private float yDimension = 0f;
    private float zDimension = 0f;
    private float blockOffset = 0f;

    private void Start()
    {
        GetDimensions();

        JsonReader.OnDataFullyLoaded += SpawnBlocks;
    }

    private void GetDimensions()
    {
        Collider collider = parentBlockPrefab.GetComponent<BoxCollider>();
        Vector3 colliderSize = collider.bounds.extents;

        xDimension = colliderSize.x;
        yDimension = colliderSize.y;
        zDimension = colliderSize.z;

        blockOffset = (zDimension - xDimension * 3) * 0.5f;
    }

    private void SpawnBlocks(List<string> gradeLevels, Dictionary<string, List<BlockData>> allBlocksData)
    {
        Vector3 spawnLocation = firstSpawnLocation.position;

        foreach (string gradeLevel in gradeLevels)
        {
            List<BlockData> blockDatas = allBlocksData[gradeLevel];
            GameObject objectToSpawn;

            foreach (var blockData in blockDatas)
            {
                objectToSpawn = blockData.Mastery switch
                {
                    1 => woodBlockPrefab,
                    2 => stoneBlockPrefab,
                    _ => glassBlockPrefab,
                };

                Instantiate(objectToSpawn, spawnLocation, Quaternion.identity, parentObject);
                spawnLocation.x += blockOffset + xDimension;
            }

            spawnLocation.x += towerOffset;
        }
    }

    private void OnDestroy()
    {
        JsonReader.OnDataFullyLoaded -= SpawnBlocks;
    }
}
