using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<GameObject> inventorySlots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //先跑一遍物品欄，檢查有沒有相同的物件
    bool CheckSameItem(GameObject item)
    {
        foreach (GameObject i in inventorySlots)
        {
            if (i.GetComponent<InventorySlot>().isOccupy)
            {
                if (i.GetComponent<InventorySlot>().type == item.GetComponent<item>().type && item.GetComponent<item>().canStack)
                {
                    i.GetComponent<InventorySlot>().amount += 1;
                    i.GetComponent<InventorySlot>().amountText.text = $"{i.GetComponent<InventorySlot>().amount}";
                    Debug.Log("找到物品欄中已有相同物件，數量+1");
                    return true;
                }
            }
        }
        return false;
    }

    //新增物品進物品欄
    public void AddItemToInventory(GameObject item)
    {
        bool hasItem = CheckSameItem(item);
        if (!hasItem)
        {
            foreach (GameObject i in inventorySlots)
            {
                if (!i.GetComponent<InventorySlot>().isOccupy)
                {
                    i.GetComponent<InventorySlot>().isOccupy = true;
                    i.GetComponent<InventorySlot>().type = item.GetComponent<item>().type;
                    i.GetComponent<InventorySlot>().canStack = item.GetComponent<item>().canStack;
                    i.GetComponent<InventorySlot>().amount = 1;
                    i.GetComponent<InventorySlot>().amountText.text = $"{i.GetComponent<InventorySlot>().amount}";
                    i.GetComponent<InventorySlot>().imgText.text = $"{item.GetComponent<item>().type}";
                    return;
                }
            }
            Debug.Log("新物件已加入物品欄");
        }
    }
}
