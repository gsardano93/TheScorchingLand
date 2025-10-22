
using UnityEngine;

public class PlayerEnergyAttack : PlayerAbility
{
    [SerializeField] Transform energyGlobe;
    [SerializeField] Transform energyGlobeSpawnPoint;


    protected override void Awake()
    {
        base.Awake();
        x = PlayerPrefs.GetInt("EnergyAttack");
        if (x == 0)
            enabled = false;
        inputActions.Player.EnergyAttack.performed += _ => EnergyAttack();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
    private void EnergyAttack()
    {
        if (cooldownTimer > cooldownTimerMax && player.IsGrounded && player.CanMove)
        {
            player.Stop();
            player.SetCanMove(false);
            cooldownTimer = 0;
            animator.SetTrigger("EnergyAttack");
        }
    }
    void InstatiateEnergyGlobe()
    {
        Instantiate(energyGlobe, energyGlobeSpawnPoint.position, Quaternion.identity);
    }
    void EndAttack()
    {
        player.SetCanMove(true);
    }
}
