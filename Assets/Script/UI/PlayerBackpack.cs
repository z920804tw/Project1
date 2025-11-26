using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    PlayerInventorySlot hoverSlot;
    [SerializeField] bool isSwitchItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HoverSlot();
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

    void HoverSlot()
    {
        //取得當前滑鼠指到的物品欄UI
        PointerEventData data = new PointerEventData(eventSystem);
        data.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        foreach (RaycastResult result in results)
        {
            if (hoverSlot == null) //檢查有沒有紀錄，如果沒有就去做檢查有沒有碰到新的Slot
            {
                PlayerInventorySlot slot = result.gameObject.GetComponentInParent<PlayerInventorySlot>();
                if (slot != null)
                {
                    hoverSlot = slot;
                    Image bgImg = slot.bgImg;
                    StartCoroutine(TranslateSlotHoverColor(bgImg, new Color32(255, 190, 108, 200), 0.1f));
                }
                return;
            }
            else
            {
                PlayerInventorySlot slot = result.gameObject.GetComponentInParent<PlayerInventorySlot>();
                if (slot != null)
                {
                    if (slot != hoverSlot)
                    {
                        //先將舊的改回原本顏色
                        StartCoroutine(TranslateSlotHoverColor(hoverSlot.bgImg, new Color32(176, 176, 176, 200), 0.1f));
                        //再來記錄新的
                        hoverSlot = slot;
                        Image bgImg = slot.bgImg;
                        StartCoroutine(TranslateSlotHoverColor(bgImg, new Color32(255, 190, 108, 200), 0.1f));
                    }
                }
                else
                {
                    StartCoroutine(TranslateSlotHoverColor(hoverSlot.bgImg, new Color32(176, 176, 176, 200), 0.1f));
                    hoverSlot = null;
                }
                return;
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
        if (hoverSlot != null)
        {
            hoverSlot.bgImg.color = new Color32(176, 176, 176, 200);
        }

        currentSelectSlot = null;
        hoverSlot = null;
        slotItemName.text = "";
        slotItemDescription.text = "";
    }
    public void AutoArrangeBackpackSlot()
    {
        PlayerInventorySlot targetSlot = null;
        PlayerInventorySlot newSlot = null;
        int currentIndex = 0;
        foreach (GameObject i in backpackInventorySlots)
        {
            //找到目標，並將目標往前方的空位移動
            if (i.GetComponent<PlayerInventorySlot>().isOccupy)
            {
                targetSlot = i.GetComponent<PlayerInventorySlot>();
            }
            else
            {
                currentIndex++;
                continue;
            }

            for (int y = currentIndex; y >= 0; y--)
            {
                if (backpackInventorySlots[y].GetComponent<PlayerInventorySlot>().isOccupy == false)
                {
                    newSlot = backpackInventorySlots[y].GetComponent<PlayerInventorySlot>();
                }
            }
            if (newSlot != null)
            {
                //交換slot內容，將目標slot的資訊轉移到空的newSlot上，並且重製原本的targetSlot資訊
                newSlot.SetSwitchSlotInfo(targetSlot.slotItemSO,targetSlot.Amount);
                targetSlot.InitializationInfo();
                Debug.Log("整理完成");
            }
        }

    }

    IEnumerator TranslateSlotHoverColor(Image target, Color end, float duration)
    {
        float timer = 0;
        Color start = target.color;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            target.color = Color.Lerp(start, end, timer / duration);
            yield return null;
        }

        target.color = end;
    }
}
