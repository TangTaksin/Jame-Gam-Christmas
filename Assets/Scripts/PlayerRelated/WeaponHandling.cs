using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandling : MonoBehaviour
{
    Animator animator;

    public Weapon currentWeapon;
    public Transform weaponSpot;
    public float bufferTime;
    public float expireTime = .5f;

    Camera _mainCam;
    Rigidbody2D rb2d;

    public string[] attackName = {"Attack 1", "Attack 2"};

    bool canAttack;
    bool expiring;
    bool isAttacking;

    int attack_index = 0;
    float expireTimer;
    float attackTimer;

    private void Start()
    {
        _mainCam = Camera.main;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ExpireTimer();
        AttackTracker();
    }

    public void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

    public void OnAttack()
    {
        ExecuteAttack();
    }

    void ExecuteAttack()
    {
        if (currentWeapon.isAttacking)
            return;

        var scn2pnt = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        var trfm2mse = scn2pnt - transform.position;

        currentWeapon.PlayAttack(attack_index, trfm2mse);

        attack_index++;
        if (attack_index+1 >= attackName.Length)
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
            rb2d.AddForce(currentWeapon.currentAttack.attackmovement, ForceMode2D.Impulse);
        }
    }

    void BufferTimer()
    {

    }
}
