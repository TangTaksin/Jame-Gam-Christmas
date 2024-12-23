using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State State;

    private void Start()
    {

    }

    private void Update()
    {
        if (State)
            State.OnUpdate();
    }
}
