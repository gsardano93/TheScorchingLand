using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour, IInteractable
{
    [SerializeField] string areaName;
    [SerializeField] string sceneTransitionName;
    [SerializeField] float secondsToWait = 0.75f;
    [SerializeField] bool isInteractable = false;
    bool canInteractAgain=true;
    private void OnTriggerEnter2D(Collider2D other)

    {
        if (isInteractable)
            return;
        if (other.GetComponent<Player>() != null)
        {
            ChangeScene();
        }
    }
    void ChangeScene()
    {
        Player.Instance.Stop();
        Player.Instance.SetCanMove(false);
        SceneManagement.Instance.SetSceneTransitionName(sceneTransitionName);
        StartCoroutine(ChangeSceneRoutine());
    }
    private IEnumerator ChangeSceneRoutine()
    {
        GameObject fadeUI = GameObject.Find("FadeUI");
        fadeUI.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(secondsToWait);
        SceneManager.LoadScene(areaName);
    }

    public void StartInteraction()
    {
        if (isInteractable)
            ChangeScene();
    }

    public bool CanInteractAgain()
    {
         return canInteractAgain;
    }
}
