using System;

using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }

    private void Player_OnPlayerDeath(object sender, EventArgs e)
    {
        GetComponent<Animator>().SetTrigger("Active");
    }

}
