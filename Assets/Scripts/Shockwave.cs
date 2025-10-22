using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float timeToLive;
    [SerializeField] float acceleration;
    [SerializeField] float velocity;
    [SerializeField] int damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    void Update()
    {
        float moveDir = transform.localScale.x;
        velocity += Time.deltaTime * acceleration;
        rb.velocity = new Vector2(velocity * moveDir, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().StopAllDashes();
            other.GetComponent<Player>().Knockback(transform);
            other.GetComponent<Player>().ReceiveDamage(damage);
        }
    }
}
