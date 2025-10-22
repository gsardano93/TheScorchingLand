using UnityEngine;

public class EnergyAttackTreasure : Treasure
{
    protected override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt("EnergyAttack") == 1)
            Destroy(gameObject);
    }
    public override void AssignTreasure()
    {
         SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        PlayerPrefs.SetInt("EnergyAttack", 1);
        PlayerEnergyAttack playerEnergyAttack = FindObjectOfType<PlayerEnergyAttack>();
        playerEnergyAttack.enabled = true;
    }
}
