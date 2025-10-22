using System.Collections;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] string transitionName;
    bool shouldUpdateUI = true;
    private void Start()
    {
        RestoreCurrencyManager.Instance.SpawnCurrencyRestorer();
        Debug.Log("Called!");
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            Player.Instance.transform.position = transform.position;
            Player.Instance.SetCanMove(true);
            CameraController.Instance.SetFollow(Player.Instance.transform);
        }
    }
    private void Update()
    {
        if (shouldUpdateUI)
            StartCoroutine(UpdateUIRoutine());
    }

    IEnumerator UpdateUIRoutine()
    {
        shouldUpdateUI = false;
        yield return new WaitForSeconds(0.1f);
        Player.Instance.UpdateUI();
    }

}
