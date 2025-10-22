using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manhole : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider2D;
    [SerializeField] int damage;
    [SerializeField] float cooldownTimerMax;
    float cooldownTimer;
    [SerializeField] float eruptDurationMax;
    float eruptDuration;
    [SerializeField] float eruptDelay;
    bool canErupt;
    bool isErupting;
    void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        StartCoroutine(EruptWithDelayRoutine());
        boxCollider2D.enabled = false;
    }
    private void Update()
    {
        animator.SetBool("IsErupting", isErupting);
        if (canErupt && !isErupting)
            cooldownTimer += Time.deltaTime;
        if (isErupting)
            eruptDuration += Time.deltaTime;
        if (cooldownTimer > cooldownTimerMax)
        {
            cooldownTimer = 0;
            isErupting = true;
        }
        if (eruptDuration > eruptDurationMax)
        {
            eruptDuration = 0;
            isErupting = false;
            boxCollider2D.enabled = false;
        }
    }
    private IEnumerator EruptWithDelayRoutine()
    {
        yield return new WaitForSeconds(eruptDelay);
        canErupt = true;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        { Player.Instance.StopAllDashes();
            player.Knockback(transform);
            player.ReceiveDamage(damage);
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().explosions, transform.position);
        }
    }
    void ActiveBoxColliderEventAnim()
    {
        boxCollider2D.enabled = true;
    }
}
