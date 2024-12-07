using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BricksManager : MonoBehaviour
{
    #region Singleton
    
    private static BricksManager instance;
    public static BricksManager Instance => instance;

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
    
    public static event Action onLevelLoaded;
    public int InitialBricksCount { set; get; }
    public float shiftAmount = 1f;
    public Color[] brickColors;
    public Brick brickPrefab;
    public Sprite[] brickSprites;
    private GameObject bricksContainer;
    public List<Brick> remainingBricks { get; set; }
    public List<int[,]> LevelData { get; set; }
    private int maxRow = 9;
    private int maxCol = 23;
    public int currentLevel;
    private float initialBrickSpawnPosX = -11f;
    private float initialBrickSpawnPosY = 4.8f;
    void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        this.LevelData = this.LoadLevelData();
        this.GenerateBricks();

    }

    private void GenerateBricks()
    {
        this.remainingBricks = new List<Brick>();
        int[,] currentLevelData = this.LevelData[this.currentLevel];
        float currentSpawnX = initialBrickSpawnPosX;
        float currentSpawnY = initialBrickSpawnPosY;
        float zShift = 0;

        for (int row = 0; row < this.maxRow; row++)
        {
            for (int col = 0; col < this.maxCol; col++)
            {
                int brickType = currentLevelData[row, col];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.brickSprites[brickType - 1], this.brickColors[brickType - 1], brickType);
                    this.remainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                    
                }

                currentSpawnX += shiftAmount;
                if (col + 1 == this.maxCol)
                {
                    currentSpawnX = initialBrickSpawnPosX;
                }
            }
            currentSpawnY -= shiftAmount;
        }

        this.InitialBricksCount = this.remainingBricks.Count;
        onLevelLoaded?.Invoke();
    }
    private List<int[,]> LoadLevelData()
    {
        TextAsset textAsset = Resources.Load("Level" + LevelMenu.levelNumber) as TextAsset;
        string[] rows = textAsset.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        
        List<int[,]> levelData = new List<int[,]>();
        int[,] currentLevelData = new int[maxRow , maxCol ];
        int curentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];
            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevelData[curentRow, col] = int.Parse(bricks[col]);
                }

                curentRow++;
            }
            else
            {
                curentRow = 0;
                levelData.Add(currentLevelData);
                currentLevelData = new int[maxRow, maxCol];
            }
        }
        return levelData;
    }

    public void LoadNextLevel()
    {
        this.currentLevel++;

        if (this.currentLevel >= this.LevelData.Count)
        {
            GameManager.Instance.ShowVictoryScreen();
        }
        else
        {
            this.LoadLevel(this.currentLevel);
        }
    }

    public void LoadLevel(int level)
    {
        this.currentLevel = level;
        this.Clearbricks();
        this.GenerateBricks();
    }

    private void Clearbricks()
    {
        foreach (Brick brick in this.remainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }
}
