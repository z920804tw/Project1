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

    [Header("互動設定")]
    public UnityEvent<GameObject> unityEvent;
    string hintText;

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
        PlayerInventory playerInventory = target.GetComponent<PlayerStatus>().playerInventory;
        if (playerInventory != null)
        {
            if (playerInventory.SlotAmount < playerInventory.slots.Count)
            {
                playerInventory.AddItemToInventory(this.gameObject);
            }
            else
            {
                Debug.Log("物品欄已滿");
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
        Physics.IgnoreCollision(triggerBox, GameObject.FindWithTag("Player").GetComponent<Collider>(), true);
        rb.AddForce(dir, ForceMode.Impulse);
        StartCoroutine(WaitReset());
    }

    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(boxCollider, GameObject.FindWithTag("Player").GetComponent<Collider>(), false);
        Physics.IgnoreCollision(triggerBox, GameObject.FindWithTag("Player").GetComponent<Collider>(), false);
    }
}
