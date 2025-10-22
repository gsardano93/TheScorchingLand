using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] Button continueButton;
    [SerializeField] Button mainMenuButton;
    private void Start()
    {

        continueButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            Hide();
        });
        mainMenuButton.onClick.AddListener(() =>
       {
           Time.timeScale = 1f;
           StartCoroutine(BackToMainMenuRoutine());
       });
        Hide();
    }

    IEnumerator BackToMainMenuRoutine()
    {
        GameObject fadeUI = GameObject.Find("FadeUI");
        fadeUI.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
        Destroy(Player.Instance.gameObject);
        Destroy(ManagersSingleton.Instance.gameObject);
       
    }

    public void Show()
    {
        gameObject.SetActive(true);
        continueButton.Select();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
