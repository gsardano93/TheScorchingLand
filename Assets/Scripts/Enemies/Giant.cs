using UnityEngine;

public class Giant : Enemy
{
    [SerializeField] float triggerGuardRadiusCheck;
    [SerializeField] float attackRadiusCheck;
    [SerializeField] Transform attackPoint;
    [SerializeField] ParticleSystem guardParticles;
    int guardCounter = 3;
    bool isAttacking;
    float timeToDoublePunchMax = 2f;
    float timeToDoublePunch;

    enum State
    {
        Idle,
        Guard,
        Uppercut,
        DoublePunch
    }
    State state;
    bool isPlayerDetected;
    protected override void Start()
    {
        base.Start();
        state = State.Idle;
        timeToDoublePunch = timeToDoublePunchMax;
    }
    void Update()
    {
        FacePlayer();
        isPlayerDetected = Physics2D.OverlapCircle(transform.position, triggerGuardRadiusCheck, whatIsPlayer);

       
        switch (state)
        {
            case State.Idle:
                if (isAttacking)
                    return;
                guardCounter = 3;
                timeToDoublePunch = timeToDoublePunchMax;
                animator.SetBool("IsGuarding", false);
                if (isPlayerDetected)
                    state = State.Guard;
                break;
            case State.Guard:
                if (isAttacking)
                    return;
                if (!isPlayerDetected)
                    state = State.Idle;
                animator.SetBool("IsGuarding", true);
                timeToDoublePunch -= Time.deltaTime;
                if (timeToDoublePunch <= 0)
                    state = State.DoublePunch;
                break;
            case State.DoublePunch:
                if (isAttacking)
                    return;
                animator.SetTrigger("DoublePunch");
                isAttacking = true;
                break;
        }
    }

    public override void ReceiveDamage(int i, Transform other)
    {
        if (state == State.Guard)
        {
            guardCounter--;
            Instantiate(guardParticles,attackPoint.position, Quaternion.identity);
            if (guardCounter <= 0)
            {
                animator.SetTrigger("Uppercut");
                state = State.Idle;
            }
        }
        else
            base.ReceiveDamage(i, other);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, triggerGuardRadiusCheck);
        Gizmos.DrawWireSphere(attackPoint.position, attackRadiusCheck);

    }
    private void DoublePunchAnimationEvent()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().slashes, transform.position);
        Collider2D[] damageZone = Physics2D.OverlapCircleAll(attackPoint.position, attackRadiusCheck);
        foreach (Collider2D collider in damageZone)
        {
            if (collider.GetComponent<Player>() != null)
            {
                 Player.Instance.StopAllDashes();
                Player.Instance.Knockback(transform);
                Player.Instance.ReceiveDamage(damage);
                SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().gunshots, transform.position);
            }
        }
    }
    private void UppercutAnimationEvent()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().slashes, transform.position);
        Collider2D[] damageZone = Physics2D.OverlapCircleAll(attackPoint.position, attackRadiusCheck);
        foreach (Collider2D collider in damageZone)
        {
            if (collider.GetComponent<Player>() != null)
            {
                 Player.Instance.StopAllDashes();
                Player.Instance.StrongKnockback(transform);
                Player.Instance.ReceiveDamage(damage);
                SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().gunshots, transform.position);
            }
        }
    }
    void EndAttack()
    {
        timeToDoublePunch = timeToDoublePunchMax;
        state = State.Idle;
        isAttacking = false;
    }
}
