using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMidAirAttack : PlayerAbility
{
    bool isAttackingAtMidAir;
    protected override void Awake()
    {
        base.Awake();
        x = PlayerPrefs.GetInt("MidAirAttack");
        if (x == 0)
            enabled = false;
        inputActions.Player.MidAirAttack.performed += _ => MidAirAttack();
    }

    private void Update()
    {
        if (player.IsGrounded)
            isAttackingAtMidAir = false;
        cooldownTimer += Time.deltaTime;
        animator.SetBool("IsAttackingAtMidAir", isAttackingAtMidAir);

    }
    private void MidAirAttack()
    {
        if (cooldownTimer > cooldownTimerMax && !player.IsGrounded && player.CanMove)
        {
            isAttackingAtMidAir = true;
            cooldownTimer = 0;
        }
    }
    void EndMidAirAttack()
    {
        isAttackingAtMidAir = false;
    }
    public bool IsAttackingAtMidAir()
    {
        return isAttackingAtMidAir;
    }
}
