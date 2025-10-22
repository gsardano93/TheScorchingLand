
using UnityEngine;

[CreateAssetMenu()]
public class ShopItemSO : ScriptableObject
{
  	public string saveSystemReference;
	public string itemName;
	public Sprite itemIcon;
	[TextArea(2,6)]
	public string itemDescription;
	public int itemPrice;
	public Treasure itemObtained;
}
