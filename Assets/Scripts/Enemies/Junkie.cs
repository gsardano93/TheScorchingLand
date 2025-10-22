
using UnityEngine;

public class Junkie : Enemy
{
    [SerializeField] float attackRadiusCheck;
    SpriteRenderer playerSprite;
    float cooldownTimer;
    float cooldownTimerMax = 1f;
    protected override void Start()
    {
        base.Start();
        playerSprite = Player.Instance.GetComponent<SpriteRenderer>();
        cooldownTimer = cooldownTimerMax;
    }
    void Update()
    {

        bool isPlayerDetected = Physics2D.OverlapCircle(transform.position, attackRadiusCheck, whatIsPlayer);
        cooldownTimer -= Time.deltaTime;
        if (isPlayerDetected && cooldownTimer < 0)
        {
            FacePlayer();
            GrabPlayer();
        }
    }
    void GrabPlayer()
    {
        Player.Instance.SetCanMove(false);
        Player.Instance.StopAllDashes();
        PlayerMidAirDash playerMidAirDash = Player.Instance.GetComponent<PlayerMidAirDash>();
        playerMidAirDash.EndMidAirDash();
        Player.Instance.transform.position = new Vector2(Player.Instance.transform.position.x, transform.position.y);
        playerSprite.gameObject.SetActive(false);
        animator.SetTrigger("Grab");
    }
    private void ThrowPlayer()
    { 
        playerSprite.gameObject.SetActive(true);
        Player.Instance.StrongKnockback(transform);
        Player.Instance.ReceiveDamage(damage);
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().gunshots, transform.position);
        cooldownTimer = cooldownTimerMax;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadiusCheck);
    }
}
