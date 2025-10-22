using System.Collections;
using TMPro;
using UnityEngine;
public class NonPlayingCharacter : MonoBehaviour, IInteractable
{
    [TextArea(0, 4)]
    [SerializeField] protected string[] stringArray;
    [SerializeField] protected GameObject dialogBox;
    [SerializeField] protected string identifierString;
    protected TextMeshProUGUI textBox;
    
    protected int i = 0;
    protected private bool canInteractAgain;
    protected float textSpeed = 0.025f;
    [SerializeField] protected bool canBeRepeated;

    protected virtual void Start()
    {
        if (PlayerPrefs.GetInt("NPC" + identifierString) == 1 && identifierString != "")
            Destroy(gameObject);
        canInteractAgain = true;
       
        textBox = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        Hide();
    }

    public bool CanInteractAgain()
    {
        return canInteractAgain;
    }
    public void StartInteraction()
    {
        if (DialogIsOver())
            return;
        if (dialogBox == null)
        {
            dialogBox = GameObject.Find("DialogBoxUI");
            textBox = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
            Hide();
        }
        if (Player.Instance.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
        Player.Instance.SetCanMove(false);
        StartCoroutine(StartDialog());
    }

    public IEnumerator StartDialog()
    {
        Show();

        textBox.text = "";

        if (i < stringArray.Length)
        {
            canInteractAgain = false;
            textBox.text = "";
            char[] chars = stringArray[i].ToCharArray();
            foreach (char c in chars)
            {
                textBox.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        i++;
        canInteractAgain = true;

    }
    protected virtual void Show()
    {
        dialogBox.gameObject.SetActive(true);
    }
    protected virtual void Hide()
    {
        dialogBox.gameObject.SetActive(false);
    }

    protected virtual bool DialogIsOver()
    {
        if (i >= stringArray.Length)
        {
            Hide();
            Player.Instance.SetCanMove(true);
            if (canBeRepeated)
                i = 0;
            else
                PlayerPrefs.SetInt("NPC" + identifierString, 1);
            return true;
        }
        return false;
    }
    /*protected virtual void EndDialog()
    {
        if (i >= stringArray.Length)
        {
            Hide();
            player.SetCanMove(true);
            PlayerPrefs.SetInt("NPC" + identifierString, 1);
            i = 0;
        }
        if (PlayerPrefs.GetInt("NPC" + identifierString) == 1 && identifierString != "")
            canInteractAgain = false;

    }*/
}
