using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    TextMeshProUGUI dialogText;
    [SerializeField] List<ShopItemSO> itemShopList;
    [SerializeField] Transform shopItemUIPrefab;
    public GameObject merchContainer { get; set; }
    public GameObject soldOutMessage { get; set; }
    Transform descriptionBoxUI;
    public bool IsOpen { get; private set; }
    int x;
    int index;
    public BaseEventData baseEventData;
    private void Start()
    {
        merchContainer = GameObject.Find("MerchContainer");
        soldOutMessage = GameObject.Find("SoldoutMessage");
        dialogText = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        CloseShop();
    }
    public void OpenShop()
    {
        dialogBox.gameObject.SetActive(true);
        merchContainer.gameObject.SetActive(true);
        CleanShop();
        UpdateShop();
        IsOpen = true;
    }
    public void CloseShop()
    {
        dialogBox.gameObject.SetActive(false);
        merchContainer.gameObject.SetActive(false);
        IsOpen = false;
        soldOutMessage.gameObject.SetActive(false);
        // descriptionBoxUI.gameObject.SetActive(false);

    }
    public void UpdateShop()
    {
        x = 0;
       
        CleanShop();
        bool isFirst = true;
        foreach (ShopItemSO itemShop in itemShopList)
        {

            Transform itemShopContainer = Instantiate(shopItemUIPrefab, merchContainer.transform);
            itemShopContainer.GetComponentInChildren<Image>().sprite = itemShop.itemIcon;
            itemShopContainer.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemShop.itemName;
            itemShopContainer.Find("ItemPrice").GetComponent<TextMeshProUGUI>().text = itemShop.itemPrice.ToString();
            Button buyButton = itemShopContainer.GetComponentInChildren<Button>();
            ShowDescriptionUI showDescriptionUI = buyButton.GetComponent<ShowDescriptionUI>();
            showDescriptionUI.Setup(itemShop.itemDescription, dialogBox, dialogText);
            buyButton.onClick.AddListener(() =>
            {
                if (Player.Instance.GetExperience() >= itemShop.itemPrice)
                {
                    itemShop.itemObtained.AssignTreasure();
                    PlayerPrefs.SetInt(itemShop.saveSystemReference, 1);
                    Player.Instance.SetExperience(-itemShop.itemPrice);
                    UpdateShop();
                }
                else
                {
                    dialogText.text = "You don't have enough essence!";
                }
            });
            if (PlayerPrefs.GetInt(itemShop.saveSystemReference) == 1)
                itemShopContainer.gameObject.SetActive(false);
            if (isFirst)
                if (itemShopContainer.gameObject.activeInHierarchy == true)
                {
                    isFirst = false;
                    buyButton.Select();
                }
        }
        foreach (ShopItemSO item in itemShopList)
        {
            x += PlayerPrefs.GetInt(item.saveSystemReference);

        }
        if (x == itemShopList.Count)
        {
            dialogBox.gameObject.SetActive(false);
            soldOutMessage.gameObject.SetActive(true);
        }
    }

    void CleanShop()
    {
        foreach (Transform child in merchContainer.transform)
            Destroy(child.gameObject);
    }
}
