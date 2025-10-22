
using UnityEngine;

public class PlayerDash : PlayerAbility
{

    private bool isDashing;
    float gravityDefault;

    protected override void Awake()
    {
        base.Awake();
        x = PlayerPrefs.GetInt("Dash");
        if (x == 0)
            enabled = false;
        gravityDefault = rb.gravityScale;
        inputActions.Player.Dash.performed += _ => Dash();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        animator.SetBool("IsDashing", isDashing);

    }
    private void Dash()
    {
        if (cooldownTimer > cooldownTimerMax && player.IsGrounded && player.CanMove && !player.HasJumped)
        {
            rb.velocity = Vector2.zero;
            float dashForce = 20f;
            rb.AddForce(Vector2.right * dashForce * transform.localScale.x, ForceMode2D.Impulse);
            player.SetCanMove(false);
            isDashing = true;
            cooldownTimer = 0;
            rb.gravityScale = 0f;
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().woshes, transform.position);
        }
    }
    public void EndDash()
    {
        rb.gravityScale = gravityDefault;
        isDashing = false;
        player.Stop();
        player.SetCanMove(true);
    }
    public bool IsDashing(){
        return isDashing;
    }
}
