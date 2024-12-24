using UnityEngine;

public class PlayerIdleState : State
{
    public float deccelerationRate;
    Vector2 InputAxis;

    PlayerStateMachine psm;

    private void Start()
    {
        psm = GetComponent<PlayerStateMachine>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        anim.Play("Idle");
    }

    public override void OnUpdate()
    {
        if (InputAxis.magnitude > 0 || psm.attackPressed || psm.dashPressed)
            isComplete = true;
    }

    public override void OnFixedUpdate()
    {
        InputAxis = s_manager.InputAxis;
        var targetSpeed = Vector2.zero;

        var speedDif = targetSpeed - rigid.linearVelocity;
        var movement = speedDif * deccelerationRate;

        rigid.AddForce(movement);
    }
}
