using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandInventorySlot: MonoBehaviour
{
    [Header("手部物品欄參數")]
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
    public void InitializationInfo()
    {
        isOccupy = false;
        slotItemSO = null;
        slotObject = null;
        itemName = $"";
        itemID = -1;
        itemDescription = $"";
        canStack = false;

        amount = 0;
        amountText.text = $"{amount}";
        imgText.text = $"{itemName}";
    }
}
