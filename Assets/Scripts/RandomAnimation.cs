
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
     Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
