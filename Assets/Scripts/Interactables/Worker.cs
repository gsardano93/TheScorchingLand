using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Worker : NonPlayingCharacter
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] int fixCost;
    [SerializeField] BrokenObject brokenObject;
    [SerializeField] string confirmMessage;
    protected override void Start()
    {
        base.Start();
        HideButtons();
    }
    /* protected override void Update()
     {
         if (i == stringArray.Length)
         {
             canInteractAgain = false;
             textBox.text = "";
             textBox.text = "Riparare l'oggetto per: " + fixCost + "?";
             if (Player.Instance.GetExperience() < fixCost)
                 yesButton.interactable = false;
             ShowButtons();
             yesButton.onClick.AddListener(() =>
             {
                 Player.Instance.SetExperience(-fixCost);
                 brokenObject.FixObject();
                 Hide();
                 player.SetCanMove(true);
                 HideButtons();
                 i = 0;
                 canInteractAgain = true;
             });
             noButton.onClick.AddListener(() =>
             {
                 Hide();
                 player.SetCanMove(true);
                 HideButtons();
                 i = 0;
                 canInteractAgain = true;
             });
         }
     }*/

    protected override bool DialogIsOver()
    {
        if (!canBeRepeated)
            return true;
        if (i >= stringArray.Length)
        {
            if (Player.Instance.GetExperience() < fixCost)
                yesButton.interactable = false;
            textBox.text = "";
            textBox.text = confirmMessage;
            ShowButtons();
            
            return true;
        }
        return false;
    }

    void ShowButtons()
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        noButton.Select();
        yesButton.onClick.AddListener(() =>
           {
               StartCoroutine(FixRoutine());
           });
        noButton.onClick.AddListener(() =>
        {
            Hide();
            Player.Instance.SetCanMove(true);
            HideButtons();
            i = 0;
            canInteractAgain = true;
        });
    }
    void HideButtons()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
    }
    IEnumerator FixRoutine()
    {
        PlayerPrefs.SetInt("NPC" + identifierString, 1);
        Player.Instance.SetExperience(-fixCost);
        Debug.Log("fixed!");
        GameObject fadeUI = GameObject.Find("FadeUI");
        fadeUI.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(3f);
        fadeUI.GetComponent<Animator>().SetTrigger("FadeIn");
        brokenObject.FixObject();
        Hide();
        Player.Instance.SetCanMove(true);
        HideButtons();
        canBeRepeated = false;
    }
}
