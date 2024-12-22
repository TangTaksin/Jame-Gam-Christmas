using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public float basePower;
    public string[] attackOrder;
    public Attack[] attackDatas;

    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAttack()
    {

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

            Gizmos.DrawWireCube(atk.areaCenter, atk.areaSize);

            lastAtk = atk;
        }
    }
}
