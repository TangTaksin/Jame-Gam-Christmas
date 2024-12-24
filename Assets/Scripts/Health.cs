using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int max_health;

    public delegate void HealthEvent(int hp);
    public HealthEvent OnHealthChange;
    public HealthEvent OnHealthOut;

    public UnityEvent OnHealthChanged, OnHealthOuted;

    int cur_health;
    public int current_health
    {
        set 
        {
            if (value != cur_health)
            {
                cur_health = value;
                OnHealthChange?.Invoke(current_health);
                OnHealthChanged?.Invoke();
            }
        }
        get { return cur_health; }
    }

    private void Start()
    {
        current_health = max_health;
    }

    public void DamageHealth(int amount)
    {
        current_health -= amount;

        if (current_health <= 0)
        {
            current_health = 0;
            OnHealthOut?.Invoke(current_health);
            OnHealthOuted?.Invoke();
        }
    }

    public void HealHealth(int amount)
    {
        current_health += amount;

        if (current_health <= max_health)
        {
            current_health = max_health;
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
