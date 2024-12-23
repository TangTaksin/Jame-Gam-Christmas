using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float damage = 10f; // Damage dealt on hit
    public float lifetime = 5f; // Time before the projectile is destroyed

    private Transform playerTarget; // Reference to the player's position
    private bool isHoming = false; // Flag to determine if the projectile should home in

    private void Start()
    {
        // Destroy the projectile after a set time to avoid infinite objects
        Destroy(gameObject, lifetime);
    }

    public void Init(Transform target)
    {
        // Set the player as the target for homing
        playerTarget = target;
        isHoming = true;
    }

    private void Update()
    {
        if (isHoming && playerTarget != null)
        {
            // Get direction to the player
            Vector3 directionToPlayer = (playerTarget.position - transform.position).normalized;

            // Move the projectile in the direction of the player
            transform.position += directionToPlayer * speed * Time.deltaTime;

            // Rotation is no longer required, so we won't rotate the projectile.
            // If you want to keep the projectile facing its original direction, you can reset its rotation like this:
            transform.rotation = Quaternion.identity; // Keeps the original rotation fixed.
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Deal damage to the player (you can add a method in the player script)
            Debug.Log("Player hit by ranged attack!");
            // Example of applying damage to the player:
            // other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);

            // Destroy the projectile after it hits
            Destroy(gameObject);
        }

    }
}
