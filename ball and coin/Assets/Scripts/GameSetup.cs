using UnityEngine;
using System.Collections.Generic;

public class GameSetup : MonoBehaviour
{
    public int numberOfCoins = 10;
    public float groundSize = 10f;
    public Color groundColor = new Color(0.2f, 0.8f, 0.2f); // Green color for ground
    public float wallHeight = 1.5f;
    public float wallThickness = 0.5f;
    public Color wallColor = new Color(0.8f, 0.2f, 0.2f); // Red color for walls
    public int gridSize = 15;
    public int numberOfEnemies = 3; // Number of enemies to spawn
    public float enemySpeed = 3f; // Speed of enemies
    public float enemyHealth = 3f; // Health of enemies
    public Color playerColor = new Color(0.2f, 0.4f, 1f); // Bright blue for player
    public Color coinColor = new Color(1f, 0.8f, 0f); // Shiny gold for coins
    public Color enemyColor = new Color(0.8f, 0.2f, 0.2f); // Bright red for enemies
    public Color bulletColor = new Color(1f, 0.5f, 0f); // Orange for bullets
    public float ambientLightIntensity = 0.5f;
    public Color ambientLightColor = new Color(0.5f, 0.5f, 0.5f);

    private bool[,] mazeGrid; // true = wall, false = path

    void Start()
    {
        // Create Ground tag if it doesn't exist
        if (!DoesTagExist("Ground"))
        {
            CreateTag("Ground");
        }

        // Create or get Camera
        Camera mainCamera;
        if (Camera.main == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.tag = "MainCamera";
        }
        else
        {
            mainCamera = Camera.main;
        }

        // Initialize maze grid
        mazeGrid = new bool[gridSize, gridSize];
        GenerateMaze();

        // Setup lighting
        SetupLighting();

        // Create Ground with improved visuals
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.tag = "Ground";
        ground.transform.localScale = new Vector3(groundSize, 1, groundSize);
        
        // Add material to ground with improved visuals
        Renderer groundRenderer = ground.GetComponent<Renderer>();
        Material groundMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        groundMaterial.color = groundColor;
        groundMaterial.SetFloat("_Metallic", 0.1f);
        groundMaterial.SetFloat("_Smoothness", 0.3f);
        groundMaterial.EnableKeyword("_NORMALMAP");
        groundMaterial.EnableKeyword("_METALLICSPECGLOSSMAP");
        groundRenderer.material = groundMaterial;
        
        // Add Rigidbody to ground (kinematic)
        Rigidbody groundRb = ground.AddComponent<Rigidbody>();
        groundRb.isKinematic = true;
        groundRb.useGravity = false;

        // Create Maze Walls with improved visuals
        CreateMazeWalls();

        // Create Player with improved visuals
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0, 1, 0);
        player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        // Add Rigidbody to Player
        Rigidbody playerRb = player.AddComponent<Rigidbody>();
        playerRb.useGravity = true;
        playerRb.mass = 1f;
        playerRb.linearDamping = 1f;
        playerRb.angularDamping = 0.05f;
        playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        
        // Add PlayerController script
        player.AddComponent<PlayerController>();

        // Add player material with improved visuals
        Renderer playerRenderer = player.GetComponent<Renderer>();
        Material playerMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        playerMaterial.color = playerColor;
        playerMaterial.SetFloat("_Metallic", 0.8f);
        playerMaterial.SetFloat("_Smoothness", 0.8f);
        playerMaterial.EnableKeyword("_EMISSION");
        playerMaterial.SetColor("_EmissionColor", playerColor * 0.5f);
        playerRenderer.material = playerMaterial;

        // Create Coins with improved visuals
        CreateCoins();

        // Create Enemies with improved visuals
        CreateEnemies();

