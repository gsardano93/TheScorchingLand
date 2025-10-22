
using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
    public void SetShopActive()
    {
        Transform shopUI = transform.GetChild(0);
        foreach (Transform item in shopUI)
            item.gameObject.SetActive(true);

    }
    public void SetDialogBoxActive()
    {
        Transform dialogBoxUI = transform.GetChild(1);
        dialogBoxUI.gameObject.SetActive(true);
    }
}
