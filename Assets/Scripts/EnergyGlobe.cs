using UnityEngine;

public class EnergyGlobe : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int damage;
    [SerializeField] float speed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        transform.localScale = new Vector3(Player.Instance.transform.localScale.x, 1, 1);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * transform.localScale.x, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().explosions, transform.position);
        }
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.ReceiveDamage(damage, transform);
            Destroy(gameObject);
            SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().explosions, transform.position);
        }
    }
}
