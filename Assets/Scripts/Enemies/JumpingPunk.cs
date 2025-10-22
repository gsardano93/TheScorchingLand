using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPunk : Enemy
{
    float stateTimer;
    [SerializeField] float playerRadiusCheck;
    [SerializeField] float groundLineCheck;
    [SerializeField] float jumpSpeed;
    [SerializeField] float wallLineCheck;
    [SerializeField] float attackRadiusCheck;
    [SerializeField] LayerMask ground;
    enum State
    {
        Idle,
        Jump,
        Attack
    }
    State state;
    private bool hasDetectedWall;
    bool isAttacking = false;
    bool hasJumped = false;
    bool isGrounded;
    bool hasDetectedPlayer;
    bool canStartFight;
    Vector2 attackDirection;
    protected override void Start()
    {
        base.Start();
        stateTimer = 2f;
    }
    void Update()
    {
        AnimatorController();
        CollisionDetection();
        if (hasDetectedPlayer)
            canStartFight = true;
        else
            canStartFight = false;
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Jump:
                Jump();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    private void Idle()
    {
        canBeKnocked = true;
        isAttacking = false;
        hasJumped = false;
        if (canStartFight)
            stateTimer -= Time.deltaTime;
        if (stateTimer < 0)
        {
            stateTimer = 2f;
            state = State.Jump;
        }
    }

    private void Jump()
    {
        canBeKnocked = false;
        if (!hasJumped && isGrounded)
        {
            rb.velocity = new Vector2(0, jumpSpeed);
            hasJumped = true;
        }
        if (rb.velocity.y < -0.1f)
            state = State.Attack;
    }
    private void Attack()
    {
        canBeKnocked = false;
        Collider2D[] damageZone = Physics2D.OverlapCircleAll(transform.position, attackRadiusCheck);
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
        if (!isAttacking)
        {
            FacePlayer();
            attackDirection = (Player.Instance.transform.position - transform.position).normalized;
            isAttacking = true;
        }
        rb.velocity = attackDirection * speed;
        if (hasDetectedWall)
        {
            rb.velocity = Vector2.down * speed;
        }
        if (isGrounded)
        {
            rb.velocity = Vector2.zero;
            state = State.Idle;
        }
    }
    void AnimatorController()
    {
        // animator.SetFloat("VerticalVelocity", rb.velocity.y);
        animator.SetBool("HasJumped", hasJumped);
        animator.SetBool("IsAttacking", isAttacking);
    }
    private void CollisionDetection()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundLineCheck, ground);
        hasDetectedPlayer = Physics2D.OverlapCircle(transform.position, playerRadiusCheck, whatIsPlayer);
        hasDetectedWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallLineCheck, ground);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundLineCheck));
        Gizmos.DrawWireSphere(transform.position, playerRadiusCheck);
        Gizmos.DrawWireSphere(transform.position, attackRadiusCheck);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallLineCheck, transform.position.y));
    }
}
