using System;
using Microsoft.Win32.SafeHandles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public TextMeshProUGUI levelText;
   public TextMeshProUGUI targetText;
   public TextMeshProUGUI livesText;
 
   public int score { get; set; }
   private void Start()
   {
      OnLivesLost(GameManager.Instance.availableLives);
   }

   public void Awake()
   {
      Brick.OnDestruction += OnDestruction;
      BricksManager.onLevelLoaded += OnLevelLoaded;
      GameManager.OnLivesLost += OnLivesLost;
   }

   private void OnLivesLost(int lives)
   {
      livesText.text = $"Lives: {lives}";
   }

   private void OnLevelLoaded()
   {
      UpdateRemainingBricksText();
      UpdateLevelText();
   }
   
   private void UpdateLevelText()
   {
      levelText.text = $@"LEVEL:{Environment.NewLine}{LevelMenu.levelNumber}";
   }

   private void OnDestruction(Brick obj)
   {
      UpdateRemainingBricksText();
   }

   private void UpdateRemainingBricksText()
   {
      targetText.text = $@"TARGET:{Environment.NewLine}{BricksManager.Instance.remainingBricks.Count} / {BricksManager.Instance.InitialBricksCount}";
   }

   private void OnDisable()
   {
      Brick.OnDestruction -= OnDestruction;
      BricksManager.onLevelLoaded -= OnLevelLoaded;
      GameManager.OnLivesLost -= OnLivesLost;
   }
}
