using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterState.MovementState current_state = CharacterState.MovementState.idling;
    
    public void SetState(CharacterState.MovementState _state)
    {
        current_state = _state;
    }
}
