using UnityEngine;

public class ChaseState : State
{
    public float accel_rate = 5;
    public float max_speed = 10;

    EnemyStateMachine esm;

    private void Start()
    {
        esm = GetComponent<EnemyStateMachine>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (esm.distantFromTarget <= esm.attackDistance)
        {
            isComplete = true;
        }
    }

    // Update is called once per frame
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        var targetSpeed = max_speed * esm.vectorToTarget;

        var speedDif = targetSpeed - (Vector3)rigid.linearVelocity;
        var movement = speedDif * accel_rate;

        rigid.AddForce(movement);
    }
}
