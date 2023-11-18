using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public bool IsPaused;
    [SerializeField] private GameObject startMenu;
    
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text record;
    
    [SerializeField] private int recordInt = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Record"))
        {
            PlayerPrefs.SetInt("Record", 0);
            PlayerPrefs.Save();
        }
        else
        {
            recordInt = PlayerPrefs.GetInt("Record");
            record.text = recordInt.ToString();
        }
        
        if (!PlayerPrefs.HasKey("NeedStartMenu"))
        {
            PlayerPrefs.SetInt("NeedStartMenu", 1);
            PlayerPrefs.Save();
        }
        
        if (PlayerPrefs.GetInt("NeedStartMenu") == 1)
            PauseGame(startMenu);
        else
            Time.timeScale = 1f;
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
        int needStartMenu = startWithMenu ? 1 : 0;
        
        PlayerPrefs.SetInt("NeedStartMenu", needStartMenu);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }

    public void OnGameOver()
    {
        PauseGame(gameOver);
        var scoreComp = GetComponent<Score>();

        score.text = scoreComp.score.ToString();
        record.color = Color.white;
        if (recordInt < scoreComp.score)
        {
            record.text = score.text;
            record.color = Color.yellow;
            
            PlayerPrefs.SetInt("Record", scoreComp.score);
            PlayerPrefs.Save();
        }
    }
}
