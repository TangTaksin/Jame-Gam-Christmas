using UnityEngine;

public abstract class State: MonoBehaviour
{
    public bool isComplete { get; protected set; }

    protected Animator anim;
    protected Rigidbody2D rigid;
    protected StateManager s_manager;

    public void SetUp(Animator _anim, Rigidbody2D _rb, StateManager _statemMang)
    {
        anim = _anim;
        rigid = _rb;
        s_manager = _statemMang;
    }

    public virtual void OnEnter() { isComplete = false; }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnExit() { }

}
