using UnityEngine;

public abstract class StateManager : MonoBehaviour
{
    public State state;

    public Rigidbody2D rigid2d;
    public Animator animator;

    public Vector2 InputAxis { get; set; }
    public Vector2 LastInput = Vector2.up;

    protected virtual void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (state)
        {
            state.OnUpdate();
            if (state.isComplete)
            {
                state.OnExit();
                ChooseState();
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (state)
        {
            state.OnFixedUpdate();
        }
    }

    protected virtual void ChooseState()
    {
        state.OnEnter();
    }
}
