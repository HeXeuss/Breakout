using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    
    private static GameManager instance;
    public static GameManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion
    
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public bool IsGameStarted { get; set; }
    public int lives { get; set; }
    public int availableLives = 3;
    public static event Action<int> OnLivesLost;
    
    private void Start()
    {
        lives = availableLives;
        Screen.SetResolution(1920, 1080, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnDestruction += OnDestruction;
    }

    private void OnDestruction(Brick brick)
    {
        if (BricksManager.Instance.remainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }

 
    
    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.lives--;

            if (this.lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLivesLost?.Invoke(lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BricksManager.Instance.LoadLevel(BricksManager.Instance.currentLevel);
            }
        }
    }
    
    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }
    
    
}
