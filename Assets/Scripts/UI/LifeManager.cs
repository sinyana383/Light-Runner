using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private Image[] fires;
    [SerializeField] private Sprite noFire;
    [SerializeField] private Sprite fire;

    [SerializeField] private float rangePerFire = 10f;
    void Start()
    {
        for (int i = 0; i < fires.Length / 2; i++)
        {
            fires[i].sprite = fire;
        }
    }

    void OnEnable()
    {
        // Subscribe to the LightRangeChanged event
        PlayerController.OnLightRangeChanged += UpdateUI;
    }

    void OnDisable()
    {
        // Unsubscribe from the LightRangeChanged event
        PlayerController.OnLightRangeChanged -= UpdateUI;
    }

    void UpdateUI(float newRange)
    {
        int flamesCount = (int)(newRange / rangePerFire);

        int i = 0;
        for (; i < flamesCount; i++)
        {
            fires[i].sprite = fire;
        }
        for (; i < fires.Length; i++)
        {
            fires[i].sprite = noFire;
        }
    }
}
