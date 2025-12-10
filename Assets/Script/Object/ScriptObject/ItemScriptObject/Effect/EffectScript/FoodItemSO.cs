using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect", menuName = "Item/Effect/New FoodEffect")]
public class FoodItemSO : ScriptableObject, IItemEffect
{
    public int amount;
    public void ItemEffect(GameObject target)
    {
        Debug.Log($"回復 {target} 的 {amount} 飽食度");
    }
}
