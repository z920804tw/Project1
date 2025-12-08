using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("物品欄參數")]
    public ItemSO slotItemSO;
    [SerializeField] string itemName;
    public string ItemName { get { return itemName; } }
    [SerializeField] int itemID;
    public int ItemID { get { return itemID; } }
    [SerializeField] string itemDescription;
    public string ItemDescription { get { return itemDescription; } }
    [SerializeField] int amount;
    public int Amount { get { return amount; } set { amount = value; } }
    [SerializeField] bool canStack;
    public bool CanStack { get { return canStack; } }
    bool isOccupy;
    public bool IsOccupy { get { return isOccupy; } }

    [Header("物件套用")]
    public GameObject slotObject;
    public GameObject selectImg;
    public Image bgImg;
    public TMP_Text amountText;
    public TMP_Text imgText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateInfo(int value)
    {
        amount += value;
        if (amount > 0)
        {
            amountText.text = $"{amount}";
        }
        else if (amount <= 0)
        {
            InitializationInfo();
            UIManager.Instance.backpack.CurrentSlotAmount=-1;
        }

    }
    public void SetHandSlotInfo(GameObject item)
    {
        isOccupy = true;
        slotObject = item;
        slotItemSO = item.GetComponent<PickObject>().itemSO;
        itemName = slotItemSO.itemName;
        itemID = slotItemSO.itemID;
        itemDescription = slotItemSO.itemDescription;
        canStack = slotItemSO.canStack;

        amount = 1;
        amountText.text = $"{amount}";
        imgText.text = $"{itemName}";
    }

    public void SetBackpackSlotInfo(GameObject item)
    {
        isOccupy = true;
        slotItemSO = item.GetComponent<PickObject>().itemSO;

        itemName = item.GetComponent<PickObject>().itemSO.itemName;
        itemID = slotItemSO.itemID;
        itemDescription = item.GetComponent<PickObject>().itemSO.itemDescription;
        canStack = item.GetComponent<PickObject>().itemSO.canStack;

        amount = 1;
        amountText.text = $"{amount}";
        imgText.text = $"{itemName}";
    }

    public void SetSwitchSlotInfo(ItemSO itemSO, int amountValue)
    {
        if (itemSO != null)
        {
            isOccupy = true;
            slotItemSO = itemSO;
            itemName = slotItemSO.itemName;
            itemID = slotItemSO.itemID;
            itemDescription = slotItemSO.itemDescription;
            canStack = slotItemSO.canStack;

            amount = amountValue;
            amountText.text = $"{amount}";
            imgText.text = $"{itemName}";
            Debug.Log("兩個欄位資訊互換(有東西)");
        }
        else
        {
            isOccupy = false;
            slotItemSO = null;
            itemName = $"";
            itemID = -1;
            itemDescription = $"";
            canStack = false;

            amount = 0;
            amountText.text = $"{amount}";
            imgText.text = $"{itemName}";
            Debug.Log("兩個欄位資訊互換(沒東西)");
        }

    }

    public void InitializationInfo()
    {
        isOccupy = false;
        slotItemSO = null;
        slotObject=null;
        itemName = $"";
        itemID = -1;
        itemDescription = $"";
        canStack = false;

        amount = 0;
        amountText.text = $"{amount}";
        imgText.text = $"{itemName}";
    }

}
