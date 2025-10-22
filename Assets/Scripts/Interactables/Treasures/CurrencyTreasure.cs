using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyTreasure : Treasure
{
    [SerializeField] string id;
    [SerializeField] int currencyEarned;
    protected override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt("CurrencyTreasure" + id) == 1)
            Destroy(gameObject);
    }
    public override void AssignTreasure()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.GetAudioClipSO().powerUps, transform.position);
        PlayerPrefs.SetInt("CurrencyTreasure" + id, 1);
        Player.Instance.SetExperience(currencyEarned);
    }
}
