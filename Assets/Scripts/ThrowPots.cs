using System.Collections;

using UnityEngine;

public class ThrowPots : MonoBehaviour
{
    [SerializeField] Transform throwPoint;
    float throwTimer;
    [SerializeField] float throwTimerMax = 2f;
    [SerializeField] float throwTimeDelay;
    Animator animator;
    [SerializeField] Transform plant;
    private bool canThrow;
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ThrowWithDelayRoutine());
    }

    private void Update()
    {
        if (canThrow)
            throwTimer += Time.deltaTime;
        if (throwTimer > throwTimerMax)
        {
            animator.SetTrigger("Throw");
            throwTimer = 0;
        }
    }

    void IntantiatePlant()
    {
        Instantiate(plant, throwPoint.transform.position, Quaternion.identity);
    }
    private IEnumerator ThrowWithDelayRoutine()
    {
        yield return new WaitForSeconds(throwTimeDelay);
        canThrow = true;

    }

}
