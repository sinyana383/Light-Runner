using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PenalsManager : MonoBehaviour
{
    static public bool IsPaused;
    [SerializeField] private GameObject startMenu;
    
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text record;
    
    // Start is called before the first frame update
    void Start()
    {
        PauseGame(startMenu);
    }

    public void PauseGame(GameObject penal)
    {
        penal.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void ResumeGame(GameObject penal)
    {
        penal.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Restart(bool startWithMenu)
    {
        SceneManager.LoadScene(0);
    }

    public void OnGameOver()
    {
        PauseGame(gameOver);
        var scoreComp = GetComponent<Score>();

        score.text = scoreComp.score.ToString();
        record.color = Color.white;
        if (int.TryParse(record.text, out var recordNum) &&
            recordNum < scoreComp.score)
        {
            record.text = score.text;
            record.color = Color.yellow;
        }
        // save results
    }
}
