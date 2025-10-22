
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button backButton;

    private void Awake()
    {
        if (playButton != null)
            backButton.Select();
        else
            exitButton.Select();
        exitButton.onClick.AddListener(() => Application.Quit());
        if (playButton != null)
            playButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                PlayerPrefs.DeleteAll();
                StartCoroutine(BackToMainMenuRoutine());
            });
    }
    void Start()
    {
        if (Player.Instance != null && ManagersSingleton.Instance != null)
        {

            Destroy(ManagersSingleton.Instance.gameObject);
            Destroy(Player.Instance.gameObject);
        }
    }
    public void ActiveBackButton()
    {
        if (playButton != null)
            backButton.Select();
        else
            exitButton.Select();
    }
    public void ActivePlayButton()
    {
        playButton.Select();
    }

    IEnumerator BackToMainMenuRoutine()
    {
        GameObject fadeUI = GameObject.Find("FadeUI");
        fadeUI.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

}
