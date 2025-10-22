using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] string bossName;
    [SerializeField] protected float groundLineCheck;
    [SerializeField] protected float wallLineCheck;
    [SerializeField] protected float fallSpeed;
    [SerializeField] protected float attackRadiusCheck;
    [SerializeField] protected LayerMask ground;
    [SerializeField] protected Gate[] gates;
    [SerializeField] protected Transform matchTrigger;
    [SerializeField] protected float matchTriggerExtension;
    [SerializeField] AudioClip fightClip;
    protected Vector2 playerPos;
    private protected bool isGrounded;
    private protected bool hasDetectedWall;
    private protected bool canStartFight;
    private protected bool isMatchTriggered;
    protected override void Start()
    {
        base.Start();
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
        if (PlayerPrefs.GetInt(bossName) == 1)
            Destroy(gameObject);
    }
    protected virtual void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        foreach (Gate gate in gates)
            gate.Open();
        if (canStartFight)
        {
            SoundManager.Instance.RestoreMusic();
        }
    }
    protected virtual void TriggerMatch()
    {
        isMatchTriggered = Physics2D.Raycast(matchTrigger.transform.position, Vector2.up, matchTriggerExtension, whatIsPlayer);
        if (isMatchTriggered)
        {
            SoundManager.Instance.ChangeMusic(fightClip);
            CameraController.Instance.SetFollow(transform);
            Player.Instance.StopAllDashes();
            Player.Instance.SetCanMove(false);
            Player.Instance.Stop();
            matchTriggerExtension = 0;
            animator.SetTrigger("Entrance");
            foreach (Gate gate in gates)
                gate.Close();
        }
    }
    protected virtual void CollisionDetection()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundLineCheck, ground);
        hasDetectedWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, wallLineCheck, ground);
    }
    void StartFight()
    {
        canStartFight = true;
        CameraController.Instance.SetFollow(Player.Instance.transform);
        Player.Instance.SetCanMove(true);
    }
    protected override void Die()
    {
        base.Die();
        SoundManager.Instance.RestoreMusic();
        PlayerPrefs.SetInt(bossName, 1);
        foreach (Gate gate in gates)
            gate.Open();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(matchTrigger.transform.position,
        new Vector3(matchTrigger.transform.position.x, matchTrigger.transform.position.y + matchTriggerExtension));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundLineCheck));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallLineCheck, transform.position.y));
        Gizmos.DrawWireSphere(transform.position, attackRadiusCheck);
    }
}
