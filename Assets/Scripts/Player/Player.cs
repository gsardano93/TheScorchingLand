using System;
using System.Collections;
using UnityEngine;

public class Player : Singleton<Player>
{
    ExperienceCounterUI experienceCounterUI;
    Flash flash;
    PlayerInputActions inputActions;
    Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] float climbSpeed;
    [SerializeField] float jump;
    [SerializeField] Transform interactPoint;
    [SerializeField] Transform groundDetectionPoint;
    [SerializeField] float interactRadiusCheck;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadiusCheck;
    [SerializeField] float groundLineCheck;
    [SerializeField] float wallLineCheck;
    [SerializeField] int damage;
    int hitPoints;
    [SerializeField] int hitPointsMax;
    [SerializeField] LayerMask ground;
    [SerializeField] private int healAmount;
    [SerializeField] Transform stompParticles;
    private int flaskAmountMax;
    private int flaskAmount;
    private int experience;
    public bool CanMove { get; private set; }
    public bool IsGrounded { get; private set; }
    private bool hasDetectedWall;
    private bool isFallingFast;
    private bool isHealing;
    public bool IsHanged { get; set; }
    private float highFallingSpeed = -25f;
    private float maxFallingSpeed = -30f;
    private float gravityDefault = 5f;
    private float coyoteJumpTimerMax = 0.15f;
    private float coyoteJumpTimer;
    public bool HasJumped { get; private set; }
    private float damageRecoveryTime = 0.75f;
    private bool canBeDamaged = true;
    private bool isGamePaused;
    GameObject pauseMenuUIGameObject;
    PauseMenuUI pauseMenuUI;
    Animator animator;
    public event EventHandler OnHealthChange;
    public event EventHandler OnFlaskChange;
    public event EventHandler<OnExpChangedEventArgs> OnExpChanged;
    public class OnExpChangedEventArgs
    {
        public int amountChanged;
    }
    public event EventHandler OnPlayerDeath;

    [Header("Knockback info")]
    [SerializeField] Vector2 knockbackDirection;
    [SerializeField] Vector2 strongKnockbackDirection;
    bool isKnocked;
    bool isStrongKnocked;

    [SerializeField] float knockbackDuration;
    bool canBeKnocked = true;
    [SerializeField] float knockbackProtectionTime;
    protected override void Awake()
    {
        base.Awake();
        inputActions = new PlayerInputActions();
        inputActions.Player.Attack.performed += _ => AttackAction();
        inputActions.Player.Heal.performed += _ => HealAction();
        inputActions.Player.Interact.performed += _ => Interact();
        inputActions.Player.Jump.performed += _ => Jump();
        inputActions.Player.Jump.canceled += _ => InterruptJump();
        inputActions.Player.Pause.performed += _ => PauseGame();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        flash = GetComponent<Flash>();
        gravityDefault = rb.gravityScale;
        pauseMenuUIGameObject = GameObject.Find("PauseMenuUI");
        pauseMenuUI = pauseMenuUIGameObject.GetComponent<PauseMenuUI>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDestroy()
    {
        inputActions.Dispose();
    }
    private void Start()
    {
        CanMove = true;
        IsHanged = false;
        hitPoints = hitPointsMax;
        flaskAmountMax = PlayerPrefs.GetInt("FlaskOwned");
        flaskAmount = flaskAmountMax;
        transform.position = RespawnManager.Instance.LastFountain;
        experienceCounterUI = FindObjectOfType<ExperienceCounterUI>();
        // animator.SetTrigger("WakeUp");
    }

    public void UpdateUI()
    {
        if (experienceCounterUI == null)
            experienceCounterUI = FindObjectOfType<ExperienceCounterUI>();
        OnHealthChange?.Invoke(this, EventArgs.Empty);
        OnFlaskChange?.Invoke(this, EventArgs.Empty);
        experienceCounterUI.RefreshTextBox(experience);
    }
    private void Update()
    {
        ClampFallingSpeed();
        AnimatorController();
        if (isKnocked)
            return;
        CollisionDetection();
        Move();
        Climb();
        if (!IsGrounded)
            coyoteJumpTimer -= Time.deltaTime;
        else if (IsGrounded && HasJumped)
            coyoteJumpTimer -= Time.deltaTime;
        else
            coyoteJumpTimer = coyoteJumpTimerMax;
        if (HasJumped && coyoteJumpTimer <= 0)
            HasJumped = false;

        if (rb.velocity.y < highFallingSpeed)
            isFallingFast = true;
    }
    private void CollisionDetection()
    {
        IsGrounded = Physics2D.Raycast(groundDetectionPoint.position, Vector2.down, groundLineCheck, ground);
        hasDetectedWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallLineCheck, ground);
        if (IsGrounded)
            animator.SetFloat("VerticalVelocity", 0);

    }
    private void AnimatorController()
    {

        animator.SetBool("IsRunning", rb.velocity.x != 0);
        if (!IsHanged)
        {
            if (!isStrongKnocked)
                animator.SetFloat("VerticalVelocity", rb.velocity.y);
        }
        else
            animator.SetFloat("ClimbVelocity", rb.velocity.y);
        animator.SetBool("IsFallingFast", isFallingFast);
        animator.SetBool("IsGrounded", IsGrounded);

        animator.SetBool("IsDead", hitPoints <= 0 && IsGrounded);
        if (!isStrongKnocked)
            animator.SetBool("IsKnocked", isKnocked);
        animator.SetBool("IsHanged", IsHanged);
    }
    private void CanMoveAgain()
    {
        CanMove = true;
        isFallingFast = false;
    }
    private void CannotMoveAgain()
    {
        CanMove = false;
        Stop();
    }
    private void PauseGame()
    {

        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            pauseMenuUI.Show();
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenuUI.Hide();
        }
    }
    private void Jump()
    {
        if (isHealing)
            return;
        if (hitPoints <= 0)
            return;
        if (IsHanged)
        {
            Vector2 dirJump = inputActions.Player.Movement.ReadValue<Vector2>();
            if (dirJump.x != 0)
            {
                rb.velocity = new Vector2(dirJump.x * speed * 0.75f, jump * 0.75f);
                SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().jumps, transform.position);
                IsHanged = false;
                RestoreGravity();
                Invoke("CanMoveAgain", 0f);
            }
        }
        if (isKnocked)
            return;
        if (!CanMove)
            return;

        if (HasJumped)
            return;
        if (IsGrounded || coyoteJumpTimer > 0)
        {
            HasJumped = true;
            rb.velocity = new Vector2(0, jump);
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().jumps, transform.position);
            isFallingFast = false;
        }
    }
    private void InterruptJump()
    {
        if (TryGetComponent(out PlayerMidAirDash playerMidAirDash))
            if (playerMidAirDash.IsMidAirDashing())
                return;
        if (HasJumped)
            rb.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
    }
    private void Move()
    {
        if (isHealing)
            return;
        if (!CanMove)
            return;
        if (hitPoints <= 0)
            return;
        Vector2 movement = inputActions.Player.Movement.ReadValue<Vector2>();
        float horSpeed;
        if (!hasDetectedWall)
            horSpeed = movement.x * speed;
        else
            horSpeed = 0;
        rb.velocity = new Vector2(horSpeed, rb.velocity.y);
        //Flip the character
        if (movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Climb()
    {
        if (IsHanged)
        {
            animator.SetFloat("VerticalVelocity", 0);
            rb.gravityScale = 0f;
            Vector2 climbMovement = inputActions.Player.Movement.ReadValue<Vector2>();
            rb.velocity = new Vector2(0, climbMovement.y * climbSpeed);
        }
    }

    private void Attack()
    {
        Collider2D[] damageZone = Physics2D.OverlapCircleAll(attackPoint.position, attackRadiusCheck);
        foreach (Collider2D collider in damageZone)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                collider.GetComponent<Enemy>().ReceiveDamage(damage, transform);
                SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().explosions, transform.position);
            }

        }
    }
    private void Interact()
    {
        if (isHealing)
            return;
        Collider2D[] interactCircle = Physics2D.OverlapCircleAll(transform.position, interactRadiusCheck);
        foreach (Collider2D collider in interactCircle)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteractAgain())
            {
                Stop();
                StopAllDashes();
                collider.GetComponent<IInteractable>().StartInteraction();
            }
        }
    }
    public void SetCanMove(bool boolean)
    {
        CanMove = boolean;
    }
    private void ClampFallingSpeed()
    {
        if (rb.velocity.y < maxFallingSpeed)
            rb.velocity = new Vector2(rb.velocity.x, maxFallingSpeed);
    }
    void AttackAction()
    {
        if (isHealing)
            return;
        if (hitPoints <= 0)
            return;
        if (CanMove && IsGrounded)
        {
            animator.SetTrigger("GroundAttack");
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().slashes, transform.position);
            CanMove = false;
            Stop();
        }
    }
    void HealAction()
    {
        if (isHealing)
            return;
        if (hitPoints <= 0)
            return;
        if (!CanMove)
            return;
        if (IsGrounded && flaskAmount > 0)
        {
            isHealing = true;
            animator.SetTrigger("Heal");
            Stop();
        }
    }
    public void Heal()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        hitPoints += healAmount;
        flaskAmount--;
        if (hitPoints > hitPointsMax)
            hitPoints = hitPointsMax;
        OnFlaskChange?.Invoke(this, EventArgs.Empty);
        OnHealthChange?.Invoke(this, EventArgs.Empty);
    }
    void EndHeal()
    {
        isHealing = false;
    }
    public void ReceiveDamage(int i)
    {
        if (!canBeDamaged)
            return;
        if (hitPoints <= 0)
            return;
        StartCoroutine(flash.FlashRoutine());
        hitPoints -= i;
        IsHanged = false;
        RestoreGravity();
        ChinemachineShakeManager.Instance.Shake();
        canBeDamaged = false;
        StartCoroutine(DamageRecoveryRoutine());
        OnHealthChange?.Invoke(this, EventArgs.Empty);
        CheckDeath();
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canBeDamaged = true;
    }
    public int GetHitPoints()
    {
        return hitPoints;
    }
    public int GetHitPointsMax()
    {
        return hitPointsMax;
    }
    public void RestoreHitPoints()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        flaskAmount = flaskAmountMax;
        hitPoints = hitPointsMax;
        OnHealthChange?.Invoke(this, EventArgs.Empty);
        OnFlaskChange?.Invoke(this, EventArgs.Empty);
    }
    public int GetFlaskAmount()
    {
        return flaskAmount;
    }
    public void UpgradeFlasks(int i)
    {
        flaskAmountMax += i;
        PlayerPrefs.SetInt("FlaskOwned", flaskAmountMax);
        RestoreHitPoints();
    }
    public int GetFlaskMaxAmount()
    {
        return flaskAmountMax;
    }

    public int GetExperience()
    {
        return experience;
    }
    public void SetExperience(int experience)
    {
        this.experience += experience;
        OnExpChanged?.Invoke(this, new OnExpChangedEventArgs
        {
            amountChanged = experience
        });
    }

    private void CheckDeath()
    {
        if (hitPoints <= 0)
        {
            RestoreCurrencyManager.Instance.OnPlayerDeath();
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().deaths, transform.position);
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
            CanMove = false;
            SetExperience(-experience);
            Stop();
            StartCoroutine(RespawnRoutine());
        }
    }
    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(3f);
        GameObject fadeUI = GameObject.Find("FadeUI");
        fadeUI.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        RespawnManager.Instance.RespawnPlayer();
    }

    public void RestoreGravity()
    {
        rb.gravityScale = gravityDefault;
    }
    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }

    public void Knockback(Transform damageTransform)
    {
        if (!canBeKnocked || hitPoints <= 0 || !canBeDamaged)
            return;
        if (hitPoints <= 0)
            return;
        int hDirection = 0;
        if (transform.position.x < damageTransform.position.x)
            hDirection = -1;
        else if (transform.position.x > damageTransform.position.x)
            hDirection = 1;
        canBeKnocked = false;
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * hDirection, knockbackDirection.y);
        Invoke("CancelKnockback", knockbackDuration);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }
    public void StrongKnockback(Transform damageTransform)
    {
        if (!canBeKnocked || hitPoints <= 0 || !canBeDamaged)
            return;
        if (hitPoints <= 0)
            return;
        int hDirection = 0;
        if (transform.position.x < damageTransform.position.x)
            hDirection = -1;
        else if (transform.position.x > damageTransform.position.x)
            hDirection = 1;
        isStrongKnocked = true;
        canBeKnocked = false;
        animator.SetTrigger("StrongKnockback");
        isKnocked = true;
        rb.velocity = new Vector2(strongKnockbackDirection.x * hDirection, strongKnockbackDirection.y);
        Invoke("CancelKnockback", knockbackDuration);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }
    void CancelKnockback()
    {
        isStrongKnocked = false;
        isKnocked = false;
        CanMove = true;
    }
    void AllowKnockback()
    {
        canBeKnocked = true;
    }
    public void StopAllDashes()
    {
        if (TryGetComponent<PlayerDash>(out PlayerDash playerDash))
            if (playerDash.IsDashing())
                playerDash.EndDash();
        if (TryGetComponent<PlayerMidAirDash>(out PlayerMidAirDash playerMidAirDash))
            if (playerMidAirDash.IsMidAirDashing())
                playerMidAirDash.EndMidAirDash();
    }

    void StrongLandAnimEvent()
    {
        Instantiate(stompParticles, groundDetectionPoint.position, Quaternion.identity);
        ChinemachineShakeManager.Instance.Shake();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundDetectionPoint.position, new Vector2(groundDetectionPoint.position.x, groundDetectionPoint.position.y - groundLineCheck));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallLineCheck, transform.position.y));
        Gizmos.DrawWireSphere(interactPoint.position, interactRadiusCheck);
        Gizmos.DrawWireSphere(attackPoint.position, attackRadiusCheck);
    }
}
