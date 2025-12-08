using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PickObject : MonoBehaviour, IInteractable
{
    [Header("組件套用")]
    public SphereCollider triggerBox;
    public Collider collider1;
    Rigidbody rb;
    [Header("參數設定")]
    public ItemSO itemSO;
    string hintText;

    [Header("互動設定")]
    public UnityEvent<GameObject> interactUnityEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hintText = $"{itemSO.hintText} {itemSO.itemName}";
    }
    //-------IInteractable--------//
    public void Interact(GameObject target)
    {
        interactUnityEvent.Invoke(target);
    }
    public string GetHintText()
    {
        return hintText;
    }
    //-------IInteractable--------//

    //新增進物品欄
    public void CollectToInventory(GameObject target)
    {
        //判斷該物品是屬於哪個類別，是可拿在手上還是放在背包
        if (itemSO.itemType == ItemType.HandItem)
        {
            //放到手部欄位
            PlayerInventory playerInventory = target.GetComponent<PlayerStatus>().playerInventory;
            if (playerInventory != null)
            {
                HandInventory playerHand = UIManager.Instance.playerHand;
                if (playerHand.HandSlotAmount < playerHand.handInventorySlots.Count)
                {
                    playerInventory.AddItemToHandInventory(this.gameObject);
                }
                else
                {
                    Debug.Log("物品欄(手部)已滿");
                }
            }
        }
        else
        {
            //收進BackPack欄位中
            PlayerInventory playerInventory = target.GetComponent<PlayerStatus>().playerInventory;
            if (playerInventory != null)
            {
                //檢查背包的當前物件數量是否有沒有滿
                Inventory playerBackpack = UIManager.Instance.backpack;
                if (playerBackpack.CurrentSlotAmount < playerBackpack.backpackInventorySlots.Count)
                {
                    playerInventory.AddItemToBackpackInventory(this.gameObject);
                }
                else
                {
                    Debug.Log("物品欄(背包)已滿");
                }
            }
        }

    }
    public void ColliderAndRig(bool t)
    {
        triggerBox.enabled = t;
        rb.isKinematic = !t;
    }
}