        // Setup Camera with improved visuals
        SetupCamera(mainCamera);
    }

    void GenerateMaze()
    {
        // Initialize all cells as walls
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                mazeGrid[x, z] = true;
            }
        }

        // Start from the center
        int startX = gridSize / 2;
        int startZ = gridSize / 2;
        mazeGrid[startX, startZ] = false;

        // Create paths using recursive backtracking
        CreatePath(startX, startZ);

        // Ensure the edges are walls
        for (int x = 0; x < gridSize; x++)
        {
            mazeGrid[x, 0] = true;
            mazeGrid[x, gridSize - 1] = true;
            mazeGrid[0, x] = true;
            mazeGrid[gridSize - 1, x] = true;
        }
    }

    void CreatePath(int x, int z)
    {
        // Define possible directions (up, right, down, left)
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, -2), // up
            new Vector2Int(2, 0),  // right
            new Vector2Int(0, 2),  // down
            new Vector2Int(-2, 0)  // left
        };

        // Shuffle directions
        for (int i = directions.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2Int temp = directions[i];
            directions[i] = directions[j];
            directions[j] = temp;
        }

        // Try each direction
        foreach (Vector2Int dir in directions)
        {
            int newX = x + dir.x;
            int newZ = z + dir.y;

            // Check if the new position is valid and not visited
            if (newX >= 0 && newX < gridSize && newZ >= 0 && newZ < gridSize && mazeGrid[newX, newZ])
            {
                // Create path by removing walls
                mazeGrid[x + dir.x/2, z + dir.y/2] = false;
                mazeGrid[newX, newZ] = false;
                CreatePath(newX, newZ);
            }
        }
    }

    void CreateMazeWalls()
    {
        // Create wall material
        Material wallMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        wallMaterial.color = wallColor;
        wallMaterial.SetFloat("_Metallic", 0.1f);
        wallMaterial.SetFloat("_Smoothness", 0.1f);

        float cellSize = groundSize / gridSize;

        // Create walls based on the maze grid
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (mazeGrid[x, z])
                {
                    float posX = (x - gridSize/2) * cellSize + cellSize/2;
                    float posZ = (z - gridSize/2) * cellSize + cellSize/2;
                    CreateWall(
                        new Vector3(posX, wallHeight/2, posZ),
                        new Vector3(cellSize * wallThickness, wallHeight, cellSize * wallThickness),
                        wallMaterial
                    );
                }
            }
        }
    }

    void CreateWall(Vector3 position, Vector3 scale, Material material)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall";
        wall.transform.position = position;
        wall.transform.localScale = scale;
        
        // Add material with improved visuals
        Renderer wallRenderer = wall.GetComponent<Renderer>();
        Material wallMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        wallMaterial.color = wallColor;
        wallMaterial.SetFloat("_Metallic", 0.3f);
        wallMaterial.SetFloat("_Smoothness", 0.4f);
        wallMaterial.EnableKeyword("_NORMALMAP");
        wallRenderer.material = wallMaterial;
        
        // Add Rigidbody (kinematic)
        Rigidbody wallRb = wall.AddComponent<Rigidbody>();
        wallRb.isKinematic = true;
        wallRb.useGravity = false;

        // Add Box Collider with adjusted size
        BoxCollider wallCollider = wall.GetComponent<BoxCollider>();
        wallCollider.size = new Vector3(1.2f, 1.2f, 1.2f);
    }

    void CreateCoins()
    {
        float cellSize = groundSize / gridSize;
        List<Vector3> validPositions = new List<Vector3>();

        // Find all valid positions (empty cells) in the maze
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (!mazeGrid[x, z])
                {
                    float posX = (x - gridSize/2) * cellSize + cellSize/2;
                    float posZ = (z - gridSize/2) * cellSize + cellSize/2;
                    validPositions.Add(new Vector3(posX, 0.5f, posZ));
                }
            }
        }

        // Place coins at random valid positions
        for (int i = 0; i < numberOfCoins && validPositions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3 position = validPositions[randomIndex];
            validPositions.RemoveAt(randomIndex);

            GameObject coin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            coin.name = "Coin" + i;
            coin.tag = "Coin";
            coin.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            coin.transform.position = position;
            
            // Add Rigidbody to Coin
            Rigidbody coinRb = coin.AddComponent<Rigidbody>();
            coinRb.useGravity = false;
            coinRb.isKinematic = true;
            
            // Add Collider and set as trigger
            SphereCollider coinCollider = coin.GetComponent<SphereCollider>();
            coinCollider.isTrigger = true;
            
            // Add yellow material
            Renderer coinRenderer = coin.GetComponent<Renderer>();
            Material coinMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            coinMaterial.color = coinColor;
            coinMaterial.SetFloat("_Metallic", 0.9f);
            coinMaterial.SetFloat("_Smoothness", 0.9f);
            coinMaterial.EnableKeyword("_EMISSION");
            coinMaterial.SetColor("_EmissionColor", coinColor * 0.5f);
            coinRenderer.material = coinMaterial;

            // Add rotation animation
            coin.AddComponent<CoinRotation>();
        }
    }

    void CreateEnemies()
    {
        float cellSize = groundSize / gridSize;
        List<Vector3> validPositions = new List<Vector3>();

        // Find all valid positions (empty cells) in the maze
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (!mazeGrid[x, z])
                {
                    float posX = (x - gridSize/2) * cellSize + cellSize/2;
                    float posZ = (z - gridSize/2) * cellSize + cellSize/2;
                    validPositions.Add(new Vector3(posX, 0.5f, posZ));
                }
            }
        }

        // Place enemies at random valid positions
        for (int i = 0; i < numberOfEnemies && validPositions.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3 position = validPositions[randomIndex];
            validPositions.RemoveAt(randomIndex);

            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            enemy.name = "Enemy" + i;
            enemy.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            enemy.transform.position = position;
            
            // Add Rigidbody to Enemy
            Rigidbody enemyRb = enemy.AddComponent<Rigidbody>();
            enemyRb.useGravity = true;
            enemyRb.mass = 1f;
            enemyRb.linearDamping = 1f;
            enemyRb.angularDamping = 0.05f;
            enemyRb.constraints = RigidbodyConstraints.FreezeRotation;
            
            // Add Enemy script
            Enemy enemyScript = enemy.AddComponent<Enemy>();
            enemyScript.speed = enemySpeed;
            enemyScript.health = enemyHealth;
            
            // Add material
            Renderer enemyRenderer = enemy.GetComponent<Renderer>();
            Material enemyMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            enemyMaterial.color = enemyColor;
            enemyMaterial.SetFloat("_Metallic", 0.5f);
            enemyMaterial.SetFloat("_Smoothness", 0.6f);
            enemyMaterial.EnableKeyword("_EMISSION");
            enemyMaterial.SetColor("_EmissionColor", enemyColor * 0.3f);
            enemyRenderer.material = enemyMaterial;
        }
    }

    private bool DoesTagExist(string tag)
    {
        try
        {
            GameObject.FindGameObjectWithTag(tag);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void CreateTag(string tag)
    {
        #if UNITY_EDITOR
        var asset = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0];
        var serializedObject = new UnityEditor.SerializedObject(asset);
        var tags = serializedObject.FindProperty("tags");
        tags.InsertArrayElementAtIndex(tags.arraySize);
        tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
        serializedObject.ApplyModifiedProperties();
        #endif
    }

    void SetupCamera(Camera mainCamera)
    {
        // Position the camera directly above the ground
        mainCamera.transform.position = new Vector3(0, 15, 0);
        mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        
        // Set camera background color to a nice sky blue
        mainCamera.backgroundColor = new Color(0.4f, 0.6f, 0.8f, 1f);
        
        // Set camera properties for better visuals
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.orthographic = true;
        mainCamera.orthographicSize = groundSize / 2;
    }

    void SetupLighting()
    {
        // Create main directional light
        GameObject mainLight = new GameObject("Main Light");
        Light directionalLight = mainLight.AddComponent<Light>();
        directionalLight.type = LightType.Directional;
        directionalLight.intensity = 1.2f;
        directionalLight.color = Color.white;
        directionalLight.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        directionalLight.shadows = LightShadows.Soft;
        directionalLight.shadowStrength = 0.8f;
        directionalLight.shadowBias = 0.05f;
        directionalLight.shadowNormalBias = 0.4f;

        // Setup ambient light
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambientLightColor;
        RenderSettings.ambientIntensity = ambientLightIntensity;
    }
} 