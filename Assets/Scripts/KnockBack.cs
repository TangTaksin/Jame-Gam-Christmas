using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [Range(0,100)] public float resistance;

    Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockBack(Vector2 force)
    {
        rb2d.AddForce(force * (100 - resistance)/100, ForceMode2D.Impulse);
    }
}
