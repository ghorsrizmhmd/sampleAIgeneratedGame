using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float gameTime = 60f; // 60 seconds to collect all coins
    private float currentTime;
    private bool isGameOver = false;
    private TextMeshProUGUI timerText;
    private TextMeshProUGUI scoreText;
    private PlayerController player;
    private GameOverPanel gameOverPanel;

    void Awake()
    {
        // Create the main game object
        GameObject gameObject = new GameObject("Game");
        
        // Add all necessary components
        gameObject.AddComponent<GameSetup>();
        gameObject.AddComponent<GameUI>();
    }

    void Start()
    {
        // Find the player
        player = FindFirstObjectByType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("Player not found in scene!");
            return;
        }

        // Find the game over panel
        gameOverPanel = FindFirstObjectByType<GameOverPanel>();
        if (gameOverPanel == null)
        {
            Debug.LogError("Game Over Panel not found in scene!");
            return;
        }

        // Initialize game state
        isGameOver = false;
        gameOverPanel.gameObject.SetActive(false);
        UpdateScore(0);

        currentTime = gameTime;
        SetupUI();
    }

    void Update()
    {
        if (!isGameOver)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();

            if (currentTime <= 0)
            {
                GameOver(false);
            }
        }
    }

    void SetupUI()
    {
        // Create Canvas if it doesn't exist
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create Timer Text
        GameObject timerObj = new GameObject("TimerText");
        timerObj.transform.SetParent(canvas.transform, false);
        timerText = timerObj.AddComponent<TextMeshProUGUI>();
        timerText.fontSize = 24;
        timerText.alignment = TextAlignmentOptions.TopRight;
        timerText.rectTransform.anchorMin = new Vector2(1, 1);
        timerText.rectTransform.anchorMax = new Vector2(1, 1);
        timerText.rectTransform.pivot = new Vector2(1, 1);
        timerText.rectTransform.anchoredPosition = new Vector2(-20, -20);
        timerText.rectTransform.sizeDelta = new Vector2(200, 50);

        // Create Score Text
        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(canvas.transform, false);
        scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        scoreText.fontSize = 24;
        scoreText.alignment = TextAlignmentOptions.TopLeft;
        scoreText.rectTransform.anchorMin = new Vector2(0, 1);
        scoreText.rectTransform.anchorMax = new Vector2(0, 1);
        scoreText.rectTransform.pivot = new Vector2(0, 1);
        scoreText.rectTransform.anchoredPosition = new Vector2(20, -20);
        scoreText.rectTransform.sizeDelta = new Vector2(200, 50);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void GameOver(bool won)
    {
        isGameOver = true;
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.gameOverText.text = won ? "You Win!" : "Time's Up!";
        gameOverPanel.gameOverText.color = won ? Color.green : Color.red;
    }
} 