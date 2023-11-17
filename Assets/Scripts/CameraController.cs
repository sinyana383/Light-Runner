using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 distToPlayer;
    
    void Start()
    {
        distToPlayer = transform.position - player.position;
    }
    
    void FixedUpdate()
    {
        var cameraPos = transform.position;
        Vector3 newPosition = new Vector3(cameraPos.x, cameraPos.y, distToPlayer.z + player.position.z);

        transform.position = newPosition;
    }
}
