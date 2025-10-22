
using UnityEngine;
using UnityEngine.UI;

public class FlaskIconsUI : MonoBehaviour
{
    [SerializeField] Sprite fullFlask;
    [SerializeField] Sprite emptyFlask;
    [SerializeField] Image[] flaskIconArray;
    private void Start()
    {
        Player.Instance.OnFlaskChange += Player_OnFlaskChange;
        SetAvailableFlask();
    }
    private void OnEnable() {
        
    }
    private void OnDisable() {
        Player.Instance.OnFlaskChange -= Player_OnFlaskChange;
    }
    private void Player_OnFlaskChange(object sender, System.EventArgs e)
    {
        SetAvailableFlask();
        foreach (Image image in flaskIconArray)
        {
            image.sprite = emptyFlask;
        }
        for (int i = 0; i < Player.Instance.GetFlaskAmount(); i++)
        {
            flaskIconArray[i].sprite = fullFlask;
        }
    }

    void SetAvailableFlask()
    {
        for (int i = 0; i < flaskIconArray.Length; i++)
        {
            flaskIconArray[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Player.Instance.GetFlaskMaxAmount(); i++)
        {
            flaskIconArray[i].gameObject.SetActive(true);
        }
    }
}
