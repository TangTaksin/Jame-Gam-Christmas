using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int damage = 1;
    public float interval = 1f;
    public string targetTag = "Player";

    float interval_timer;
    Character target;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            target = collision.gameObject.GetComponent<Character>();
            target.health.DamageHealth(damage);
            interval_timer = interval;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (target)
        {
            interval_timer -= Time.deltaTime;

            if (interval_timer <= 0)
            {
                interval_timer = interval;
                target.health.DamageHealth(damage);
            }
        }
    }
}
