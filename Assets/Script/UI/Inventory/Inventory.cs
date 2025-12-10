using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("背包物品欄參數")]
    public List<GameObject> backpackInventorySlots;
    [SerializeField] TMP_Text slotItemName;
    [SerializeField] TMP_Text slotItemDescription;
    [Header("組件套用")]
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;
    PlayerInput playerInput;
    GameObject currentTarget;

    [Header("Debug")]
    [SerializeField] int currentSlotAmount;
    public int CurrentSlotAmount { get { return currentSlotAmount; } set { currentSlotAmount = value; } }
    [SerializeField] InventorySlot currentSelectSlot;
    [SerializeField] GameObject dragSlot = null;
    [SerializeField] InventorySlot firstSlot = null;
    InventorySlot hoverSlot = null;
    [SerializeField] bool isSwitchItem;
    [SerializeField] bool isDrag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int amount = 0;
        foreach (GameObject slot in backpackInventorySlots)
        {
            if (slot.GetComponent<InventorySlot>().IsOccupy)
            {
                amount++;
            }
        }
        currentSlotAmount = amount;
    }

    // Update is called once per frame
    void Update()
    {
        HoverSlot();
        DragSlot();
    }

    public void SelectBackPackSlot()
    {
        //取得當前滑鼠指到的物品欄UI
        PointerEventData data = new PointerEventData(eventSystem);
        data.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(data, results);
        if (results.Count > 0)
        {
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
                    InventorySlot slot = result.gameObject.GetComponentInParent<InventorySlot>();
                    if (slot != null)
                    {
                        ItemSO temporary = slot.slotItemSO;
                        int amount = slot.Amount;

                        slot.SetSwitchSlotInfo(currentSelectSlot.slotItemSO, currentSelectSlot.Amount);
                        currentSelectSlot.SetSwitchSlotInfo(temporary, amount);

                        SwitchItem(); //重製switch開關
                        SelectBackPackSlot();

                        temporary = null;

                        Destroy(temporary);

                        return;
                    }
                }
            }
            else
            {
                foreach (RaycastResult result in results)
                {
                    InventorySlot slot = result.gameObject.GetComponentInParent<InventorySlot>();
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
        else
        {
            if (currentSelectSlot != null)
            {
                currentSelectSlot.selectImg.SetActive(false);
                currentSelectSlot = null;
            }
        }

    }

    //長按拖曳
    public void DragSlot()
    {
        if (isDrag)
        {
            if (dragSlot == null)
            {
                //取得滑鼠當前的slot物件
                //取得當前滑鼠指到的物品欄UI
                PointerEventData data = new PointerEventData(eventSystem);
                data.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(data, results);
                if (results.Count > 0)
                {
                    foreach (RaycastResult result in results)
                    {
                        InventorySlot slot = result.gameObject.GetComponentInParent<InventorySlot>();
                        if (slot != null && slot.IsOccupy)
                        {
                            dragSlot = Instantiate(slot.gameObject, Input.mousePosition, Quaternion.identity);
                            dragSlot.transform.SetParent(transform);
                            dragSlot.GetComponent<InventorySlot>().bgImg.raycastTarget = false;
                            dragSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);

                            firstSlot = slot;
                            slot.InitializationInfo();
                            return;
                        }
                    }
                }
            }
            else if (dragSlot != null)
            {
                //物件跟隨滑鼠移動
                dragSlot.transform.position = Input.mousePosition;
            }
        }
        else if (!isDrag) //放開
        {
            if (dragSlot != null && firstSlot != null)
            {
                //取得當前滑鼠指到的物品欄UI
                PointerEventData data = new PointerEventData(eventSystem);
                data.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(data, results);
                if (results.Count > 0)
                {
                    foreach (RaycastResult result in results)
                    {
                        InventorySlot newslot = result.gameObject.GetComponentInParent<InventorySlot>();
                        //將拖曳的slot預覽物件資訊給予新的Slot欄位
                        if (newslot != null)
                        {
                            InventorySlot oldSlot = dragSlot.GetComponent<InventorySlot>();
                            if (newslot != firstSlot) //如果放開時的欄位與拖曳的欄位不一樣的話就交換
                            {
                                ItemSO temporary = newslot.slotItemSO;
                                int amount = newslot.Amount;

                                newslot.SetSwitchSlotInfo(oldSlot.slotItemSO, oldSlot.Amount);
                                firstSlot.SetSwitchSlotInfo(temporary, amount);

                                Destroy(dragSlot);
                                dragSlot = null;
                                firstSlot = null;
                            }
                            else //如果是一樣的話就覆蓋
                            {
                                firstSlot.SetSwitchSlotInfo(oldSlot.slotItemSO, oldSlot.Amount);
                                Destroy(dragSlot);
                                dragSlot = null;
                                firstSlot = null;
                            }
                        }
                        return;
                    }
                }
                else //如果是陣列是<=0的話就將拖曳的物件直接回到原本的slot位置上
                {
                    InventorySlot newSlot = dragSlot.GetComponent<InventorySlot>();
                    firstSlot.SetSwitchSlotInfo(newSlot.slotItemSO, newSlot.Amount);
                    Destroy(dragSlot);
                    dragSlot = null;
                    firstSlot = null;
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
        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (hoverSlot == null) //檢查有沒有紀錄，如果沒有就去做檢查有沒有碰到新的Slot
                {
                    InventorySlot slot = result.gameObject.GetComponentInParent<InventorySlot>();
                    if (slot != null)
                    {
                        hoverSlot = slot;
                        Image bgImg = slot.bgImg;
                        StartCoroutine(TranslateSlotHoverColor(bgImg, new Color32(255, 190, 108, 200), 0.1f));
                    }

                }
                else
                {
                    InventorySlot slot = result.gameObject.GetComponentInParent<InventorySlot>();
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
                }
                return;
            }
        }
        else
        {
            if (hoverSlot != null)
            {
                StartCoroutine(TranslateSlotHoverColor(hoverSlot.bgImg, new Color32(176, 176, 176, 200), 0.1f));
                hoverSlot = null;
            }
        }

    }
    //切換物品的按鈕
    public void SwitchItem()
    {
        if (currentSelectSlot != null)
            isSwitchItem = !isSwitchItem;
    }

    public void UseItem()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (currentSelectSlot != null && player != null)
        {
            if (currentSelectSlot.slotItemSO != null && currentSelectSlot.slotItemSO.itemEffectList.Count > 0)
            {
                foreach (IItemEffect effectSo in currentSelectSlot.slotItemSO.itemEffectList)
                {
                    effectSo.ItemEffect(player);
                }
                currentSelectSlot.UpdateInfo(-1);
            }
        }
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
        if (dragSlot != null) Destroy(dragSlot);
        dragSlot = null;
        firstSlot = null;
        currentTarget = null;
        slotItemName.text = "";
        slotItemDescription.text = "";

    }
    public void AutoArrangeBackpackSlot()
    {
        InventorySlot targetSlot = null;
        InventorySlot newSlot = null;
        int currentIndex = 0;
        foreach (GameObject i in backpackInventorySlots)
        {
            //找到目標，並將目標往前方的空位移動
            if (i.GetComponent<InventorySlot>().IsOccupy)
            {
                targetSlot = i.GetComponent<InventorySlot>();
            }
            else
            {
                currentIndex++;
                continue;
            }

            for (int y = currentIndex; y >= 0; y--)
            {
                if (backpackInventorySlots[y].GetComponent<InventorySlot>().IsOccupy == false)
                {
                    newSlot = backpackInventorySlots[y].GetComponent<InventorySlot>();
                }
            }
            if (newSlot != null)
            {
                //交換slot內容，將目標slot的資訊轉移到空的newSlot上，並且重製原本的targetSlot資訊
                newSlot.SetSwitchSlotInfo(targetSlot.slotItemSO, targetSlot.Amount);
                targetSlot.InitializationInfo();
                Debug.Log("整理完成");
            }
        }

    }
    public void SetTarget(GameObject target)
    {
        currentTarget = target;
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
    //------按鍵控制---------//
    public void OnClick(InputAction.CallbackContext ctx)
    {
        SelectBackPackSlot();
    }
    public void OnCloseBackpack(InputAction.CallbackContext ctx)
    {
        //取消背包的輸入訂閱
        DisSubInventoryInput();
        currentTarget.GetComponent<PlayerStatus>().SetStatus(Status.Normal);

        UIManager.Instance.ShowInventoryUI(false);
        ResetBackpackSlotInfo();
        Debug.Log("關閉背包");
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (!isDrag)
        {
            if (ctx.performed)
            {
                isDrag = true;
            }
        }
        else
        {
            if (ctx.canceled)
            {
                isDrag = false;
            }
        }
    }
    //------按鍵控制---------//
    public void SubInventoryInput()
    {
        playerInput = GameManager.Instance.playerInput;
        playerInput.actions["Click"].performed += OnClick;
        playerInput.actions["Click"].canceled += OnClick;

        playerInput.actions["Drag"].performed += OnDrag;
        playerInput.actions["Drag"].canceled += OnDrag;

        playerInput.actions["CloseBackpack"].performed += OnCloseBackpack;
        playerInput.actions["CloseBackpack"].canceled += OnCloseBackpack;

        Debug.Log("監聽背包控制");
    }
    public void DisSubInventoryInput()
    {

        playerInput.actions["Click"].performed -= OnClick;
        playerInput.actions["Click"].canceled -= OnClick;

        playerInput.actions["Drag"].performed -= OnDrag;
        playerInput.actions["Drag"].canceled -= OnDrag;

        playerInput.actions["CloseBackpack"].performed -= OnCloseBackpack;
        playerInput.actions["CloseBackpack"].canceled -= OnCloseBackpack;

        Debug.Log("取消監聽背包控制");
    }


}
