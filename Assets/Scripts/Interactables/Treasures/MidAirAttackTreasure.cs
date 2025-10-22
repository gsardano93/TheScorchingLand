using UnityEngine;

public class MidAirAttackTreasure : Treasure
{
    protected override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt("MidAirAttack") == 1)
            Destroy(gameObject);
    }
    public override void AssignTreasure()
    {
         SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        PlayerPrefs.SetInt("MidAirAttack", 1);
        PlayerMidAirAttack playerMidAirAttack = FindObjectOfType<PlayerMidAirAttack>();
        playerMidAirAttack.enabled = true;
    }
}
