using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public Button quitButton;

    void Start()
    {
        // Create the panel
        GameObject panelObj = new GameObject("GameOverPanel");
        panelObj.transform.SetParent(transform, false);
        
        // Add Image component for background
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        
        // Set panel size and position
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.anchoredPosition = Vector2.zero;

        // Create Game Over Text
        GameObject textObj = new GameObject("GameOverText");
        textObj.transform.SetParent(panelObj.transform, false);
        gameOverText = textObj.AddComponent<TextMeshProUGUI>();
        gameOverText.fontSize = 48;
        gameOverText.alignment = TextAlignmentOptions.Center;
        gameOverText.rectTransform.anchorMin = new Vector2(0.5f, 0.7f);
        gameOverText.rectTransform.anchorMax = new Vector2(0.5f, 0.7f);
        gameOverText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        gameOverText.rectTransform.anchoredPosition = Vector2.zero;
        gameOverText.rectTransform.sizeDelta = new Vector2(400, 100);

        // Create Restart Button
        GameObject restartObj = new GameObject("RestartButton");
        restartObj.transform.SetParent(panelObj.transform, false);
        restartButton = restartObj.AddComponent<Button>();
        Image restartImage = restartObj.AddComponent<Image>();
        restartImage.color = new Color(0.2f, 0.8f, 0.2f);
        
        // Create text for restart button
        GameObject restartTextObj = new GameObject("Text");
        restartTextObj.transform.SetParent(restartObj.transform, false);
        TextMeshProUGUI restartText = restartTextObj.AddComponent<TextMeshProUGUI>();
        restartText.text = "Restart";
        restartText.fontSize = 24;
        restartText.alignment = TextAlignmentOptions.Center;
        restartText.rectTransform.anchorMin = Vector2.zero;
        restartText.rectTransform.anchorMax = Vector2.one;
        restartText.rectTransform.sizeDelta = Vector2.zero;
        restartText.rectTransform.anchoredPosition = Vector2.zero;
        
        restartButton.onClick.AddListener(RestartGame);
        
        // Set restart button position
        RectTransform restartRect = restartObj.GetComponent<RectTransform>();
        restartRect.anchorMin = new Vector2(0.5f, 0.4f);
        restartRect.anchorMax = new Vector2(0.5f, 0.4f);
        restartRect.pivot = new Vector2(0.5f, 0.5f);
        restartRect.anchoredPosition = Vector2.zero;
        restartRect.sizeDelta = new Vector2(200, 50);

        // Create Quit Button
        GameObject quitObj = new GameObject("QuitButton");
        quitObj.transform.SetParent(panelObj.transform, false);
        quitButton = quitObj.AddComponent<Button>();
        Image quitImage = quitObj.AddComponent<Image>();
        quitImage.color = new Color(0.8f, 0.2f, 0.2f);
        
        // Create text for quit button
        GameObject quitTextObj = new GameObject("Text");
        quitTextObj.transform.SetParent(quitObj.transform, false);
        TextMeshProUGUI quitText = quitTextObj.AddComponent<TextMeshProUGUI>();
        quitText.text = "Quit";
        quitText.fontSize = 24;
        quitText.alignment = TextAlignmentOptions.Center;
        quitText.rectTransform.anchorMin = Vector2.zero;
        quitText.rectTransform.anchorMax = Vector2.one;
        quitText.rectTransform.sizeDelta = Vector2.zero;
        quitText.rectTransform.anchoredPosition = Vector2.zero;
        
        quitButton.onClick.AddListener(QuitGame);
        
        // Set quit button position
        RectTransform quitRect = quitObj.GetComponent<RectTransform>();
        quitRect.anchorMin = new Vector2(0.5f, 0.3f);
        quitRect.anchorMax = new Vector2(0.5f, 0.3f);
        quitRect.pivot = new Vector2(0.5f, 0.5f);
        quitRect.anchoredPosition = Vector2.zero;
        quitRect.sizeDelta = new Vector2(200, 50);
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 