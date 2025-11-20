using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PickObject : MonoBehaviour, IInteractable
{
    [Header("組件套用")]
    public SphereCollider triggerBox;
    public BoxCollider boxCollider;
    Rigidbody rb;
    [Header("參數設定")]
    public ItemSO itemSO;
    string hintText;

    [Header("互動設定")]
    public UnityEvent<GameObject> unityEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hintText = $"{itemSO.hintText} {itemSO.itemName}";
    }
    //-------IInteractable--------//
    public void Interact(GameObject target)
    {
        unityEvent.Invoke(target);
    }
    public string GetHintText()
    {
        return hintText;
    }
    //-------IInteractable--------//

    //新增進物品欄
    public void CollectToInventory(GameObject target)
    {
        //判斷該物品是屬於哪個類別，是可拿在手上還是只能放在背包
        if (itemSO.itemType == ItemType.PickItem)
        {
            //放到手部欄位
            PlayerInventory playerInventory = target.GetComponent<PlayerStatus>().playerInventory;
            if (playerInventory != null)
            {
                if (playerInventory.SlotAmount < playerInventory.handSlots.Count)
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
                if (playerInventory.BackpackAmount < playerInventory.backpackSlots.Count)
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

    public void Throw(Vector3 dir)
    {
        ColliderAndRig(true);
        Physics.IgnoreCollision(boxCollider, GameObject.FindWithTag("Player").GetComponent<Collider>(), true);
        Physics.IgnoreCollision(triggerBox, GameObject.FindWithTag("PlayerInteract").GetComponent<Collider>(), true);
        rb.AddForce(dir, ForceMode.Impulse);
        StartCoroutine(WaitReset());
    }

    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(boxCollider, GameObject.FindWithTag("Player").GetComponent<Collider>(), false);
        Physics.IgnoreCollision(triggerBox, GameObject.FindWithTag("PlayerInteract").GetComponent<Collider>(), false);
    }
}
