using System;
using System.Collections;
using UnityEngine;

public class FatMan : Boss
{
    [SerializeField] float height;

    float timeBetweenStates = 1f;
    float timerForStates;
    float timerToDescend;

    bool isAttacking = false;

    float timeStunned = 4f;
    bool isStunned;

    enum State
    {
        Idle,
        AttackFromAbove,
        Uprise,
        AttackFromGround,
        JumpTowardsTheEdgeOfArena,
        Charge,
        Stunned
    }
    State state;
 
    void Update()
    {
        TriggerMatch();
        CollisionDetection();
        if (canStartFight)
        {
            timerForStates += Time.deltaTime;
            Debug.Log(timerForStates);
            switch (state)
            {
                case State.Idle:
                    Idle();
                    break;
                case State.AttackFromAbove:
                    AttackFromAbove();
                    break;
                case State.Uprise:
                    JumpTowardsThePlayer();
                    break;
                case State.AttackFromGround:
                    AttackFromGround();
                    break;
                case State.JumpTowardsTheEdgeOfArena:
                    JumpTowardsTheEdgeOfArena();
                    break;
                case State.Charge:
                    Charge();
                    break;
                case State.Stunned:
                    if (!isStunned)
                        Stun();
                    break;
            }
        }
    }


    void Idle()
    {
        if (state == State.Idle && timerForStates > timeBetweenStates)
        {
            int indexState = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));
            switch (indexState)
            {
                case 0:
                    FacePlayer();
                    state = State.Uprise;
                    break;
                case 1:
                    FacePlayer();
                    state = State.AttackFromGround;
                    break;
                case 2:
                    FacePlayer();
                    state = State.JumpTowardsTheEdgeOfArena;
                    break;
            }
        }
    }
    void JumpTowardsTheEdgeOfArena()
    {
        if (!hasDetectedWall)
        {
            animator.SetBool("IsJumping", true);
            float height = 3f;
            rb.velocity = new Vector2(-speed * transform.localScale.x, height);
        }
        else
        {
            rb.velocity = new Vector2(0, -fallSpeed);
            if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
                FacePlayer();
                state = State.Charge;
            }
        }
    }
    void Charge()
    {
        rb.velocity = new Vector2(-speed * transform.localScale.x, 0);
        animator.SetBool("IsCharging", true);
        Collider2D[] damageZone = Physics2D.OverlapCircleAll(transform.position, attackRadiusCheck);
        foreach (Collider2D collider in damageZone)
        {
            if (collider.GetComponent<Player>() != null)
            {
                 Player.Instance.StopAllDashes();
                Player.Instance.Knockback(transform);
                Player.Instance.ReceiveDamage(damage);
                rb.velocity = Vector2.zero;
                animator.SetBool("IsCharging", false);
                ReturnToIdle();
            }
        }

        if (hasDetectedWall)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsCharging", false);
            animator.SetTrigger("Impact");
            state = State.Stunned;
        }
    }

    void Stun()
    {
        isStunned = true;
        StartCoroutine(StunRoutine());
    }

    IEnumerator StunRoutine()
    {
        animator.SetBool("IsStunned", true);
        yield return new WaitForSeconds(timeStunned);
        animator.SetBool("IsStunned", false);
        ReturnToIdle();
    }
    void JumpTowardsThePlayer()
    {
        if (!isAttacking)
        {
            playerPos = Player.Instance.transform.position;
            Debug.Log(playerPos);
            isAttacking = true;
            rb.velocity = new Vector2(playerPos.x - transform.position.x, height).normalized * speed;
            animator.SetBool("IsJumping", true);
        }
        if (Mathf.Abs(transform.position.x - playerPos.x) < 1f &&
            Mathf.Abs(transform.position.y - (playerPos.y + height)) < 1f)
        {
            rb.velocity = Vector2.zero;
            state = State.AttackFromAbove;
            timerToDescend = 0;
        }
        timerToDescend += Time.deltaTime;
        if (timerToDescend > 2)
        {
            Debug.Log("Target missed!");
            rb.velocity = Vector2.zero;
            state = State.AttackFromAbove;
            timerToDescend = 0;
        }
    }
    void AttackFromAbove()
    {
        rb.velocity = Vector2.down * fallSpeed;
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsLanding", true);
        if (isGrounded)
        {
            rb.velocity = Vector2.zero;
            animator.SetTrigger("HelmBreaker");
            animator.SetBool("IsLanding", false);
        }
    }

    void AttackFromGround()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("GroundPunch");
            isAttacking = true;
        }
    }
    void ReturnToIdle()
    {
        isStunned = false;
        isAttacking = false;
        state = State.Idle;
        timerForStates = 0f;
    }
    private void AttackAnimationEvent()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().slashes, transform.position);
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
    }
}
