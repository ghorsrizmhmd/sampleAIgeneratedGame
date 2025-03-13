using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1f;
    public float lifetime = 2f;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        // Destroy bullet after lifetime
        if (Time.time - startTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an enemy
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        
        // Destroy the bullet on any collision
        Destroy(gameObject);
    }
} 