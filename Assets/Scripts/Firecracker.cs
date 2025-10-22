using System.Collections;
using UnityEngine;

public class Firecracker : MonoBehaviour
{
    [SerializeField] float explosionRadius;
    [SerializeField] float timeToExplode;
    [SerializeField] float thrust;
    [SerializeField] int damage;
    Vector2 moveDir;
    Collider2D[] colliders;
    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (transform.position.x < Player.Instance.transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
        moveDir = new Vector2(transform.localScale.x, 1);
        thrust = Mathf.Abs(Player.Instance.transform.position.x - transform.position.x);
        rb.AddForce(moveDir * thrust, ForceMode2D.Impulse);
        StartCoroutine(TriggerExplosion());
    }


    void Update()
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
    }

    private IEnumerator TriggerExplosion()
    {
        yield return new WaitForSeconds(timeToExplode);
        animator.SetTrigger("Explode");
         SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().explosions, transform.position);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Player player))
            { Player.Instance.StopAllDashes();
                player.Knockback(transform);
                player.ReceiveDamage(damage);
                SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().gunshots, transform.position);
            }
        }
    }
   
    void DestroyObject()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
