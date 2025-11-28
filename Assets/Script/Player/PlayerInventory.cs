using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("組件套用")]
    [SerializeField] ThirdPersonMove thirdPersonMove;
    [Header("物品欄(手部)參數")]
    public List<GameObject> handSlots;
    [SerializeField] int selectIndex;
    public int SelectIndex { get { return selectIndex; } }
    int slotAmount;
    public int SlotAmount { get { return slotAmount; } set { slotAmount += value; } }
    public GameObject handObj;
    [SerializeField] Transform handTransform;
    [Header("物品欄(背包)參數")]
    public List<GameObject> backpackSlots;
    int backpackAmount;
    public int BackpackAmount { get { return backpackAmount; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectIndex = 0;
        if (UIManager.Instance != null)
        {
            //從UIManager中抓取儲存格
            handSlots = UIManager.Instance.handInventorySlots;
            handSlots[selectIndex].GetComponent<PlayerInventorySlot>().selectImg.SetActive(true);

            backpackSlots=UIManager.Instance.backpack.backpackInventorySlots;
        }

    }

    //---------------(手部)---------------//
    bool CheckHandSameItem(GameObject item)
    {
        foreach (GameObject i in handSlots)
        {
            if (i.GetComponent<PlayerInventorySlot>().IsOccupy)
            {
                //檢查該物品的itemID是否與撿取的一樣，並且檢查是否能夠堆疊
                if (i.GetComponent<PlayerInventorySlot>().ItemID == item.GetComponent<PickObject>().itemSO.itemID && item.GetComponent<PickObject>().itemSO.canStack)
                {
                    i.GetComponent<PlayerInventorySlot>().UpdateInfo(1);
                    Destroy(item);
                    Debug.Log("找到物品欄中已有相同物件，數量+1");
                    return true;
                }
            }
        }
        return false;
    }

    public void AddItemToHandInventory(GameObject item)
    {
        int index = 0;
        bool hasItem = CheckHandSameItem(item);
        if (!hasItem)
        {
            item.SetActive(false);
            foreach (GameObject i in handSlots)
            {
                PlayerInventorySlot slot = i.GetComponent<PlayerInventorySlot>();
                if (!slot.IsOccupy)
                {
                    //設定物品欄位資訊
                    slot.SetInfo(item);
                    //設定物品位置、關閉碰撞
                    item.transform.SetParent(handTransform);
                    item.transform.position = handTransform.position;
                    item.GetComponent<PickObject>().ColliderAndRig(false);
                    if (index == selectIndex)
                    {
                        handObj = item;
                        handObj.SetActive(true);
                    }
                    slotAmount++;
                    Debug.Log("新物件已加入物品欄");
                    return;
                }
                index++;
            }
        }
    }
    public void OnSelect(float value)
    {
        if (!thirdPersonMove.IsAim)
        {
            //先將手中物品設定成null，目的是為了確保如果最後物品欄有空欄位並且選擇框是選在空的欄位上時，可以上handObj也是空的
            handObj = null;
            //先將全部的物品、選取UI都設定關閉
            foreach (GameObject slot in handSlots)
            {
                PlayerInventorySlot pis = slot.GetComponent<PlayerInventorySlot>();
                pis.selectImg.SetActive(false);
                if (pis.slotObject != null)
                {
                    pis.slotObject.SetActive(false);
                }
            }
            //取得滾輪數值，並賦予給selectIndex
            int value1 = (int)value;
            selectIndex += value1;
            if (selectIndex > 2)
            {
                selectIndex = 0;
            }
            else if (selectIndex < 0)
            {
                selectIndex = handSlots.Count - 1;
            }
            //設定當前選擇的物品欄選擇框
            handSlots[selectIndex].GetComponent<PlayerInventorySlot>().selectImg.SetActive(true);
            //如果該物品欄有紀錄東西，就讓該物品顯示
            if (handSlots[selectIndex].GetComponent<PlayerInventorySlot>().slotObject != null)
            {
                handSlots[selectIndex].GetComponent<PlayerInventorySlot>().slotObject.SetActive(true);
                handObj = handSlots[selectIndex].GetComponent<PlayerInventorySlot>().slotObject;
            }
        }
    }
    //---------------(手部)---------------//
    //---------------(背包)---------------//
    bool CheckBackpackSameItem(GameObject item)
    {
        foreach (GameObject i in backpackSlots)
        {
            if (i.GetComponent<PlayerInventorySlot>().IsOccupy)
            {
                //檢查該物品的itemName是否與撿取的一樣，並且檢查是否能夠堆疊
                if (i.GetComponent<PlayerInventorySlot>().ItemID == item.GetComponent<PickObject>().itemSO.itemID && i.GetComponent<PlayerInventorySlot>().CanStack)
                {
                    i.GetComponent<PlayerInventorySlot>().UpdateInfo(1);
                    Destroy(item);
                    Debug.Log("找到物品欄中已有相同物件，數量+1");
                    return true;
                }
            }
        }
        return false;
    }

    public void AddItemToBackpackInventory(GameObject item)
    {
        bool hasItem = CheckBackpackSameItem(item);
        if (!hasItem)
        {
            foreach (GameObject i in backpackSlots)
            {
                PlayerInventorySlot slot = i.GetComponent<PlayerInventorySlot>();
                if (!slot.IsOccupy)
                {
                    //設定物品欄位資訊
                    slot.SetBackpackSlotInfo(item);
                    backpackAmount++;
                    Destroy(item);
                    Debug.Log("新物件已加入物品欄(背包)");
                    return;
                }
            }
        }
    }
    //---------------(背包)---------------//
}
