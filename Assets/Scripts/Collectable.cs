using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float lightValue = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
            player.Collect(lightValue);
        Destroy(gameObject);
    }
}
