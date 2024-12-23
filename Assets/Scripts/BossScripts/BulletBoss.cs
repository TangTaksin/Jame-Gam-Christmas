using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float damage = 10f; // Damage dealt on hit
    public float lifetime = 5f; // Time before the projectile is destroyed
    public Transform playerTarget; // Reference to the player's position

    private Rigidbody2D rb; // Reference to the Rigidbody2D for the bullet
    private Vector2 direction; // Direction to move in

    private void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing!");
        }

        // Destroy the projectile after a set time to avoid infinite objects
        Destroy(gameObject, lifetime);
    }

    // Call this method when the bullet is instantiated
    public void Init(Transform target)
    {
        if (target == null)
        {
            Debug.LogError("Target is null!");
            return;
        }

        // Calculate direction toward the target's position at the time of firing
        direction = (target.position - transform.position).normalized;

        // Optionally, rotate the bullet to face the target (for visual effect)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Apply force to the bullet (not the target)
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player
            Debug.Log("Player hit by ranged attack!");

            // Example of applying damage to the player:
            // other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

            // Destroy the projectile after it hits
            Destroy(gameObject);
        }
    }
}
