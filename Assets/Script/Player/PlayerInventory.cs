using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("組件套用")]
    [SerializeField] ThirdPersonMove thirdPersonMove;
    [SerializeField] ThirdPersonAnimation anim;
    [Header("物品欄(手部))參數")]
    public List<GameObject> slots;
    [SerializeField] Transform rightHand;
    [SerializeField] int selectIndex;
    int pickAmount;
    public GameObject handObj;
    [SerializeField] PickObject pickObj;

    Vector3 placePos;

    bool canPick;
    [SerializeField] bool canPlace;
    [SerializeField] bool canThrow;

    GameObject mainCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        selectIndex = 0;
        slots[selectIndex].GetComponent<PlayerInventorySlot>().selectImg.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        PlaceAndThrow();

    }

    void PlaceAndThrow()
    {
        if (handObj != null)
        {
            if (thirdPersonMove.IsAim)
            {
                Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 5f))
                {
                    canPlace = true;
                    canThrow = false;
                    placePos = hit.point;
                    placePos.y += 1f;
                }
                else
                {
                    canPlace = false;
                    canThrow = true;
                }
                anim.ThrowAnim(true, false);
            }
            else if (!thirdPersonMove.IsAim)
            {
                canPlace = false;
                canThrow = false;
                anim.ThrowAnim(false, false);
            }
        }
    }


    //檢查物品欄是否有相同物品
    bool CheckSameItem(GameObject item)
    {
        foreach (GameObject i in slots)
        {
            if (i.GetComponent<PlayerInventorySlot>().isOccupy)
            {
                //檢查該物品的itemName是否與撿取的一樣，並且檢查是否能夠堆疊
                if (i.GetComponent<PlayerInventorySlot>().type == item.GetComponent<PickObject>().itemName && item.GetComponent<PickObject>().canStack)
                {
                    i.GetComponent<PlayerInventorySlot>().UpdateInfo(1);
                    Destroy(i);
                    Debug.Log("找到物品欄中已有相同物件，數量+1");
                    return true;
                }
            }
        }
        return false;
    }

    public void AddItemToInventory(GameObject item)
    {
        int index = 0;
        bool hasItem = CheckSameItem(item);
        if (!hasItem)
        {
            foreach (GameObject i in slots)
            {
                PlayerInventorySlot slot = i.GetComponent<PlayerInventorySlot>();
                if (!slot.isOccupy)
                {
                    //設定物品欄位資訊
                    slot.SetInfo(item);
                    if (index == selectIndex)
                    {
                        handObj = item;
                        handObj.SetActive(true);
                    }
                    Debug.Log("新物件已加入物品欄");
                    return;
                }
                index++;
            }


        }
    }
    // //按鍵偵測(撿取、放置、丟)
    public void OnPick(InputValue value)
    {
        if (canPick)
        {
            if (pickAmount < slots.Count)
            {
                pickObj.transform.SetParent(rightHand);
                pickObj.transform.position = rightHand.transform.position;
                pickObj.ColliderAndRig(false);
                pickObj.ShowCloseInfo(false);
                AddItemToInventory(pickObj.gameObject);

                pickAmount++;

                pickObj = null;
                canPick = false;
            }
            else
            {
                Debug.Log("物品欄已滿無法撿取，請丟棄一項物品");
            }

        }
    }

    public void OnPlace(InputValue value)
    {
        if (canPlace)
        {
            handObj.transform.SetParent(null);
            handObj.transform.position = placePos;
            handObj.GetComponent<PickObject>().ColliderAndRig(true);
            handObj = null;

            anim.ThrowAnim(false, false);
            slots[selectIndex].GetComponent<PlayerInventorySlot>().UpdateInfo(-1);
            pickAmount--;
            canPlace = false;
            Debug.Log("放置");
        }
    }
    public void OnThrow(InputValue value)
    {
        if (canThrow)
        {
            handObj.transform.SetParent(null);
            handObj.GetComponent<PickObject>().Throw(mainCam.transform.forward * 10 + transform.up * 5f);
            handObj = null;
            canThrow = false;
            anim.ThrowAnim(true, true);
            //更新當前選取slot的資訊
            slots[selectIndex].GetComponent<PlayerInventorySlot>().UpdateInfo(-1);
            //扣除撿取上限
            pickAmount--;
            Debug.Log("丟出物品");
        }
    }

    public void OnSelect(InputValue value)
    {
        if (!thirdPersonMove.IsAim)
        {
            //先將手中物品設定成null，目的是為了確保如果最後物品欄有空欄位並且選擇框是選在空的欄位上時，可以上handObj也是空的
            handObj = null;
            //先將全部的物品都設定關閉
            foreach (GameObject slot in slots)
            {
                PlayerInventorySlot pis = slot.GetComponent<PlayerInventorySlot>();
                pis.selectImg.SetActive(false);
                if (pis.slotObject != null)
                {
                    pis.slotObject.SetActive(false);
                }
            }
            //取得滾輪數值，並賦予給selectIndex
            int value1 = (int)value.Get<float>();
            selectIndex += value1;
            if (selectIndex > 2)
            {
                selectIndex = 0;
            }
            else if (selectIndex < 0)
            {
                selectIndex = slots.Count - 1;
            }
            //設定當前選擇的物品欄選擇框
            slots[selectIndex].GetComponent<PlayerInventorySlot>().selectImg.SetActive(true);
            //如果該物品欄有紀錄東西，就讓該物品顯示
            if (slots[selectIndex].GetComponent<PlayerInventorySlot>().slotObject != null)
            {
                slots[selectIndex].GetComponent<PlayerInventorySlot>().slotObject.SetActive(true);
                handObj = slots[selectIndex].GetComponent<PlayerInventorySlot>().slotObject;
            }
        }
    }

    // //偵測可以撿取物件
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickObject"))
        {
            if (other.GetComponent<PickObject>() != null && pickObj == null)
            {
                pickObj = other.gameObject.GetComponent<PickObject>();
                pickObj.ShowCloseInfo(true);

                if (pickObj.CanPick)
                {
                    canPick = true;
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickObject"))
        {
            if (pickObj != null && pickObj.name == other.name)
            {
                pickObj.GetComponent<PickObject>().ShowCloseInfo(false);
                pickObj = null;

                canPick = false;

            }
        }
    }
}
