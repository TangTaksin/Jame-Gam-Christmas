using UnityEngine;

public class Character : MonoBehaviour
{
    public Health health;
    public KnockBack knockBackable;

    private void Start()
    {
        health = GetComponent<Health>();
        knockBackable = GetComponent<KnockBack>();
    }

    
}
