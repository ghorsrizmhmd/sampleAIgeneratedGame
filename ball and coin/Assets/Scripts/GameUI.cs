using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private PlayerController player;

    void Start()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // Create Score Text
        GameObject textObj = new GameObject("ScoreText");
        textObj.transform.SetParent(canvasObj.transform, false);
        scoreText = textObj.AddComponent<TextMeshProUGUI>();
        scoreText.fontSize = 24;
        scoreText.alignment = TextAlignmentOptions.TopLeft;
        scoreText.rectTransform.anchorMin = new Vector2(0, 1);
        scoreText.rectTransform.anchorMax = new Vector2(1, 1);
        scoreText.rectTransform.pivot = new Vector2(0, 1);
        scoreText.rectTransform.anchoredPosition = new Vector2(20, -20);
        scoreText.rectTransform.sizeDelta = new Vector2(200, 50);

        // Find the player
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (player != null)
        {
            scoreText.text = "Score: " + player.GetScore();
        }
    }
} 