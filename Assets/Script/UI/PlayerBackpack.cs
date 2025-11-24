using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBackpack : MonoBehaviour
{
    [Header("背包物品欄參數")]
    public List<GameObject> backpackInventorySlots;
    [SerializeField] TMP_Text slotItemName;
    [SerializeField] TMP_Text slotItemDescription;
    [Header("組件套用")]
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    [Header("Debug")]
    [SerializeField] PlayerInventorySlot currentSelectSlot;
    [SerializeField] bool isSwitchItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectBackPackSlot()
    {

        //取得當前滑鼠指到的物品欄UI
        PointerEventData data = new PointerEventData(eventSystem);
        data.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);

        //檢查是否有開啟交換
        if (isSwitchItem)
        {
            if (currentSelectSlot.slotItemSO == null)
            {
                SwitchItem();
                SelectBackPackSlot();
                return;
            }

            foreach (RaycastResult result in results)
            {
                PlayerInventorySlot slot = result.gameObject.GetComponentInParent<PlayerInventorySlot>();
                if (slot != null)
                {
                    PlayerInventorySlot temporary = new PlayerInventorySlot();
                    temporary.slotItemSO = slot.slotItemSO;
                    temporary.Amount = slot.Amount;

                    slot.SetSwitchSlotInfo(currentSelectSlot.slotItemSO, currentSelectSlot.Amount);
                    currentSelectSlot.SetSwitchSlotInfo(temporary.slotItemSO, temporary.Amount);

                    SwitchItem(); //重製switch開關
                    SelectBackPackSlot();

                    Destroy(temporary);

                    return;
                }
            }
        }
        else
        {
            foreach (RaycastResult result in results)
            {
                PlayerInventorySlot slot = result.gameObject.GetComponentInParent<PlayerInventorySlot>();
                if (slot != null)
                {
                    if (currentSelectSlot != null)
                    {
                        currentSelectSlot.selectImg.SetActive(false);
                    }

                    slot.selectImg.SetActive(true);
                    slotItemName.text = slot.ItemName;
                    slotItemDescription.text = slot.ItemDescription;
                    currentSelectSlot = slot;
                    return;
                }
            }
        }

    }
    //切換物品的按鈕
    public void SwitchItem()
    {
        if (currentSelectSlot != null)
            isSwitchItem = !isSwitchItem;
    }

    //重製背包資訊
    public void ResetBackpackSlotInfo()
    {
        if (currentSelectSlot != null)
        {
            currentSelectSlot.selectImg.SetActive(false);
        }

        currentSelectSlot = null;
        slotItemName.text = "";
        slotItemDescription.text = "";
    }
}
