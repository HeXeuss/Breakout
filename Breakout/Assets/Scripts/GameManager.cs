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

    public bool isPaused = false;
    [SerializeField] private GameObject pauseMenu;
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public bool IsGameStarted { get; set; }
    public int lives { get; set; }
    public int availableLives = 3;
    public static event Action<int> OnLivesLost;
    
    private void Start()
    {        
        
        lives = availableLives;
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnDestruction += OnDestruction;
    }

    private void OnDestroy()
    {
        isPaused = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !victoryScreen.activeSelf && !gameOverScreen.activeSelf)
        {
            if (pauseMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void LoadNextLevel()
    {
        LevelMenu.levelNumber++;
        if (LevelMenu.levelNumber > 20)
        {
            LevelMenu.levelNumber = 1;
        }
        SceneManager.LoadScene("Game");
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

    void UnlockNewLevel()
    {
        if (LevelMenu.levelNumber >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", LevelMenu.levelNumber + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
    
    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
        UnlockNewLevel();
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

    public void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    
    
}
