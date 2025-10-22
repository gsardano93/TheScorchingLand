using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyRestorer : MonoBehaviour, IInteractable
{
    public static CurrencyRestorer Instance;
    bool canInteractAgain;
    int currencyToRestore;
    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        canInteractAgain = true;
    }
    public bool CanInteractAgain()
    {
        return canInteractAgain;
    }
    public void StartInteraction()
    {
         GameObject essenceRecoveredUI = GameObject.Find("EssenceRecoveredUI");
        essenceRecoveredUI.GetComponent<Animator>().SetTrigger("EssenceRecovered");
        Player.Instance.SetExperience(currencyToRestore);
        RestoreCurrencyManager.Instance.SetHasRestoreCurrency(false);
        Destroy(gameObject);
    }

    public void SetCurrency(int currencyToRestore)
    {
        this.currencyToRestore = currencyToRestore;
    }
}
