using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private Image[] fires;
    [SerializeField] private Sprite noFire;
    [SerializeField] private Sprite fire;
    
    void Start()
    {
        for (int i = 0; i < fires.Length / 2; i++)
        {
            fires[i].sprite = fire;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
