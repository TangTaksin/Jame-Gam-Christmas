using UnityEngine;

public abstract class StateManager : MonoBehaviour
{
    public State state;

    public Vector2 InputAxis { get; set; }
    public Vector2 LastInput{ get; set; }

    protected virtual void Start()
    {

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
