
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] ParticleSystem deathParticleSystem;
    [SerializeField] protected int damage;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected float speed;
    [SerializeField] protected float knockbackThrust = 5f;
    [SerializeField] protected private int hitPointsMax;
    protected private int hitPoints;
    [SerializeField] protected int experienceReward;
    [SerializeField] protected bool canBeKnocked;
    protected Rigidbody2D rb;
    protected Knockback knockback;
    protected Flash flash;
    protected private Animator animator;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitPoints = hitPointsMax;
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();

    }
  
    public virtual void ReceiveDamage(int i, Transform other)
    {
        if (canBeKnocked)
            knockback.GetKnockBack(other, knockbackThrust);
        StartCoroutine(flash.FlashRoutine());
        hitPoints -= i;
        if (hitPoints <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        Player.Instance.SetExperience(experienceReward);
        gameObject.SetActive(false);
    }
    protected virtual void FacePlayer()
    {
        if (transform.position.x < Player.Instance.transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
    public int GetHitPoints(){
        return hitPoints;
    }

}
