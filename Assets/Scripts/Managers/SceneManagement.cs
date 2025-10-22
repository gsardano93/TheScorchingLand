

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }

    public void SetSceneTransitionName(string sceneTransitionName)
    {
        SceneTransitionName = sceneTransitionName;
    }
}
