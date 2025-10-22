
using UnityEngine;

public class InstaDeath : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
            player.ReceiveDamage(Player.Instance.GetHitPoints());
           
        if(other.TryGetComponent(out Enemy enemy))
            enemy.ReceiveDamage(enemy.GetHitPoints(),transform);

    }
}
