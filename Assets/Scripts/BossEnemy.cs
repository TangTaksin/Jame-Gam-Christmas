using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private Transform playerTarget; // Reference to the player
    private NavMeshAgent agent;

    // Attack variables
    [SerializeField] private float meleeAttackRange = 2f; // Range for melee attacks
    [SerializeField] private float rangedAttackRange = 5f; // Range for ranged attacks
    [SerializeField] private float attackCooldown = 2f; // Cooldown time between attacks
    private float timeSinceLastAttack = 0f; // Timer for cooldown

    // Projectile Prefab
    [SerializeField] private GameObject projectilePrefab; // Reference to the projectile prefab
    [SerializeField] private Transform projectileSpawnPoint; // Point from where the projectile will spawn

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on " + gameObject.name);
            return;
        }

        agent.updateRotation = false; // Prevent automatic rotation
    }

    private void Update()
    {
        if (playerTarget == null)
        {
            Debug.LogWarning("Player target is not assigned!");
            return;
        }

        // Always set the destination to the player's position
        agent.SetDestination(playerTarget.position);

        // Lock rotation on all axes, only allowing Y-axis rotation
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // Update attack cooldown timer
        timeSinceLastAttack += Time.deltaTime;

        // Trigger attack based on player distance
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        // Check if enough time has passed since the last attack (cooldown)
        if (timeSinceLastAttack >= attackCooldown)
        {
            if (distanceToPlayer <= meleeAttackRange)
            {
                // Melee attack
                MeleeAttack();
            }
            else if (distanceToPlayer <= rangedAttackRange)
            {
                // Ranged attack
                RangedAttack();
            }

            // Reset the attack cooldown
            timeSinceLastAttack = 0f;
        }
    }

    private void MeleeAttack()
    {
        // Perform melee attack (e.g., play animation, deal damage)
        Debug.Log("Boss performs a melee attack!");
    }

    private void RangedAttack()
    {
        // Perform ranged attack by shooting a projectile
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            // Initialize the projectile to target the player
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Init(playerTarget); // Pass the player as the target
            }

            Debug.Log("Boss performs a ranged attack!");
        }
    }

    private void SpecialAttack()
    {
        // Perform a special attack (e.g., a powerful area attack or a slam)
        Debug.Log("Boss performs a special attack!");
    }

    // Draw gizmos in the Scene view to visualize the attack ranges
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange); // Draw melee attack range

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange); // Draw ranged attack range
    }
}
