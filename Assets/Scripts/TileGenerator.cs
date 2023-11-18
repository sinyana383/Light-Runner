using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private List<GameObject> presentGrounds = new List<GameObject>();
    [SerializeField] private float tilesInGround = 50;
    [SerializeField] private int onStartGrounds = 6;
    private float spawnPos;
    
    [SerializeField] private GameObject[] tilesPrefabs;
    [SerializeField] private float tileLength;

    [SerializeField] private Transform player;
    
    [SerializeField] private GameObject upObstaclePrefab;
    [SerializeField] private GameObject downObstaclePrefab;
    void Start()
    {
        tileLength = tilesPrefabs[0].transform.localScale.z;
        
        spawnPos = -(tilesInGround * tileLength) / 2;
        for (int i = 0; i < onStartGrounds; ++i)
        {
            SpawnGround(Random.Range(0, tilesPrefabs.Length));
        }
    }
    
    void Update()
    {
        if (player.position.z - tilesInGround * tileLength * 1.5f > spawnPos - (onStartGrounds * tilesInGround * tileLength))
        {
            SpawnGround(Random.Range(0, tilesPrefabs.Length));
            DeleteAndDestroyTile();
        }

    }

    // TODO: make a cycle run
    private void SpawnGround(int tileIndex)
    {
        GameObject newGround = Instantiate(groundPrefab, Vector3.forward * spawnPos, transform.rotation);
        bool wasUp = false;
        int prevPosUp = -2;
        for (int i = 0; i < tilesInGround; i++)
        {

            var tilePosition = Vector3.forward * spawnPos + new Vector3(0, 0, i * tileLength);
            GameObject newRow = SpawnRaw(tilePosition.z, tileIndex);
            
            if (Random.Range(0, 1f) < 0.2f || wasUp)
            {
                prevPosUp = SpawnUpObstacle(tilePosition.z, newRow, prevPosUp);
                wasUp = !wasUp;
                if (!wasUp)
                    prevPosUp = -2;
            }
            
            newRow.transform.SetParent(newGround.transform);
        }
        presentGrounds.Add(newGround);
        
        spawnPos += tilesInGround * tileLength;
    }

    private GameObject SpawnRaw(float posZ, int tileIndex)
    {
        var row = new GameObject();
        int obstaclePos = -2;
        
        row.transform.position = new Vector3(0f, 0f, posZ);
        row.transform.rotation = transform.rotation;

        if (Random.Range(0, 1f) < 0.2f)
            obstaclePos = SpawnDownObstacle(posZ, row);

        if (obstaclePos != 0)
            Instantiate(tilesPrefabs[tileIndex], new Vector3(0, 0, posZ), transform.rotation, row.transform);
        if (obstaclePos != -1)
            Instantiate(tilesPrefabs[tileIndex], new Vector3(-tileLength, 0, posZ), transform.rotation, row.transform);
        if (obstaclePos != 1)
            Instantiate(tilesPrefabs[tileIndex], new Vector3(tileLength, 0, posZ), transform.rotation, row.transform);

        return row;
    }
    
    private int SpawnDownObstacle(float posZ, GameObject parent, int prevPos = -2)
    {
        int pos = prevPos < -1 || prevPos > 1? Random.Range(-1, 1 + 1) : prevPos;
        
        GameObject obstacle1 = Instantiate(downObstaclePrefab, new Vector3(tileLength * pos, 0f, posZ), transform.rotation, parent.transform);
        return pos;
    }

    private int SpawnUpObstacle(float posZ, GameObject parent, int prevPos = -2)
    {
        int pos = prevPos < -1 || prevPos > 1? Random.Range(-1, 1 + 1) : prevPos;
        
        GameObject obstacle1 = Instantiate(upObstaclePrefab, new Vector3(tileLength * pos, 3f, posZ), transform.rotation, parent.transform);
        return pos;
    }

    private void DeleteAndDestroyTile()
    {
        Destroy(presentGrounds[0]);
        presentGrounds.RemoveAt(0);
    }
}
