using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public LayerMask targetLayer;
    public string[] attackOrder;
    public Attack[] attackDatas;

    public Attack currentAttack;
    int attack_index;
    public float atk_timer;
    public bool isAttacking;

    List<Character> charInArea = new List<Character>();

    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AttackTimeTracker();
    }

    public void PlayAttack(int atk_index, Vector2 direction)
    {
        attack_index = atk_index;

        _animator.Play(attackOrder[attack_index]);
        _animator.SetFloat("InputX", direction.x);
        _animator.SetFloat("InputY", direction.y);

        charInArea.Clear();
        GetAttackData(attackOrder[attack_index], direction);
    }

    void GetAttackData(string atk_name, Vector2 direction)
    {
        direction.Normalize();
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            direction.y = 0;
        else
            direction.x = 0;
        var vecRound = new Vector2(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));

        //print(vecRound);
        //find same name
        foreach (var data in attackDatas)
        {
            if ((atk_name == data.animationStateName) && (data.AttackDirection == vecRound))
            {
                currentAttack = data;
                break;
            }
        }
    }

    void DamageArea(bool active)
    {
        if (active)
        {
            Character character = null;

            var string_debug = string.Empty;

            var damage_area = Physics2D.OverlapBoxAll(transform.position + (Vector3)currentAttack.areaCenter, currentAttack.areaSize, 0, targetLayer);
            foreach (var col in damage_area)
            {
                col.TryGetComponent<Character>(out character);

                print(character);

                if (!charInArea.Contains(character))
                {
                    character.health.DamageHealth((int)(currentAttack.attackPower));
                    var dir = character.transform.position - transform.position;
                    dir.Normalize();

                    character.knockBackable.ApplyKnockBack(dir * currentAttack.knockbackPower);

                    charInArea.Add(character);

                    if (string_debug == string.Empty)
                        string_debug += " " + col.name;
                }
            }

            //print(string_debug);
        }
    }

    void AttackTimeTracker()
    {
        if (currentAttack == null)
            return;

        var nameMatch = _animator.GetCurrentAnimatorStateInfo(0).IsName(currentAttack.animationStateName);

        if (nameMatch)
        {
            atk_timer = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (atk_timer >= currentAttack.active_start)
                DamageArea(true);

            if (atk_timer >= currentAttack.active_end)
                DamageArea(false);

            isAttacking = (atk_timer <= currentAttack.recovery_over);

        }
    }

    private void OnDrawGizmos()
    {
        if (attackDatas == null)
            return;

        Gizmos.color = Color.red;
        Attack lastAtk = null;

        foreach (var atk in attackDatas)
        {   
            if (lastAtk != null && lastAtk.animationStateName != atk.animationStateName)
            {
                Gizmos.color += new Color(0, 0.1f, 0);
            }

            Gizmos.DrawWireCube(transform.position + (Vector3)atk.areaCenter, atk.areaSize);

            lastAtk = atk;
        }
    }
}
