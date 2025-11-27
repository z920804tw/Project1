using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public List<ScriptableObject> itemEffectList;
    public int itemID;
    public string itemName;
    public string itemDescription;
    public string hintText;
    public bool canStack;
}
