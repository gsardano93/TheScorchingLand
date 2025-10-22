using UnityEngine;
using UnityEngine.SceneManagement;

public class Fountain : MonoBehaviour, IInteractable
{
    Animator animator;
    private bool canInteractAgain;
    SpriteRenderer playerSprite;
    Enemy[] enemies;
    private void Awake()
    {
        enemies = FindObjectsOfType<Enemy>();
        animator = GetComponent<Animator>();
        canInteractAgain = true;
    }
    private void Start()
    {
        playerSprite = Player.Instance.GetComponent<SpriteRenderer>();

    }
    public bool CanInteractAgain()
    {
        return canInteractAgain;
    }

    public void StartInteraction()
    {
        if (Player.Instance.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
        canInteractAgain = false;
        Player.Instance.SetCanMove(false);
        playerSprite.gameObject.SetActive(false);
        animator.SetTrigger("Drink");
        Player.Instance.RestoreHitPoints();
        RespawnManager.Instance.SetLastFountain(transform.position);
        RespawnManager.Instance.SetSceneIndex(SceneManager.GetActiveScene().buildIndex);
    }
    private void QuitDrinking()
    {
        canInteractAgain = true;
        Player.Instance.SetCanMove(true);
        playerSprite.gameObject.SetActive(true);
    }


}
