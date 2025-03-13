using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float bulletSpeed = 10f;
    public float bulletDamage = 1f;
    public float fireRate = 0.5f; // Time between shots
    public float bulletLifetime = 2f;
    public Color bulletColor = new Color(1f, 0.5f, 0f); // Orange for bullets
    private Rigidbody rb;
    private int score = 0;
    private bool isGrounded;
    private GameManager gameManager;
    private Material playerMaterial;
    private float nextFireTime;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 1f;
        gameManager = FindObjectOfType<GameManager>();
        
        // Create a unique material for the player
        playerMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        playerMaterial.color = Color.blue;
        playerMaterial.SetFloat("_Metallic", 0.8f);
        playerMaterial.SetFloat("_Smoothness", 0.8f);
        playerMaterial.SetFloat("_Surface", 0); // 0 = Opaque
        playerMaterial.SetFloat("_Blend", 0); // 0 = Alpha
        playerMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        GetComponent<Renderer>().material = playerMaterial;

        mainCamera = Camera.main;
        rb.linearDamping = 1f;
        rb.angularDamping = 0.05f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Shooting
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Get mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        // Debug ray visualization
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            targetPoint = hit.point;
            Debug.Log($"Ray hit ground at: {targetPoint}");
        }
        else
        {
            // If no ground hit, use a point far away
            targetPoint = ray.GetPoint(100f);
            Debug.Log($"No ground hit, using point: {targetPoint}");
        }

        // Calculate direction to mouse position
        Vector3 direction = (targetPoint - transform.position).normalized;
        Debug.Log($"Shooting direction: {direction}");
        
        // Create bullet
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.name = "Bullet";
        bullet.transform.position = transform.position + direction * 2f; // Move bullet further from player
        bullet.transform.localScale = new Vector3(1f, 1f, 1f); // Make bullet much bigger
        
        // Add Rigidbody to Bullet
        Rigidbody bulletRb = bullet.AddComponent<Rigidbody>();
        bulletRb.useGravity = false;
        bulletRb.linearVelocity = direction * bulletSpeed;
        
        // Add Bullet script
        Bullet bulletScript = bullet.AddComponent<Bullet>();
        bulletScript.damage = bulletDamage;
        bulletScript.lifetime = bulletLifetime;
        
        // Add simple material
        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        Material bulletMaterial = new Material(Shader.Find("Standard")); // Use Standard shader instead of URP
        bulletMaterial.color = Color.red; // Use bright red for testing
        bulletMaterial.EnableKeyword("_EMISSION");
        bulletMaterial.SetColor("_EmissionColor", Color.red * 2f);
        bulletRenderer.material = bulletMaterial;

        Debug.Log($"Bullet created at position: {bullet.transform.position} with velocity: {bulletRb.linearVelocity}");
    }

    void OnCollisionStay(Collision collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
        catch
        {
            // Tag might not exist yet, ignore the collision
        }
    }

    void OnCollisionExit(Collision collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
        catch
        {
            // Tag might not exist yet, ignore the collision
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            // Collect the coin
            score += 1;
            Debug.Log("Score: " + score);
            
            // Visual feedback
            StartCoroutine(FlashPlayer());
            
            // Destroy the coin
            Destroy(other.gameObject);

            // Check if all coins are collected
            GameObject[] remainingCoins = GameObject.FindGameObjectsWithTag("Coin");
            if (remainingCoins.Length <= 1) // Only 1 coin remains (the one we're currently collecting)
            {
                Debug.Log("All coins collected! You win!");
                gameManager.GameOver(true);
            }
        }
    }

    System.Collections.IEnumerator FlashPlayer()
    {
        // Flash white when collecting coin
        playerMaterial.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        playerMaterial.color = Color.blue;
    }

    public int GetScore()
    {
        return score;
    }
} 