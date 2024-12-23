using UnityEngine;
using UnityEngine.AI;

public class KrampusBoss : MonoBehaviour
{
    [Header("Boss References")]
    [SerializeField] private Transform playerTarget; // Reference to the player
    [SerializeField] private GameObject projectilePrefab; // Projectile for ranged attacks
    [SerializeField] private Transform projectileSpawnPoint; // Spawn point for projectiles
    [SerializeField] private GameObject minionPrefab; // Minion prefab for summoning
    [SerializeField] private Transform[] minionSpawnPoints; // Spawn points for minions

    private NavMeshAgent agent;
    private Health health;

    [Header("Attack Parameters")]
    [SerializeField] private float meleeAttackRange = 2f;
    [SerializeField] private float rangedAttackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;

    private float timeSinceLastAttack = 0f;

    [Header("Phase Thresholds")]
    [SerializeField] private float phase2Threshold = 70; // Health percentage for Phase 2
    [SerializeField] private float phase3Threshold = 50; // Health percentage for Phase 3

    private enum BossPhase { Phase1, Phase2, Phase3 }
    private BossPhase currentPhase = BossPhase.Phase1;

    // Phase 2 Minion Summon Parameters
    private float minionSummonCooldown = 5f; // Time between minion spawns in Phase 2
    private float timeSinceLastMinionSummon = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing.");
        }

        if (health == null)
        {
            Debug.LogError("Health component is missing.");
        }

        // Subscribe to Health Events
        Health.OnHealthChange += HandleHealthChange;
        Health.OnHealthOut += HandleDeath;
    }

    private void Update()
    {
        if (playerTarget == null)
        {
            Debug.LogWarning("Player target not assigned!");
            return;
        }
          // Lock rotation on all axes, only allowing Y-axis rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);
        // Move towards the player
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(playerTarget.position);
        }

        // Update attack cooldown
        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastMinionSummon += Time.deltaTime;

        // Handle attack pattern and phases
        HandlePhases();
    }

    private void HandlePhases()
    {
        float healthPercentage = (float)health.current_health / health.max_health * 100;

        // Switch between phases
        if (healthPercentage <= phase3Threshold)
        {
            if (currentPhase != BossPhase.Phase3)
            {
                EnterPhase3();
            }
        }
        else if (healthPercentage <= phase2Threshold)
        {
            if (currentPhase != BossPhase.Phase2)
            {
                EnterPhase2();
            }
        }
        else
        {
            if (currentPhase != BossPhase.Phase1)
            {
                EnterPhase1();
            }
        }

        // Execute attack pattern
        ExecuteAttackPattern();
    }

    private void ExecuteAttackPattern()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (timeSinceLastAttack >= attackCooldown)
        {
            if (distanceToPlayer <= meleeAttackRange)
            {
                PerformMeleeAttack();
            }
            else if (distanceToPlayer <= rangedAttackRange)
            {
                PerformRangedAttack();
            }

            timeSinceLastAttack = 0f;
        }

        // Summon minions periodically in Phase 2
        if (currentPhase == BossPhase.Phase2 && timeSinceLastMinionSummon >= minionSummonCooldown)
        {
            SummonMinions();
            timeSinceLastMinionSummon = 0f; // Reset the timer after summoning
        }
    }

    private void EnterPhase1()
    {
        currentPhase = BossPhase.Phase1;
        Debug.Log("Entered Phase 1");
    }

    private void EnterPhase2()
    {
        currentPhase = BossPhase.Phase2;
        Debug.Log("Entered Phase 2");
    }

    private void EnterPhase3()
    {
        currentPhase = BossPhase.Phase3;
        Debug.Log("Entered Phase 3");
        // Buff stats for Phase 3
        agent.speed += 2f;
        attackCooldown -= 0.5f;
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Krampus performs a melee attack!");
        // Add melee attack animation and logic here
    }

    private void PerformRangedAttack()
    {
        Debug.Log("Krampus performs a ranged attack!");

        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Vector3 direction = (playerTarget.position - projectileSpawnPoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction); // Adjust rotation
            projectile.transform.rotation = rotation;

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Init(playerTarget); // Target the player
            }
        }
    }

    private void SummonMinions()
    {
        Debug.Log("Krampus summons minions!");

        foreach (Transform spawnPoint in minionSpawnPoints)
        {
            if (minionPrefab != null)
            {
                Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    private void HandleHealthChange(int currentHealth)
    {
        Debug.Log($"Health Changed: {currentHealth}");
    }

    private void HandleDeath(int remainingHealth)
    {
        Debug.Log("Krampus has been defeated!");

        // Trigger death animation or effect (if any)
        // Example: animator.SetTrigger("Death");
        
        Destroy(gameObject); // Destroy the boss after death
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
    }
}
