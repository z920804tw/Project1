using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect", menuName = "Item/Effect/New FoodEffect")]
public class FoodItemSO : ScriptableObject, IItemEffect
{
    public int amount;
    public void ItemEffect()
    {
        Debug.Log($"回復 {amount} 飽食度");
    }
}
