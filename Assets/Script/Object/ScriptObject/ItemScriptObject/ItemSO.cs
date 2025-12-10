using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public Image itemImage;
    public List<ScriptableObject> itemEffectList;
    public int itemID;
    public string itemName;
    public string itemDescription;
    public string hintText;
    public bool canStack;
}
