using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreCurrencyManager : Singleton<RestoreCurrencyManager>
{
    int currencyToRestore;
    int sceneIndex;
    int sceneIndexOfCurrencyRestorer;
    Vector2 currencyRestorerPosition;
    [SerializeField] Transform currencyRestorerPrefab;
    bool hasRestoreCurrency;

    public void SpawnCurrencyRestorer()
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneIndexOfCurrencyRestorer && hasRestoreCurrency)
        {
            Transform restoreCurrencyTransform = Instantiate(currencyRestorerPrefab, currencyRestorerPosition, Quaternion.identity);
            CurrencyRestorer currencyRestorer = restoreCurrencyTransform.GetComponent<CurrencyRestorer>();
            currencyRestorer.SetCurrency(currencyToRestore);
        }
    }
    public void SetRestoreCurrencyPosition(Vector2 position, int index)
    {
        currencyRestorerPosition = position;
        sceneIndex = index;
    }

    public void OnPlayerDeath()
    {
        Debug.Log("Player died!");
        hasRestoreCurrency = true;
        currencyToRestore = Player.Instance.GetExperience();
        sceneIndexOfCurrencyRestorer = sceneIndex;
    }
    public void SetHasRestoreCurrency(bool hasRestoreCurrency)
    {
        this.hasRestoreCurrency = hasRestoreCurrency;
    }
}
