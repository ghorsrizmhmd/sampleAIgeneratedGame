# Maze Ball Game

A 3D maze game built with Unity where you control a ball through a randomly generated maze and collect coins.

## Features

- Randomly generated maze
- Top-down view gameplay
- Collect coins to win
- Physics-based movement
- Dynamic lighting and materials

## Controls

- **WASD / Arrow Keys**: Move the ball
- **Space**: Jump
- **Mouse**: Look around (in top-down view)

## Gameplay

1. Navigate through the maze using WASD or arrow keys
2. Collect yellow coins to increase your score
3. Collect all coins to win the game

## Requirements

- Unity 2022.3 or later
- Universal Render Pipeline (URP)

## Installation

1. Clone this repository
2. Open the project in Unity Hub
3. Open the project with Unity 2022.3 or later
4. Open the `Assets/Scenes/MainScene.unity` scene
5. Press Play to start the game

## Project Structure

```
Assets/
├── Scripts/
│   ├── GameSetup.cs      # Handles game initialization and maze generation
│   ├── PlayerController.cs # Player movement mechanics
│   ├── CoinRotation.cs   # Coin animation
└── Scenes/
    └── MainScene.unity    # Main game scene
```

## Customization

You can adjust various game parameters in the Unity Inspector:

- `numberOfCoins`: Number of coins to collect
- `groundSize`: Size of the maze
- `wallHeight`: Height of maze walls

## Note

This project was generated using AI assistance. Some features may be experimental or in development.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. 