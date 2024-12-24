using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : State
{
    Health _hp;
    Stamina _pStam;

    public float dashForce = 1f;
    public bool needEnoughStamina = false;
    public float staminaCost = 25;

    bool use_succeed;
    bool dash_inputed;

    private void Start()
    {
        _hp = GetComponent<Health>();
        _pStam = GetComponent<Stamina>();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (dash_inputed)
            return;

        dash_inputed = true;

        use_succeed = _pStam.useStamina(needEnoughStamina, staminaCost);

        if (use_succeed)
        {
            rigid.AddForce(s_manager.LastInput * dashForce, ForceMode2D.Impulse);
            anim.Play("Dash");  

        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var isDashing = anim.GetCurrentAnimatorStateInfo(0).IsName("Dash");
        _hp.SetInvul(isDashing);

        if (!isDashing)
        {
            dash_inputed = false;
            isComplete = true;
        }
    }
}
