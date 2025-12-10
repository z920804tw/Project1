using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect", menuName = "Item/Effect/New HealEffect")]
public class HealItemSO : ScriptableObject, IItemEffect
{
    public int amount;
    public void ItemEffect(GameObject target)
    {
        Debug.Log($"回復 {target} 的 {amount} HP");
    }
}
