using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Treasure : MonoBehaviour, IInteractable
{
    Animator animator;
    SpriteRenderer playerSprite;
    float textSpeed = 0.025f;
    [SerializeField] GameObject dialogBox;
    [SerializeField] string[] treasureTextArray;
    [SerializeField] Transform treasureFoundUI;
    [SerializeField] Sprite treasureFoundIcon;
    TextMeshProUGUI textBox;
    private bool canInteractAgain;
    int i = 0;
    protected virtual void Start()
    {
        playerSprite = Player.Instance.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        canInteractAgain = true;

        textBox = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        Hide();
    }
    private void Update()
    {
        if (i >= treasureTextArray.Length)
        {
            playerSprite.gameObject.SetActive(true);
            Hide();
            AssignTreasure();
            Player.Instance.SetCanMove(true);
            Destroy(gameObject);
        }
    }

    public abstract void AssignTreasure();

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
        StartCoroutine(StartDialog());
    }

    public IEnumerator StartDialog()
    {
        Show();
        Image treasureFoundIconUI = treasureFoundUI.Find("TreasureFoundIconUI").GetComponent<Image>();
        treasureFoundIconUI.sprite = treasureFoundIcon;
        Player.Instance.SetCanMove(false);
        textBox.text = "";
        playerSprite.gameObject.SetActive(false);
        animator.SetTrigger("PickUp");
        if (i < treasureTextArray.Length)
        {
            canInteractAgain = false;
            textBox.text = "";
            char[] chars = treasureTextArray[i].ToCharArray();
            foreach (char c in chars)
            {
                textBox.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        i++;
        canInteractAgain = true;

    }
    private void Show()
    {
        dialogBox.gameObject.SetActive(true);
        treasureFoundUI.gameObject.SetActive(true);
    }
    private void Hide()
    {
        dialogBox.gameObject.SetActive(false);
        treasureFoundUI.gameObject.SetActive(false);
    }

}
