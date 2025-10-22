using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDescriptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    string description;
    GameObject dialogBoxUI;
    TextMeshProUGUI dialogText;

    public void Setup(string description, GameObject dialogBoxUI, TextMeshProUGUI dialogText)
    {
        this.dialogBoxUI = dialogBoxUI;
        this.dialogText = dialogText;
        this.description = description;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        dialogBoxUI.gameObject.SetActive(true);
        dialogText.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
     
        dialogBoxUI.gameObject.SetActive(false);
        dialogText.text = "";
    }

    public void OnSelect(BaseEventData eventData)
    {
      
        dialogBoxUI.gameObject.SetActive(true);
        dialogText.text = description;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        
        dialogBoxUI.gameObject.SetActive(false);
        dialogText.text = "";
    }
}
