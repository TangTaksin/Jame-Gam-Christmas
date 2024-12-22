using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float maxstamina = 100f;
    public float staminaReginRate = 1f;
    public float cooldownTilRegen = 1f;

    float curstamina;
    float cRegen_timer;

    bool canRegen = true;

    private void Start()
    {
        curstamina = maxstamina;
    }

    private void Update()
    {
        RegenTimer();
        Regenerate();
    }

    public (float, float) GetStamina()
    {
        return (curstamina, maxstamina);
    }

    public bool useStamina(bool need_enough_stam, float cost)
    {
        bool use_succeed = false;

        if (curstamina >= cost || (curstamina > 0 && !need_enough_stam))
        {
            curstamina -= cost;

            if (curstamina <= 0)
                curstamina = 0;

            use_succeed = true;
            canRegen = false;
            cRegen_timer = cooldownTilRegen;
        }

        return use_succeed;
    }

    private void Regenerate()
    {
        if (canRegen && curstamina < maxstamina)
        {
            curstamina += staminaReginRate * Time.deltaTime;

            if (curstamina >= maxstamina)
            {
                curstamina = maxstamina;
            }
        }
    }

    private void RegenTimer()
    {
        if (!canRegen)
        {
            cRegen_timer -= Time.deltaTime;

            if (cRegen_timer <= 0)
                canRegen = true;
        }
    }
}
