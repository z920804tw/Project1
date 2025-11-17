using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public bool canStack;
}
