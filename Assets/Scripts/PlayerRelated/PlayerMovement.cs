using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : State
{
    bool _isMoving;

    Vector2 InputAxis;
    Vector2 LastInput;
    public float topSpeed;
    public float accelerationRate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        anim.Play("Walk");
    }

    public override void OnUpdate()
    {
        LastInput = s_manager.LastInput;
        anim.SetFloat("InputX", LastInput.x);
        anim.SetFloat("InputY", LastInput.y);

        if (!_isMoving)
        {
            isComplete = true;
        }
    }

    public override void OnFixedUpdate()
    {        
        InputAxis = s_manager.InputAxis;

        var targetSpeed = topSpeed * InputAxis;

        var speedDif = targetSpeed - rigid.linearVelocity;
        var movement = speedDif * accelerationRate;

        rigid.AddForce(movement);
    }
}
