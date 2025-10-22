
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] Gate[] gateArray;
    Player player;
    Animator animator;
    private bool canInteractAgain;
    SpriteRenderer playerSprite;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        playerSprite = Player.Instance.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        canInteractAgain = true;
    }
    public bool CanInteractAgain()
    {
        return canInteractAgain;
    }

    public void StartInteraction()
    {
        canInteractAgain = false;
        player.SetCanMove(false);
        playerSprite.gameObject.SetActive(false);
        animator.SetTrigger("Active");
    }
    private void EndLeverAnimation()
    {
        canInteractAgain = true;
        player.SetCanMove(true);
        playerSprite.gameObject.SetActive(true);
    }

    void TriggerMechanism()
    {
        foreach (Gate gate in gateArray)
        {
            if (!gate.IsOpen())
                gate.Open();
            else
                gate.Close();
        }
    }
}
