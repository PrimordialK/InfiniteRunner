//using UnityEngine;
//using TMPro;

//public class ScoreManager : MonoBehaviour
//{
//    public static int score;
//    public TMP_Text scoreText;

//    [SerializeField] private int scoreToWin = 10;
//    private bool hasWon = false;

//    void Start()
//    {
//        score = 0;
//        hasWon = false;
//        UpdateScoreUI();
//    }
//    #region Score Management
//    public void AddPoints(int points)
//    {
//        if (hasWon) return;

//        score += points;
//        UpdateScoreUI();
//        Debug.Log($"Score: {score}");

//        if (score >= scoreToWin)
//            TriggerWinScreen();
//    }

//    void UpdateScoreUI() // Update the score display in the UI
//    {
//        if (scoreText != null) 
//            scoreText.text = "Score: " + score.ToString(); // Update the score text in the UI
//        else
//            Debug.LogWarning("ScoreText not assigned in ScoreManager!");
//    }
//    #endregion
//    void TriggerWinScreen() // Call this method when the player reaches the win condition
//    {
//        hasWon = true;
//        Debug.Log("Win condition reached!");

//        MenuController menuController = FindObjectOfType<MenuController>(); // Find the MenuController in the scene
//        if (menuController != null)
//        {
//            menuController.JumpTo(MenuStates.WinScreen);
//        }
//        else
//        {
//            Debug.LogError("MenuController not found! Make sure it exists in the game scene.");
//        }
//    }

//}