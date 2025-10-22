using UnityEngine;

public class Bomber : Enemy
{
    [SerializeField] Transform firecrackerPrefab;
    [SerializeField] Transform firecrackerSpawnPoint;
    [SerializeField] float safetyDistance;
    [SerializeField] float dodgeTriggerDistance;
    float timerAttack;
    [SerializeField] float timerAttackMax = 2f;
    [SerializeField] float triggerAttackRadiusCheck;
    bool isAttacking;

    enum State
    {
        Idle,
        Attacking,
        Dodging
    }
    State state;
    protected override void Start()
    {
        base.Start();
    }

     void Update()
    {
        
        timerAttack += Time.deltaTime;
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Attacking:
                Attack();
                break;
            case State.Dodging:
                Dodge();
                break;
        }
    }
    void Idle()
    {
        float playerDistance = Vector2.Distance(Player.Instance.transform.position, transform.position);
        if (playerDistance < triggerAttackRadiusCheck && Player.Instance.GetHitPoints() > 0 && timerAttack > timerAttackMax)
            state = State.Attacking;
        FacePlayer();
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) < dodgeTriggerDistance)
            state = State.Dodging;

    }
    void Dodge()
    {
        animator.SetBool("IsDodging", true);
        Vector2 moveDir = new Vector2(-transform.localScale.x * speed, 0);
        rb.AddForce(moveDir, ForceMode2D.Impulse);
        if (Vector2.Distance(transform.position, Player.Instance.transform.position) > safetyDistance)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsDodging", false);
            state = State.Idle;
        }
    }

    void Attack()
    {
        if (isAttacking) { return; }
        animator.SetTrigger("Attack");
        timerAttack = 0f;
        isAttacking = true;
    }
    void AttackAction()
    {
        Instantiate(firecrackerPrefab, firecrackerSpawnPoint.position, Quaternion.identity);
    }
    void EndAttack()
    {
        isAttacking = false;
        state = State.Idle;
    }

}
