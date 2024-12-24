using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandling : State
{
    Animator animator;

    public Weapon currentWeapon;
    public Transform weaponSpot;
    public float expireTime = .5f;

    Camera _mainCam;

    public string[] attackName = {"Attack 1", "Attack 2"};

    bool expiring;

    int attack_index = 0;
    float expireTimer;

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        ExpireTimer();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        ExecuteAttack();
    }


    public override void OnUpdate()
    {
        base.OnUpdate();

        AttackTracker();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

    void ExecuteAttack()
    {
        if (currentWeapon.isAttacking)
            return;

        var scn2pnt = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        var trfm2mse = scn2pnt - transform.position;

        anim.SetFloat("InputX", trfm2mse.x);
        anim.SetFloat("InputY", trfm2mse.y);
        anim.Play(attackName[attack_index]);

        if (Mathf.Abs(trfm2mse.x) > Mathf.Abs(trfm2mse.y))
            trfm2mse.y = 0;
        else
            trfm2mse.x = 0;
        var vecRound = new Vector2(Mathf.RoundToInt(trfm2mse.x), Mathf.RoundToInt(trfm2mse.y));

        s_manager.LastInput = vecRound;

        print("playing " + attackName[attack_index]);

        currentWeapon.PlayAttack(attack_index, trfm2mse);

        attack_index++;
        if (attack_index > attackName.Length -1)
            attack_index = 0;
    }

    void ExpireTimer()
    {
        if (expiring)
        {
            expireTimer -= Time.deltaTime;

            if (expireTimer <= 0)
            {
                expiring = false;
                attack_index = 0;
            }
        }
    }

    void AttackTracker()
    {
        if (currentWeapon.atk_timer == currentWeapon.currentAttack.movement_start)
        {
            rigid.linearVelocity /= 2;
            rigid.AddForce(currentWeapon.currentAttack.attackmovement, ForceMode2D.Impulse);
        }

        if (currentWeapon.atk_timer >= currentWeapon.currentAttack.recovery_over && !expiring)
        {
            expiring = true;
            expireTimer = expireTime;
            isComplete = true;
        }
    }
}
