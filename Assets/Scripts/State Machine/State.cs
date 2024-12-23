using UnityEngine;

public abstract class State: MonoBehaviour
{
    public bool isComplete { get; private set; }

    public virtual void OnEnter() { }

    public virtual void OnUpdate() { }

    public virtual void OnExit() { }

}
