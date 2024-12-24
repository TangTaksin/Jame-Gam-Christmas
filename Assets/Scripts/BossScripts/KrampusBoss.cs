using UnityEngine;
using UnityEngine.AI;

public class KrampusBoss : MonoBehaviour
{
    [Header("Boss References")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private Transform[] minionSpawnPoints;
    [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private Health health;

    [Header("Attack Parameters")]
    [SerializeField] private float meleeAttackRange = 2f;
    [SerializeField] private float rangedAttackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float secondAttackCooldown = 5f;  // Added cooldown for the second attack
    [SerializeField] private float minionSummonCooldown = 5f;

    private float timeSinceLastAttack = 0f;
    private float timeSinceLastMinionSummon = 0f;
    private float timeSinceLastSecondAttack = 0f;  // Timer for second attack

    [Header("Phase Thresholds")]
    [SerializeField] private float phase2Threshold = 70;
    [SerializeField] private float phase3Threshold = 50;

    private enum BossPhase { Phase1, Phase2, Phase3 }
    private BossPhase currentPhase = BossPhase.Phase1;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        if (!agent || !health || !animator)
        {
            Debug.LogError("Required components are missing.");
            return;
        }

        Health.OnHealthChange += HandleHealthChange;
        Health.OnHealthOut += HandleDeath;
    }

    private void Update()
    {
        if (playerTarget == null) return;

        // Lock rotation on all axes, only allowing Y-axis rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Move towards player
        MoveTowardsPlayer();

        // Update timers
        timeSinceLastAttack += Time.deltaTime;
        timeSinceLastSecondAttack += Time.deltaTime;  // Update second attack timer
        timeSinceLastMinionSummon += Time.deltaTime;

        // Handle phases and attacks
        HandlePhases();
    }

    private void MoveTowardsPlayer()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.SetDestination(playerTarget.position);
        }
    }

    private void HandlePhases()
    {
        float healthPercentage = (float)health.current_health / health.max_health * 100;

        // Phase transition logic
        if (healthPercentage <= phase3Threshold && currentPhase != BossPhase.Phase3) EnterPhase3();
        else if (healthPercentage <= phase2Threshold && currentPhase != BossPhase.Phase2) EnterPhase2();
        else if (currentPhase != BossPhase.Phase1) EnterPhase1();

        // Execute attack pattern
        ExecuteAttackPattern();
    }

    private void ExecuteAttackPattern()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

        if (timeSinceLastAttack >= attackCooldown)
        {
            if (distanceToPlayer <= meleeAttackRange)
                PerformMeleeAttack();
            else if (distanceToPlayer <= rangedAttackRange)
                PerformRangedAttack();

            timeSinceLastAttack = 0f;
        }

        // Perform second attack after cooldown
        if (timeSinceLastSecondAttack >= secondAttackCooldown)
        {
            PerformSecondAttack();
            timeSinceLastSecondAttack = 0f; // Reset second attack timer
        }

        // Summon minions in Phase 2
        if (currentPhase == BossPhase.Phase2 && timeSinceLastMinionSummon >= minionSummonCooldown)
        {
            SummonMinions();
            timeSinceLastMinionSummon = 0f;
        }
    }

    private void EnterPhase1()
    {
        currentPhase = BossPhase.Phase1;
        // Reset or special effects for Phase 1 if needed
    }

    private void EnterPhase2()
    {
        currentPhase = BossPhase.Phase2;
        timeSinceLastMinionSummon = 0f;  // Reset minion summon cooldown
        // Phase 2 effects
    }

    private void EnterPhase3()
    {
        currentPhase = BossPhase.Phase3;
        agent.speed += 2f;
        attackCooldown -= 0.5f;
        // Phase 3 effects (e.g., damage boost, visual cues)
    }

    private void PerformMeleeAttack()
    {
        TriggerAttackAnimation("MeleeAttack");

        // Find player and apply damage
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            playerHealth?.DamageHealth(25);  // Adjust damage as needed
        }
    }

    private void PerformRangedAttack()
    {
        TriggerAttackAnimation("RangedAttack");

        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Vector3 direction = (playerTarget.position - projectileSpawnPoint.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript?.Init(playerTarget);
        }
    }

    private void PerformSecondAttack()
    {
        // Perform second attack logic here
        Debug.Log("Krampus performs a second attack!");

        // Example: A ground slam or special attack
        TriggerAttackAnimation("SecondAttack");

        // If the second attack should deal damage or have special effects
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            playerHealth?.DamageHealth(50);  // Adjust damage as needed for the second attack
        }
    }

    private void TriggerAttackAnimation(string attackType)
    {
        if (animator != null)
        {
            animator.SetTrigger(attackType);
        }
    }

    private void SummonMinions()
    {
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
        // Handle health changes, such as updating UI or playing sound effects
    }

    private void HandleDeath(int remainingHealth)
    {
        if (remainingHealth <= 0)
        {
            if (animator != null)
            {
                animator.SetTrigger("Death");
            }
            Destroy(gameObject);  // Destroy the boss after death
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
    }
}
