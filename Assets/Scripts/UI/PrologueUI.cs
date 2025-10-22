using System.Collections;

using TMPro;
using UnityEngine;

public class PrologueUI : MonoBehaviour
{
    [TextArea(0, 4)]
    [SerializeField] string[] stringArray;
    TextMeshProUGUI textBox;
    float textSpeed = 0.025f;
    float timeBetweenSentences = 2f;
    void Start()
    {
        if (PlayerPrefs.GetInt("Prologue") == 1)
        {
            Destroy(gameObject);
            return;
        }

        textBox = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(PrologueRoutine());
    }

    public IEnumerator PrologueRoutine()
    {
        textBox.text = "";
        Player.Instance.SetCanMove(false);
        yield return new WaitForSeconds(1f);
        Player.Instance.SetCanMove(false);
        for (int i = 0; i < stringArray.Length; i++)
        {
            char[] chars = stringArray[i].ToCharArray();
            foreach (char c in chars)
            {
                textBox.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            textBox.text += "\n";
            yield return new WaitForSeconds(timeBetweenSentences);

        }
        GameObject fadeUI = GameObject.Find("FadeUI");
        fadeUI.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        fadeUI.GetComponent<Animator>().SetTrigger("FadeIn");

        Player.Instance.SetCanMove(true);
        PlayerPrefs.SetInt("Prologue", 1);
        Destroy(gameObject);
    }


}
