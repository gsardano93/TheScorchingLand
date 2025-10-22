using UnityEngine;

public class FoodTruck : MonoBehaviour, IInteractable
{
    Shop shop;
    private bool canInteractAgain;
    private void Start()
    {
        shop = GetComponent<Shop>();
        canInteractAgain = true;
    }
    public bool CanInteractAgain()
    {
        return canInteractAgain;
    }

    public void StartInteraction()
    {
        if (shop.merchContainer == null)
        {
            //CanvasSingleton.Instance.SetShopActive();
            shop.merchContainer = GameObject.Find("MerchContainer");
            shop.soldOutMessage = GameObject.Find("SoldoutMessage");
            shop.CloseShop();
        }
        if (!shop.IsOpen)
        {
            Player.Instance.SetCanMove(false);
            shop.OpenShop();
        }
        else
        {
            Player.Instance.SetCanMove(true);
            shop.CloseShop();
        }
    }
}
