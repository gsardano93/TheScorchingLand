using System.Collections;
using UnityEngine;

public class SpinnerGuy : Enemy
{
    [SerializeField] float attackRadiusCheck;
    [SerializeField] float wallLineCheck;
    
    [SerializeField] LayerMask whatIsGround;
    float idleDuration = 2f;
    float chargeDuration = 0.5f;
    float attackDuration = 3f;
    bool isDead = false;
    enum State
    {
        Idle,
        Charge,
        Attack
    }
    State state;
    protected override void Start()
    {
        base.Start();
        state = State.Idle;
    }
     void Update()
    {
        bool wallCheck = Physics2D.Raycast(transform.position, new Vector2(-transform.localScale.x, 0), wallLineCheck, whatIsGround);
        if (wallCheck)
            transform.localScale = new Vector2(-transform.localScale.x, 1);
        if (isDead) { return; }
        switch (state)
        {
            case State.Idle:
                StartCoroutine(StartToCharge());
                break;
            case State.Charge:
                StartCoroutine(PrepareToAttack());
                break;
            case State.Attack:
                StartCoroutine(AttackRoutine());
                break;
        }
    }
    protected override void Die()
    {
        isDead = true;
        base.Die();
    }
    private IEnumerator StartToCharge()
    {
        yield return new WaitForSeconds(idleDuration);
        state = State.Charge;
    }
    private IEnumerator PrepareToAttack()
    {
        animator.SetBool("IsCharging", true);
        yield return new WaitForSeconds(chargeDuration);
        animator.SetBool("IsCharging", false);
        state = State.Attack;
    }
    private IEnumerator AttackRoutine()
    {
        Attack();
        rb.velocity = new Vector2(speed * -transform.localScale.x, 0);
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("IsAttacking", false);
        rb.velocity = Vector2.zero;
        state = State.Idle;
    }
    private void Attack()
    {
        Collider2D[] damageZone = Physics2D.OverlapCircleAll(transform.position, attackRadiusCheck);
        foreach (Collider2D collider in damageZone)
        {
            if (collider.GetComponent<Player>() != null)
            { Player.Instance.StopAllDashes();
                Player.Instance.Knockback(transform);
                Player.Instance.ReceiveDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadiusCheck);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - wallLineCheck, transform.position.y));
    }

}
