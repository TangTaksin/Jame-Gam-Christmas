using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    Camera _mainCam;
    Rigidbody2D rb2d;
    Vector2 InputAxis;
    Vector2 LastAxis = Vector2.down;

    PlayerMovement _pMove;

    public float dashDistance = 1f;
    public float dashTine = 0.5f;

    private void Start()
    {
        _mainCam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
        _pMove = GetComponent<PlayerMovement>();
    }

    public void OnMove(InputValue _value)
    {
        InputAxis = _value.Get<Vector2>();
        InputAxis.Normalize();

        LastAxis = Vector2.Lerp(LastAxis, InputAxis, InputAxis.magnitude);
    }

    public void OnDash()
    {
        var veloNeeded = (dashDistance / dashTine);
        print(veloNeeded);
        rb2d.linearVelocity += veloNeeded * LastAxis;
    }
}
