using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateManager
{
    [Space]
    public State idleState;
    public State moveState;
    public State dashState;
    public State attackState;

    Rigidbody2D rigid2d;
    Animator animator;

    public bool dashPressed { get; protected set; } 
    public float dashBufferTime = .1f;
    float dashBufferTimer;

    public bool attackPressed { get; protected set; }
    public float atkBufferTime = .1f;
    float atkBufferTimer;

    protected override void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        idleState?.SetUp(animator, rigid2d, this);
        moveState?.SetUp(animator, rigid2d, this);
        dashState?.SetUp(animator, rigid2d, this);
        attackState?.SetUp(animator, rigid2d, this);

        state = idleState;
    }

    protected override void Update()
    {
        base.Update();

        InputTimer();
    }

    public void OnMove(InputValue _value)
    {
        InputAxis = _value.Get<Vector2>();
        InputAxis.Normalize();

        LastInput = Vector2.Lerp(LastInput, InputAxis, InputAxis.magnitude);
    }

    public void OnDash()
    {
        dashPressed = true;
        dashBufferTimer = dashBufferTime;
    }

    public void OnAttack()
    {
        attackPressed = true;
        atkBufferTimer = atkBufferTime;
    }

    void InputTimer()
    {
        if (dashPressed)
        {
            dashBufferTimer -= Time.deltaTime;
            if (dashBufferTimer < 0)
                dashPressed = false;
        }

        if (attackPressed)
        {
            atkBufferTimer -= Time.deltaTime;
            if (atkBufferTimer < 0)
                attackPressed = false;
        }
    }

    protected override void ChooseState()
    {
        state = idleState;

        if (InputAxis.magnitude > 0)
            state = moveState;

        if (attackPressed)
            state = attackState;

        if (dashPressed)
            state = dashState;

        state.OnEnter();
    }
}
