using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    protected PlayerInputActions inputActions;
    protected float cooldownTimer;
    protected float cooldownTimerMax = 1f;

    protected Animator animator;
    protected Rigidbody2D rb;
    protected Player player;
    protected int x;
    protected virtual void Awake()
    {
        inputActions = new PlayerInputActions();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        cooldownTimer = cooldownTimerMax;
    }

    protected virtual void OnEnable()
    {
        inputActions.Enable();
    }
    protected virtual void OnDestroy()
    {
        inputActions.Dispose();
    }
}
