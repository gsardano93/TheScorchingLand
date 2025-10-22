using System.Collections;

using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool IsGettingKnockBack{get; private set;}
    Rigidbody2D rb;
    [SerializeField] float knockbackDuration;
    
    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }
  
    public void GetKnockBack(Transform damageSource, float knockbackThrust )
    {
        IsGettingKnockBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * rb.mass * knockbackThrust;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }
    private IEnumerator KnockRoutine(){
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        IsGettingKnockBack = false;
    }
}
