using UnityEngine;

public class Health : MonoBehaviour
{
    public int max_health;

    public delegate void HealthEvent(int hp);
    public static HealthEvent OnHealthChange;
    public static HealthEvent OnHealthOut;

    int cur_health;
    public int current_health
    {
        set 
        {
            if (value != cur_health)
            {
                cur_health = value;
                OnHealthChange?.Invoke(current_health);
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
}
