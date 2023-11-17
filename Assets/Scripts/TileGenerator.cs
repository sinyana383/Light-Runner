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
    private float spawnPos = 0;
    
    [SerializeField] private GameObject[] tilesPrefabs;
    // TODO: Auto-recognize length
    [SerializeField] private float tileLength = 10;
    

    [SerializeField] private Transform player;
    void Start()
    {
        for (int i = 0; i < onStartGrounds; ++i)
        {
            SpawnGround(Random.Range(0, tilesPrefabs.Length));
        }
    }
    
    void Update()
    {
        if (player.position.z - tilesInGround * tileLength > spawnPos - (onStartGrounds * tilesInGround * tileLength))
        {
            SpawnGround(Random.Range(0, tilesPrefabs.Length));
            DeleteAndDestroyTile();
        }

    }

    // TODO: make a cycle run
    private void SpawnGround(int tileIndex)
    {
        GameObject newGround = Instantiate(groundPrefab, Vector3.forward * spawnPos, transform.rotation);
        for (int i = 0; i < tilesInGround; i++)
        {
            var tilePosition = Vector3.forward * spawnPos + new Vector3(0, 0, i * tileLength);
            GameObject newTile = Instantiate(tilesPrefabs[tileIndex], tilePosition, transform.rotation);
            newTile.transform.SetParent(newGround.transform);
        }
        presentGrounds.Add(newGround);
        
        spawnPos += tilesInGround * tileLength;
    }

    private void DeleteAndDestroyTile()
    {
        Destroy(presentGrounds[0]);
        presentGrounds.RemoveAt(0);
    }
}
