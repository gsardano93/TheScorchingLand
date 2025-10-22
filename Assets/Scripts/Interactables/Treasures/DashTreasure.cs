
using UnityEngine;

public class DashTreasure : Treasure
{
    protected override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt("Dash") == 1)
            Destroy(gameObject);
    }
    public override void AssignTreasure()
    {
         SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        PlayerPrefs.SetInt("Dash", 1);
        PlayerDash playerDash = FindObjectOfType<PlayerDash>();
        playerDash.enabled = true;
    }
}
