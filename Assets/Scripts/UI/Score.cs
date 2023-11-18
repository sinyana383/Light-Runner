using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text scoreText;
    public int score;

    // Update is called once per frame
    void Update()
    {
        score = (int)player.position.z / 10;
        scoreText.text = score.ToString(CultureInfo.InvariantCulture);
    }
}
