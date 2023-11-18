using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PenalsManager : MonoBehaviour
{
    static public bool IsPaused;
    [SerializeField] private GameObject startMenu;
    
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
}
