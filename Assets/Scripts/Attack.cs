using UnityEngine;

[System.Serializable]
public class Attack
{
    public string animationStateName;
    public Vector2 AttackDirection;

    [Header("Attack data")]
    public float attackPower = 1f;
    public float knockbackPower = 100f;
    public Vector2 attackmovement;

    [Header("Attack Area")]
    public Vector2 areaCenter;
    public Vector2 areaSize;

    [Header("Active Time")]
    public float movement_start;
    [Range(0f,1f)] public float active_start;
    [Range(0f, 1f)] public float active_end;
    public float recovery_over;
}
