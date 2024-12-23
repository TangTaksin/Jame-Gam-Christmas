using UnityEngine;

public class BossShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform player; // Reference to the player's transform

    // Call this function to fire a projectile
    public void FireProjectile()
    {
        // Instantiate the projectile at the boss's position
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Initialize the projectile to shoot towards the player
        projectile.GetComponent<Projectile>().Init(player);
    }
}
