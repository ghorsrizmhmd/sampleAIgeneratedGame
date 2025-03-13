using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float health = 3f;
    private Vector3 targetPosition;
    private float changeDirectionDelay = 2f;
    private float nextDirectionChange;

    void Start()
    {
        SetNewTargetPosition();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f || Time.time >= nextDirectionChange)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomZ = Random.Range(-5f, 5f);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
        
        nextDirectionChange = Time.time + changeDirectionDelay;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
} 