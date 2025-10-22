using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationAreaUI : MonoBehaviour
{
    [SerializeField] string areaName;
    [SerializeField] Transform triggerPoint;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] float triggerRay;
    Animator animator;
    bool playerDetected;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (PlayerPrefs.GetInt(areaName) == 1)
            Destroy(gameObject);
    }


    void Update()
    {
        playerDetected = Physics2D.Raycast(triggerPoint.position, Vector2.up, triggerRay, whatIsPlayer);
        if (playerDetected)
        {
            animator.SetTrigger("PresentArea");
            PlayerPrefs.SetInt(areaName, 1);
        }
    }
    private void OnDrawGizmos() {
        Debug.DrawLine(triggerPoint.position,triggerPoint.position+new Vector3(0,triggerRay));
    }
}
