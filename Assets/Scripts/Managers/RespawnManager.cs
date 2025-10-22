
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : Singleton<RespawnManager>
{
    int sceneIndex;
    public Vector3 LastFountain { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        if (Player.Instance != null)
            LastFountain = Player.Instance.transform.position;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void SetSceneIndex(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
    }
    public void SetLastFountain(Vector3 lastFountain)
    {
        LastFountain = lastFountain + new Vector3(0, 0.2f);
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
