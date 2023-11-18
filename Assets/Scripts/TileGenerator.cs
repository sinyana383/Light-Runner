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
    
    [SerializeField] private GameObject obstaclePrefab;
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
        bool wasFirst = false;
        int prevPos = -2;
        for (int i = 0; i < tilesInGround; i++)
        {
            var tilePosition = Vector3.forward * spawnPos + new Vector3(0, 0, i * tileLength);
            GameObject newRow = SpawnRaw(tilePosition.z, tileIndex);
            if (Random.Range(0, 1f) < 0.2f || wasFirst)
            {
                prevPos = SpawnObstacles(tilePosition.z, newRow, prevPos);
                wasFirst = !wasFirst;
                if (!wasFirst)
                    prevPos = -2;
            }
            newRow.transform.SetParent(newGround.transform);
        }
        presentGrounds.Add(newGround);
        
        spawnPos += tilesInGround * tileLength;
    }

    private GameObject SpawnRaw(float posZ, int tileIndex)
    {
        var row = new GameObject();
        
        row.transform.position = new Vector3(0f, 0f, posZ);
        row.transform.rotation = transform.rotation;

        GameObject middleTile = Instantiate(tilesPrefabs[tileIndex], new Vector3(0, 0, posZ), transform.rotation, row.transform);
        GameObject leftTile = Instantiate(tilesPrefabs[tileIndex], new Vector3(-tileLength, 0, posZ), transform.rotation, row.transform);
        GameObject rightTile = Instantiate(tilesPrefabs[tileIndex], new Vector3(tileLength, 0, posZ), transform.rotation, row.transform);

        return row;
    }

    private int SpawnObstacles(float posZ, GameObject parent, int prevPos = -2)
    {
        int pos = prevPos < -1 || prevPos > 1? Random.Range(-1, 1 + 1) : prevPos;
        
        GameObject obstacle1 = Instantiate(obstaclePrefab, new Vector3(tileLength * pos, 3f, posZ), transform.rotation, parent.transform);
        return pos;
    }

    private void DeleteAndDestroyTile()
    {
        Destroy(presentGrounds[0]);
        presentGrounds.RemoveAt(0);
    }
}
