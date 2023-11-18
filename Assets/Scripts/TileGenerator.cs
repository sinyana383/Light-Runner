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
    [SerializeField] private GameObject downStartObstaclePrefab;
    [SerializeField] private GameObject downFinishObstaclePrefab;
    [SerializeField] private int lastDownObstaclePos;
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

    // TODO: Optimize, if have time!!!
    private void SpawnGround(int tileIndex)
    {
        GameObject newGround = Instantiate(groundPrefab, Vector3.forward * spawnPos, transform.rotation);
        bool wasUp = false;
        bool wasDown = false;
        int prevPosUp = -2;
        int prevPosDown = -2;
        GameObject newRow;
        for (int i = 0; i < tilesInGround; i++)
        {
            var tilePosition = Vector3.forward * spawnPos + new Vector3(0, 0, i * tileLength);

            if (Random.Range(0, 1f) < 0.2f || wasDown)
            {
                if (prevPosDown == -2)
                    prevPosDown = SpawnDownObstacle(downStartObstaclePrefab, tilePosition.z, newGround);
                else
                    prevPosDown = SpawnDownObstacle(downFinishObstaclePrefab, tilePosition.z, newGround, prevPosDown);
                Debug.Log(prevPosDown);
                newRow = SpawnRaw(tilePosition.z, tileIndex, prevPosDown);
                wasDown = !wasDown;
                if (!wasDown)
                {
                    lastDownObstaclePos = prevPosDown;
                    prevPosDown = -2;
                }
            }
            else
                newRow = SpawnRaw(tilePosition.z, tileIndex);
            
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

    private GameObject SpawnRaw(float posZ, int tileIndex, int obstaclePos = -2)
    {
        var row = new GameObject();

        row.transform.position = new Vector3(0f, 0f, posZ);
        row.transform.rotation = transform.rotation;

        if (obstaclePos != 0)
            Instantiate(tilesPrefabs[tileIndex], new Vector3(0, 0, posZ), transform.rotation, row.transform);
        if (obstaclePos != -1)
            Instantiate(tilesPrefabs[tileIndex], new Vector3(-tileLength, 0, posZ), transform.rotation, row.transform);
        if (obstaclePos != 1)
            Instantiate(tilesPrefabs[tileIndex], new Vector3(tileLength, 0, posZ), transform.rotation, row.transform);

        return row;
    }
    
    private int SpawnDownObstacle(GameObject prefab, float posZ, GameObject parent, int prevPos = -2)
    {
        int pos = prevPos < -1 || prevPos > 1? Random.Range(-1, 1 + 1) : prevPos;

        if (prevPos == -2 && pos == lastDownObstaclePos)
            pos = lastDownObstaclePos >= 0 ? -1 : 1;
        Instantiate(prefab, new Vector3(tileLength * pos, -1f, posZ), transform.rotation, parent.transform);
        return pos;
    }

    private int SpawnUpObstacle(float posZ, GameObject parent, int prevPos = -2)
    {
        int pos = prevPos < -1 || prevPos > 1? Random.Range(-1, 1 + 1) : prevPos;
        
        Instantiate(upObstaclePrefab, new Vector3(tileLength * pos, 3f, posZ), transform.rotation, parent.transform);
        return pos;
    }

    private void DeleteAndDestroyTile()
    {
        Destroy(presentGrounds[0]);
        presentGrounds.RemoveAt(0);
    }
}
