using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tilesPrefabs;
    [SerializeField] private List<GameObject> presentTiles = new List<GameObject>();
    private float spawnPos = 0;
    // TODO: Auto-recognize length
    [SerializeField] private float tileLength = 100;
    [SerializeField] private int onStartTiles = 3;

    [SerializeField] private Transform player;
    void Start()
    {
        for (int i = 0; i < onStartTiles; ++i)
        {
            SpawnTile(Random.Range(0, tilesPrefabs.Length));
        }
    }
    
    void Update()
    {
        if (player.position.z - tileLength > spawnPos - (onStartTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilesPrefabs.Length));
            DeleteAndDestroyTile();
        }

    }

    // TODO: make a cycle run
    private void SpawnTile(int tileIndex)
    {
        GameObject newTile = Instantiate(tilesPrefabs[tileIndex], transform.forward * spawnPos, transform.rotation);
        presentTiles.Add(newTile);
        
        spawnPos += tileLength;
    }

    private void DeleteAndDestroyTile()
    {
        Destroy(presentTiles[0]);
        presentTiles.RemoveAt(0);
    }
}
