using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle player taking damage here
            Destroy(gameObject);
        }
    }
}
