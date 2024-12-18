using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    bool _isMoving;

    Vector2 InputAxis;
    public float topSpeed;
    public float accelerationRate, deccelerationRate;

    Rigidbody2D rb2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputMovement();
    }

    public void OnMove(InputValue _value)
    {
        InputAxis = _value.Get<Vector2>();
        InputAxis.Normalize();
        _isMoving = InputAxis.x > 0;

        print(string.Format("{0}, {1}", InputAxis, _isMoving));

    }

    void InputMovement()
    {
        var targetSpeed = topSpeed * InputAxis;
        var rate = _isMoving ? accelerationRate : deccelerationRate;

        var speedDif = targetSpeed - rb2d.linearVelocity;
        var movement = speedDif * rate;

        rb2d.AddForce(movement);

    }
}
