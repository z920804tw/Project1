using TMPro;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventorySlot : MonoBehaviour
{
    [Header("物品欄參數")]
    public ItemSO slotItemSO;
    // [SerializeField] Image itemImg;
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
    public bool isOccupy;

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
            amountText.text = $"{0}";
            imgText.text = $"";

            slotItemSO = null;
            slotObject = null;
            itemName = "";
            itemDescription = "";
            isOccupy = false;
            canStack = false;
        }

    }
    public void SetInfo(GameObject item)
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
            Debug.Log("套用舊到新");
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
            Debug.Log("套用新到舊");
        }

    }

    public void InitializationInfo()
    {
        isOccupy=false;
        slotItemSO=null;
        itemName=$"";
        itemID = -1;
        itemDescription=$"";
        canStack=false;

        amount = 0;
        amountText.text = $"{amount}";
        imgText.text = $"{itemName}";
    }
}
