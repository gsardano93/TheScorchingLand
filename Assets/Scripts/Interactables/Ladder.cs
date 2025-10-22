
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    bool canInteractAgain;
    void Awake()
    {
        canInteractAgain = true;
    }
    public bool CanInteractAgain()
    {
        return canInteractAgain;
    }

    public void StartInteraction()
    {
        if (!Player.Instance.IsHanged)
        {
            Player.Instance.IsHanged = true;
            Player.Instance.SetCanMove(false);
            Player.Instance.transform.position = new Vector3(transform.position.x,
                                                            Player.Instance.transform.position.y);
        }
        else
            DropLadder();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
            if (Player.Instance.IsHanged)
                DropLadder();
    }
    void DropLadder()
    {
        Player.Instance.IsHanged = false;
        Player.Instance.SetCanMove(true);
        Player.Instance.RestoreGravity();
    }


}
