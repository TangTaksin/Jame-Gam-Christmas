using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Vector2 direction; // Direction of movement (set externally or randomly)

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Move enemy
        Vector3 moveDelta = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        transform.position += moveDelta;

        // Update animation
        UpdateAnimation(direction);
    }

    void UpdateAnimation(Vector2 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    // Call this to change direction dynamically (e.g., for patrolling)
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Ensure direction is normalized
    }
}
