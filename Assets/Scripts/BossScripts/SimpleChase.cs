using UnityEngine;

public class SimpleChase : MonoBehaviour
{
    public Transform target;  // The target object to chase (Player)
    public float accel_rate = 5;  // Acceleration rate
    public float max_speed = 10;  // Maximum speed

    private Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If the target isn't assigned (either in the inspector or dynamically), find the player at runtime
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");  // Find the player object by tag
            if (player != null)
            {
                target = player.transform;  // Set the target to the player's transform
            }
            else
            {
                Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            }
        }

        // Initialize the Rigidbody2D component
        rb2d = GetComponent<Rigidbody2D>();

        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D component is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Lock rotation on all axes, only allowing Y-axis rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (target == null) return; // Early return if there's no target

        // Calculate direction towards the target
        var direction = target.position - transform.position;
        direction.Normalize();  // Normalize to get a unit vector in the direction of the target

        // Calculate the target speed in that direction
        var targetSpeed = max_speed * direction;

        // Calculate the difference between the current speed and the target speed
        var speedDif = targetSpeed - (Vector3)rb2d.velocity;

        // Calculate the movement force
        var movement = speedDif * accel_rate;

        // Apply the force to the Rigidbody2D to move the object
        rb2d.AddForce(movement);
    }
}
