
using UnityEngine;



[CreateAssetMenu(fileName = "DialogLineSO", menuName = "DialogLineSO", order = 0)]
public class DialogLineSO : ScriptableObject
{
    [TextArea(0, 4)]
    public string dialogText;

}
