using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreCurrencyPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
            RestoreCurrencyManager.Instance.SetRestoreCurrencyPosition(transform.position, SceneManager.GetActiveScene().buildIndex);
    }
}
