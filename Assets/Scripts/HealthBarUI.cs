

using UnityEngine;

using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{

    [SerializeField] Image healthBar;
    void Start()
    {
        Player.Instance.OnHealthChange += Player_OnHealthChange;
    }
    private void Update()
    {
        //if (Player.Instance.OnHealthChange == null)
        //  Player.Instance.OnHealthChange += Player_OnHealthChange;
    }
    private void OnDisable()
    {
        Player.Instance.OnHealthChange -= Player_OnHealthChange;
    }

    private void Player_OnHealthChange(object sender, System.EventArgs e)
    {
        healthBar.fillAmount = (float)Player.Instance.GetHitPoints() / Player.Instance.GetHitPointsMax();
    }
}
