using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    [SerializeField] private Transform cameraAnchor;
    [SerializeField] private Transform gradeText;

    [Space]
    [SerializeField] private Transform parentObject;
    [SerializeField] private StackControls stackControls;

    private float xDimension = 0f;
    private float yDimension = 0f;
    private float zDimension = 0f;
    private float blockOffset = 0f;

    private Quaternion normalRotation = Quaternion.identity;
    private Quaternion perpendicularRotation = Quaternion.Euler(0f, 90f, 0f);

    public static event Action<Transform> OnCameraAnchorCreated;

    private void Start()
    {
        GetDimensions();

        JsonReader.OnDataLoaded += SpawnBlocks;
    }

    private void GetDimensions()
    {
        Collider collider = parentBlockPrefab.GetComponent<BoxCollider>();
        Vector3 colliderSize = collider.bounds.extents;

        xDimension = colliderSize.x * 2;
        yDimension = colliderSize.y * 2;
        zDimension = colliderSize.z * 2;

        blockOffset = (zDimension - xDimension * 3) * 0.5f;
    }

    private void SpawnBlocks(List<string> gradeLevels, Dictionary<string, List<BlockData>> allBlocksData)
    {
        Vector3 startingSpawnLocation = firstSpawnLocation.position;
        Vector3 spawnLocation = startingSpawnLocation;
        Quaternion rotation = normalRotation;

        GameObject objectToSpawn;
        int stackCount = 0;

        foreach (string gradeLevel in gradeLevels)
        {
            List<BlockData> blockDatas = allBlocksData[gradeLevel];

            Vector3 firstBlockInOddRowLocation = startingSpawnLocation;
            Vector3 firstBlockInEvenRowLocation = new(
                startingSpawnLocation.x + xDimension + blockOffset,
                startingSpawnLocation.y,
                startingSpawnLocation.z - xDimension - blockOffset);

            int blockCount = 0;
            int rowCount = 0;
            bool isOddRow = true;

            TextMeshPro gradeTextObject = Instantiate(gradeText, new(
                startingSpawnLocation.x + xDimension + blockOffset,
                0.05f, -2.7f), Quaternion.Euler(90f, 0f, 0f), parentObject).GetComponent<TextMeshPro>();

            gradeTextObject.text = gradeLevel;

            foreach (var blockData in blockDatas)
            {
                objectToSpawn = blockData.Mastery switch
                {
                    1 => woodBlockPrefab,
                    2 => stoneBlockPrefab,
                    _ => glassBlockPrefab,
                };

                rowCount = (blockCount / 3) + 1;
                isOddRow = rowCount % 2 != 0;

                switch (isOddRow)
                {
                    case true: // Odd rows
                        rotation = normalRotation;
                        spawnLocation = GetSpawnLocation(firstBlockInOddRowLocation, rowCount, blockCount, isOddRow);
                        break;
                    case false: // Even rows
                        rotation = perpendicularRotation;
                        spawnLocation = GetSpawnLocation(firstBlockInEvenRowLocation, rowCount, blockCount, isOddRow);
                        break;
                }

                JengaBlock spawnedBlock = Instantiate(objectToSpawn, parentObject).GetComponent<JengaBlock>();
                
                spawnedBlock.Setup(blockData, spawnLocation, rotation);
                blockCount++;
            }

            Transform cameraLookAtPosition = Instantiate(cameraAnchor, new(startingSpawnLocation.x + xDimension + blockOffset, (yOffset + yDimension) * rowCount * 0.5f, 0f), Quaternion.identity, parentObject);
            OnCameraAnchorCreated?.Invoke(cameraLookAtPosition);
            stackControls.CreateButton(gradeLevel, stackCount);

            stackCount++;
            startingSpawnLocation.x += towerOffset;
        }
    }

    private Vector3 GetSpawnLocation(Vector3 startRowPosition, int rowCount, int blockCount, bool isOddRow)
    {
        int blockCountInRow = blockCount % 3;
        rowCount--;

        switch (isOddRow) // Odd +x, Even +z
        {
            case true:
                return new(startRowPosition.x + (blockOffset + xDimension) * blockCountInRow, startRowPosition.y + (yOffset + yDimension) * rowCount, startRowPosition.z);
            case false:
                return new(startRowPosition.x, startRowPosition.y + (yOffset + yDimension) * rowCount, startRowPosition.z + (blockOffset + xDimension) * blockCountInRow);
        }
    }

    private void OnDestroy()
    {
        JsonReader.OnDataLoaded -= SpawnBlocks;
    }
}
