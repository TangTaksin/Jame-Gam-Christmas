using UnityEngine;

[System.Serializable]
public class Attack
{
    public string animationStateName;
    public Vector2 AttackDirection;

    [Header("Attack data")]
    public float attackPower = 100f;

    [Header("Attack Area")]
    public Vector2 areaCenter;
    public Vector2 areaSize;

    [Header("Active Time")]
    public float active_start;
    public float active_end;
}
