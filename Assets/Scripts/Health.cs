using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour
{
    public int max_health;

    public delegate void HealthEvent(int hp);
    public HealthEvent OnHealthChange;
    public HealthEvent OnHealthOut;

    public UnityEvent OnHealthChanged, OnHealthOuted;

    bool isInvul;

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
    
    public void SetInvul(bool state)
    {
        isInvul = state;
    }

    public int DamageHealth(int amount)
    {
        if (isInvul)
            return 0;

        current_health -= amount;
        StartCoroutine(FlashColor(Color.red, 0.1f));


        if (current_health <= 0)
        {
            current_health = 0;
            OnHealthOut?.Invoke(current_health);
            OnHealthOuted?.Invoke();
        }
        
        return amount;
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

    private IEnumerator FlashColor(Color color, float duration)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color originalColor = Color.white;

        sprite.color = color;
        yield return new WaitForSeconds(duration);

        sprite.color = originalColor;
    }
}
