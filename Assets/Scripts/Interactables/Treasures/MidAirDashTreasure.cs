using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidAirDashTreasure : Treasure
{
    protected override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt("MidAirDash") == 1)
            Destroy(gameObject);
    }
    public override void AssignTreasure()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        PlayerPrefs.SetInt("MidAirDash", 1);
        PlayerMidAirDash playerMidAirAttack = FindObjectOfType<PlayerMidAirDash>();
        playerMidAirAttack.enabled = true;
    }
}
