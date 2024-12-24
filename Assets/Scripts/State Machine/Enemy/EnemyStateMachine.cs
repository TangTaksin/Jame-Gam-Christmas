using UnityEngine;

public class EnemyStateMachine : StateManager
{
    public State ChaseState;
    public State AttackState;

    public GameObject Target;
    public Vector3 vectorToTarget;
    public float distantFromTarget;
    public float attackDistance;

    

    protected override void Start()
    {
        base.Start();

        Target = GameObject.FindAnyObjectByType<PlayerStateMachine>().gameObject;

        ChaseState?.SetUp(animator, rigid2d, this);
        AttackState?.SetUp(animator, rigid2d, this);

        state = ChaseState;
    }

    protected override void ChooseState()
    {
        if (distantFromTarget <= attackDistance)
            state = AttackState;
        else
            state = ChaseState;
    }

    protected override void FixedUpdate()
    {
        vectorToTarget = Target.transform.position - transform.position;
        vectorToTarget.Normalize();
        distantFromTarget = vectorToTarget.magnitude;

        base.FixedUpdate();
    }
}
