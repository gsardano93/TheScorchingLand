using UnityEngine;

public class FallingPlant : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int damage;
    [SerializeField] float speed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(0, -speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().explosions, transform.position);
        }
        if (other.TryGetComponent(out Player player))
        {
           
            player.Knockback(transform);
            player.ReceiveDamage(damage);
            Destroy(gameObject);
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().explosions, transform.position);
        }
    }
}
