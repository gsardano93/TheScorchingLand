using System.Collections;
using UnityEngine;

public class Punk : Enemy
{
    [SerializeField] float distanceFromCorePatrol = 3f;

    [SerializeField] Transform attackPoint;
    [SerializeField] Transform patrolCorePoint;
    [SerializeField] float attackRadiusCheck;
    [SerializeField] Transform triggerAttackPoint;
    [SerializeField] float triggerAttackRadiusCheck;

    bool isAttacking = false;
    enum State
    {
        Patroling,
        Attacking
    }
    State state;
    protected override void Start()
    {
        base.Start();
        state = State.Patroling;
    }

    void Update()
    {
        DetectPlayer();
        animator.SetBool("IsWalking", rb.velocity.x != 0);
        switch (state)
        {
            case State.Patroling:
                Patrol();
                break;
            case State.Attacking:
                AttackAction();
                break;
        }
    }
    void DetectPlayer()
    {
        float playerDistance = Vector2.Distance(Player.Instance.transform.position, patrolCorePoint.transform.position);

        if (playerDistance < distanceFromCorePatrol && Player.Instance.GetHitPoints() > 0)
        {
            state = State.Attacking;
        }
        else
        {
            state = State.Patroling;
        }
    }
    void Patrol()
    {
        if (isAttacking || knockback.IsGettingKnockBack) { return; }
        rb.velocity = new Vector2(transform.localScale.x * -speed, rb.velocity.y);
        if ((transform.position.x - patrolCorePoint.transform.position.x) < -distanceFromCorePatrol)
            transform.localScale = new Vector3(-1, 1, 1);
        else if ((transform.position.x - patrolCorePoint.transform.position.x) > distanceFromCorePatrol)
            transform.localScale = new Vector3(1, 1, 1);
    }
    void AttackAction()
    {
        if (isAttacking) { return; }
        if (!knockback.IsGettingKnockBack)
            rb.velocity = new Vector2(transform.localScale.x * -speed, rb.velocity.y);
        FacePlayer();
        bool isPlayerDetected = Physics2D.OverlapCircle(triggerAttackPoint.position, triggerAttackRadiusCheck, whatIsPlayer);
        if (isPlayerDetected)
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        float timeBetweenAttacks = Random.Range(0.25f, 0.75f);
        yield return new WaitForSeconds(timeBetweenAttacks);
        animator.SetTrigger("Attack");
    }
    void EndAttack()
    {
        isAttacking = false;
    }
    private void Attack()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().slashes, transform.position);
        Collider2D[] damageZone = Physics2D.OverlapCircleAll(attackPoint.position, attackRadiusCheck);
        foreach (Collider2D collider in damageZone)
        {
            if (collider.GetComponent<Player>() != null)
            { Player.Instance.StopAllDashes();
                Player.Instance.Knockback(transform);
                Player.Instance.ReceiveDamage(damage);
                SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().gunshots, transform.position);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(triggerAttackPoint.position, triggerAttackRadiusCheck);
        Gizmos.DrawWireSphere(attackPoint.position, attackRadiusCheck);
    }
}
