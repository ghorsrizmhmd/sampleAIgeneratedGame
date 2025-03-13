using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Awake()
    {
        // Create the main game object
        GameObject gameObject = new GameObject("Game");
        
        // Add all necessary components
        gameObject.AddComponent<GameSetup>();
        gameObject.AddComponent<GameUI>();
    }
} 