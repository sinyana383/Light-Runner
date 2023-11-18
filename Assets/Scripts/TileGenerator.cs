using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileGenerator : MonoBehaviour
{
    // Ground
    private List<GameObject> presentGrounds = new List<GameObject>();
    [SerializeField] private int onStartGrounds = 5;
    [SerializeField] private float rowsInGround = 50;
    
    // Row
    private KeyValuePair<GameObject, int>[] curRow = new KeyValuePair<GameObject, int>[3];
    // Tiles
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private float tileLength;
    // Obstacles
    [SerializeField] private GameObject upObstaclePrefab;
    [SerializeField] private GameObject downStartObstaclePrefab;
    [SerializeField] private GameObject downFinishObstaclePrefab;

    private float nextSpawnPos;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject torchPrefab;
    void Start()
    {
        tileLength = tilePrefabs[0].transform.localScale.z;
        nextSpawnPos = -(rowsInGround * tileLength) / 2;
        Debug.Log(nextSpawnPos);
        
        SpawnNoObstaclesGround(Random.Range(0, tilePrefabs.Length));
        Debug.Log(nextSpawnPos);
        for (int i = 0; i < onStartGrounds; ++i)
        {
            SpawnGround(Random.Range(0, tilePrefabs.Length));
            Debug.Log(nextSpawnPos);
        }
    }

    private void SpawnNoObstaclesGround(int tileIndex)
    {
        var newGround = new GameObject();
        newGround.transform.position = Vector3.forward * nextSpawnPos;
        newGround.transform.rotation = transform.rotation;
        
        for (int j = 0; j < curRow.Length; j++)
            curRow[j] = new KeyValuePair<GameObject, int>(tilePrefabs[tileIndex], 1);
        for (int i = 0; i < rowsInGround; i++)
        {
            var rowPosZ = (Vector3.forward * nextSpawnPos + new Vector3(0, 0, i * tileLength)).z;
            
            GameObject newRow = SpawnRaw(curRow, rowPosZ);
            newRow.transform.SetParent(newGround.transform);
        }
        presentGrounds.Add(newGround);
        nextSpawnPos += rowsInGround * tileLength;
    }
    
    void Update()
    {
        if (player.position.z - rowsInGround * tileLength * 1.5f > nextSpawnPos - (onStartGrounds * rowsInGround * tileLength))
        {
            SpawnGround(Random.Range(0, tilePrefabs.Length));
            DeleteAndDestroyTile();
        }

    }

    
    private void SpawnGround(int tileIndex)
    {
        var newGround = new GameObject();
        newGround.transform.position = Vector3.forward * nextSpawnPos;
        newGround.transform.rotation = transform.rotation;

        for (int i = 0; i < rowsInGround; i++)
        {
            var rowPosZ = (Vector3.forward * nextSpawnPos + new Vector3(0, 0, i * tileLength)).z;

            GameObject newRow;
            for (int j = 0; j < curRow.Length; j++)
                curRow[j] = GetPrefabAndNumber(curRow[j].Key.tag, curRow[j].Value - 1, tileIndex);

            newRow = SpawnRaw(curRow, rowPosZ);
            newRow.transform.SetParent(newGround.transform);
        }
        
        presentGrounds.Add(newGround);
        nextSpawnPos += rowsInGround * tileLength;
    }

    private GameObject SpawnRaw(KeyValuePair<GameObject, int>[] curRow, float posZ)
    {
        var row = new GameObject();
        row.transform.position = new Vector3(0f, 0f, posZ);
        row.transform.rotation = transform.rotation;
        
        // Row
        float leftY = GetYByObjectTag(curRow[(int)Lines.Left].Key.tag);
        float middleY = GetYByObjectTag(curRow[(int)Lines.Middle].Key.tag);
        float rightY = GetYByObjectTag(curRow[(int)Lines.Right].Key.tag);
        
        Instantiate(curRow[(int)Lines.Left].Key, new Vector3(-tileLength, leftY, posZ), transform.rotation, row.transform);
        Instantiate(curRow[(int)Lines.Middle].Key, new Vector3(0, middleY, posZ), transform.rotation, row.transform);
        Instantiate(curRow[(int)Lines.Right].Key, new Vector3(tileLength, rightY, posZ), transform.rotation, row.transform);

        // Collectable
        if (Random.Range(0, 1f) < 0.05f)
        {
            int line = Random.Range((int)Lines.Left, (int)Lines.Right + 1);
            float yPos = GetYByObjectTag(curRow[line].Key.tag);
            
            Instantiate(torchPrefab, new Vector3((line - 1) * tileLength, yPos * 2 + 1f, posZ), transform.rotation, row.transform);
        }

        return row;
    }

    private float GetYByObjectTag(string keyTag)
    {
        // TODO: replace magic numbers
        switch (keyTag)
        {
            case "down obstacle":
                return -1f;
            case "up obstacle":
                return 3f;
            default:
                return 0f;
        }
    }
    
    private KeyValuePair<GameObject, int> GetPrefabAndNumber(string keyTag, int number, int tileIndex)
    {
        // Generate new
        if (number == 0)
        {
            if (Random.Range(0, 1f) < 0.05f)
                return GetPrefabAndNumber("down obstacle", 2, tileIndex);
            if (Random.Range(0, 1f) < 0.05f)
                return GetPrefabAndNumber("up obstacle", Random.Range(2, 3), tileIndex);
            return GetPrefabAndNumber("tile", 1, tileIndex);
        }
        
        // TODO: replace magic numbers
        switch (keyTag)
        {
            case "down obstacle":
                if (number == 1)
                    return new KeyValuePair<GameObject, int>(downFinishObstaclePrefab, number);
                else
                    return new KeyValuePair<GameObject, int>(downStartObstaclePrefab, number);
            case "up obstacle":
                return new KeyValuePair<GameObject, int>(upObstaclePrefab, number);
            default:
                return new KeyValuePair<GameObject, int>(tilePrefabs[tileIndex], number);
        }
    }

    private void DeleteAndDestroyTile()
    {
        Destroy(presentGrounds[0]);
        presentGrounds.RemoveAt(0);
    }
}
