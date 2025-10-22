using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMidAirDash : PlayerAbility
{

    float gravityDefault;
    private bool isMidAirDashing;
    protected override void Awake()
    {
        base.Awake();
        x = PlayerPrefs.GetInt("MidAirDash");
        if (x == 0)
            enabled = false;
        gravityDefault = rb.gravityScale;
        inputActions.Player.MidAirDash.performed += _ => MidAirDash();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        animator.SetBool("IsMidAirDashing", isMidAirDashing);

    }
    void MidAirDash()
    {
        if (TryGetComponent(out PlayerMidAirAttack playerMidAirAttack))
        {
            if (playerMidAirAttack.IsAttackingAtMidAir())
                return;
        }
        if (cooldownTimer > cooldownTimerMax && !player.IsGrounded && player.CanMove)
        {
            rb.velocity = Vector2.zero;
            float dashForce = 20f;
            rb.AddForce(Vector2.right * dashForce * transform.localScale.x, ForceMode2D.Impulse);
            player.SetCanMove(false);
            isMidAirDashing = true;
            cooldownTimer = 0;
            rb.gravityScale = 0f;
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().woshes, transform.position);
        }
    }
    public void EndMidAirDash()
    {
        rb.gravityScale = gravityDefault;
        isMidAirDashing = false;
        player.Stop();
        player.SetCanMove(true);
    }
    public bool IsMidAirDashing()
    {
        return isMidAirDashing;
    }

}
