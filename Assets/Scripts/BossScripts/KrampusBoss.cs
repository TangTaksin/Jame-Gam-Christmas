using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Animator animator; // Reference to Animator

    private NavMeshAgent agent;
    private Health health;

    [Header("Attack Parameters")]
    [SerializeField] private float meleeAttackRange = 2f;
    [SerializeField] private float rangedAttackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;

    private float timeSinceLastAttack = 0f;

    [Header("Phase Thresholds")]
    [SerializeField] private float phase2Threshold = 70;
    [SerializeField] private float phase3Threshold = 50;

    private enum BossPhase { Phase1, Phase2, Phase3 }
    private BossPhase currentPhase = BossPhase.Phase1;

    // Phase 2 Minion Summon Parameters
    private float minionSummonCooldown = 5f;
    private float timeSinceLastMinionSummon = 0f;

    [Header("Visual Effects")]
    [SerializeField] private GameObject hitEffectPrefab;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();

        if (agent == null) Debug.LogError("NavMeshAgent component is missing.");
        if (health == null) Debug.LogError("Health component is missing.");
        if (animator == null) Debug.LogError("Animator component is missing.");

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

        // Update timers
        timeSinceLastAttack += Time.deltaTime;

        // Only update the minion summon timer in Phase 2
        if (currentPhase == BossPhase.Phase2)
        {
            timeSinceLastMinionSummon += Time.deltaTime;
        }

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
            else if (distanceToPlayer >= rangedAttackRange)
            {
                PerformRangedAttack();
            }

            timeSinceLastAttack = 0f;
        }

        // Summon minions only in Phase 2
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

        // Reset minion summon timer when entering Phase 2
        timeSinceLastMinionSummon = 0f;
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

        // Play melee attack animation
        if (animator != null)
        {
            animator.SetTrigger("MeleeAttack");
            Debug.Log("Triggering MeleeAttack animation.");
        }

        // Delay damage application to sync with animation
        StartCoroutine(ApplyMeleeDamage());
    }

    private IEnumerator ApplyMeleeDamage()
    {
        yield return new WaitForSeconds(0.5f); // Adjust delay to sync with animation

        // Check for all players/enemies in range
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, meleeAttackRange);
        foreach (Collider target in hitTargets)
        {
            Health targetHealth = target.GetComponent<Health>();
            if (targetHealth != null && target.CompareTag("Player"))
            {
                // Apply damage
                targetHealth.DamageHealth(25); // Adjust damage value
                Debug.Log($"Player {target.name} takes damage from melee attack!");

                // Optional: Apply knockback
                Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
                if (targetRigidbody != null)
                {
                    Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;
                    targetRigidbody.AddForce(knockbackDirection * 5f, ForceMode.Impulse);
                }

                // Optional: Play hit effect
                PlayHitEffect(target.transform.position);
            }
        }
    }

    private void PlayHitEffect(Vector3 position)
    {
        // Example: Spawn particle effect at the target position
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, position, Quaternion.identity);
        }
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
        if (currentPhase != BossPhase.Phase2) return;

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
        if (remainingHealth <= 0)
        {
            Debug.Log("Krampus has been defeated!");

            // Trigger death animation or effect
            if (animator != null)
            {
                animator.SetTrigger("Death");
            }

            // Destroy the boss after death
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, meleeAttackRange);

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, rangedAttackRange);
    }
}
