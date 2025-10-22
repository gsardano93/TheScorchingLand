using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    [SerializeField] GameObject brokenObject;
    [SerializeField] GameObject fixedObject;
    [SerializeField] string identifierString;
    private void Start()
    {
        if (PlayerPrefs.GetInt("BrokenObject" + identifierString) == 1)
            FixObject();
    }

    public void FixObject()
    {
        brokenObject.SetActive(false);
        fixedObject.SetActive(true);
        PlayerPrefs.SetInt("BrokenObject" + identifierString, 1);
    }
}
