using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Gamemanager2 : MonoBehaviour
{
    public static Gamemanager2 GameInstance { get; private set; }
    public bool isPaused;

    public event Action OnWin;
    public event Action OnLose;

    private void Awake()
    {
        if (GameInstance == null)
        {
            GameInstance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GameOverWin()
    {
        Debug.Log("You win!");
        isPaused = true;
        Time.timeScale = 0;
        OnWin?.Invoke();
    }

    public void GameOverLose()
    {
        Debug.Log("You lose!");
        isPaused = true;
        Time.timeScale = 0;
        OnLose?.Invoke();
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        isPaused = false;

        

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

       
    }

    
}
