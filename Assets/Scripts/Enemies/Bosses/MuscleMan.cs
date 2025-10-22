using System.Collections;
using UnityEngine;

public class MuscleMan : Boss
{
    [SerializeField] Transform shockWavePrefab;
    [SerializeField] Transform fallingPlant;
    [SerializeField] Transform instantiatePlantPoint1;
    [SerializeField] Transform instantiatePlantPoint2;
    [SerializeField] float jumpSpeed;
    [SerializeField] int plantToInstantiate;
    bool isAttacking = false;
    float timeBetweenStates = 2f;
    float timerForStates;
    float delayBetweenStomps = 1f;
    float delayBetweenHits = 1.5f;
    Vector2 fallDir;
    State state;
    enum State
    {
        Idle,
        Stomp,
        HitGround,
        Uppercut,
        FallTowardsPlayer
    }
    void Update()
    {
        TriggerMatch();
        CollisionDetection();
        if (canStartFight)
        {
            timerForStates += Time.deltaTime;
            Debug.Log(state);
            animator.SetBool("IsFalling", rb.velocity.y < 0);
            switch (state)
            {
                case State.Idle:
                    Idle();
                    break;
                case State.Stomp:
                    Stomp();
                    break;
                case State.HitGround:
                    HitGround();
                    break;
                case State.Uppercut:
                    Uppercut();
                    break;
                case State.FallTowardsPlayer:
                    FallTowardsPlayer();
                    break;
            }
        }
    }
    void Idle()
    {
        if (state == State.Idle && timerForStates > timeBetweenStates)
        {
            int indexState = Mathf.RoundToInt(Random.Range(0f, 2f));
            switch (indexState)
            {
                case 0:
                    state = State.Stomp;
                    break;
                case 1:
                    state = State.Uppercut;
                    break;
                case 2:
                    state = State.HitGround;
                    break;

            }
        }
    }

    void Stomp()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(StompRoutine());
        }
    }

    IEnumerator StompRoutine()
    {
        int numberOfStomps = Random.Range(2, 4);
        for (int i = 0; i < numberOfStomps; i++)
        {
            animator.SetTrigger("Stomp");
            yield return new WaitForSeconds(delayBetweenStomps);
        }
        ReturnToIdle();
    }

    void HitGround()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(HitGroundRoutine());
        }
    }

    IEnumerator HitGroundRoutine()
    {
        int numberOfHits = Random.Range(2, 3);
        for (int i = 0; i < numberOfHits; i++)
        {
            animator.SetTrigger("HitGround");
            yield return new WaitForSeconds(delayBetweenHits);
        }
        ReturnToIdle();
    }

    void Uppercut()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Uppercut");
        }
        if (rb.velocity.y < 0)
        {
            playerPos = Player.Instance.transform.position;
            fallDir = (playerPos - (Vector2)transform.position).normalized;
            state = State.FallTowardsPlayer;
        }
    }
    void JumpAnimEvent()
    {
        Attack();
        rb.velocity = new Vector2(0, jumpSpeed);
    }
    void FallTowardsPlayer()
    {
        Attack();
        rb.velocity = fallDir * fallSpeed;
        if (isGrounded)
            ReturnToIdle();
    }

    void StompAnimEvent()
    {
        Attack();
        Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
        Transform shockWave = Instantiate(shockWavePrefab, transform.position + Vector3.down * 0.3f, Quaternion.identity);
        shockWave.transform.localScale = new Vector3(-1, 1, 1);
    }
    void HitGroundAnimEvent()
    {
        Attack();
        for (int i = 0; i < plantToInstantiate; i++)
        {
            Debug.Log("plantinstatiate");
            Vector2 instatiationPoint = new Vector2(Random.Range(instantiatePlantPoint1.position.x,
                                                                instantiatePlantPoint2.position.x),
                                                    instantiatePlantPoint1.position.y);
            Instantiate(fallingPlant, instatiationPoint, Quaternion.identity);
        }
    }
    void ReturnToIdle()
    {
        isAttacking = false;
        state = State.Idle;
        timerForStates = 0f;
    }
    private void Attack()
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
