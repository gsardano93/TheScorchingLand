using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] string i;
    int x;
    Animator animator;
    BoxCollider2D boxCollider2D;
    [SerializeField] bool isOpen;
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        x = PlayerPrefs.GetInt("Gate_" + i);
        if (x == 1)
            Open();
        if (x == 2)
            Close();
    }

    private void Update()
    {
        animator.SetBool("IsOpen", isOpen);
    }
    public void Open()
    {
        PlayerPrefs.SetInt("Gate_" + i, 1);
        if (boxCollider2D != null)
            boxCollider2D.enabled = false;
        isOpen = true;
    }
    public void Close()
    {
        PlayerPrefs.SetInt("Gate_" + i, 2);

        boxCollider2D.enabled = true;
        isOpen = false;
    }
    public bool IsOpen()
    {
        return isOpen;
    }
}
