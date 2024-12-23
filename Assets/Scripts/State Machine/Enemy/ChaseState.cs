using UnityEngine;

public class ChaseState : MonoBehaviour
{
    public Transform target;
    public float accel_rate = 5;
    public float max_speed = 10;

    Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target != null)
            target = GameObject.Find("Angri_elf").transform;

        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = target.position - transform.position;
        direction.Normalize();

        var targetSpeed = max_speed * direction;

        var speedDif = targetSpeed - (Vector3)rb2d.linearVelocity;
        var movement = speedDif * accel_rate;

        rb2d.AddForce(movement);
    }
}
