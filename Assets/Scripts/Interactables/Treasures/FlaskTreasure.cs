
using UnityEngine;

public class FlaskTreasure : Treasure
{
    [SerializeField] string id;
    protected override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt("Flask" + id) == 1)
            Destroy(gameObject);
    }
    public override void AssignTreasure()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        Player.Instance.UpgradeFlasks(1);
        PlayerPrefs.SetInt("Flask" + id, 1);
    }
}
