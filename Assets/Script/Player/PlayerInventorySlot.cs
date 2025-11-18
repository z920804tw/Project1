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
    [SerializeField] string itemDescription;
    [SerializeField] int amount;
    [SerializeField] bool canStack;
    public bool CanStack { get { return canStack; } }
    public bool isOccupy;

    [Header("物件套用")]
    public GameObject slotObject;
    public GameObject selectImg;
    public TMP_Text amountText;
    public TMP_Text imgText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        amountText.text = $"{amount}";
        imgText.text = "";
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
            isOccupy = false;
        }

    }
    public void SetInfo(GameObject item)
    {
        isOccupy = true;
        slotObject = item;
        slotItemSO = item.GetComponent<PickObject>().itemSO;
        itemName = slotItemSO.itemName;
        itemDescription = slotItemSO.itemDescription;
        canStack = slotItemSO.canStack;

        amount = 1;
        amountText.text = $"{amount}";
        imgText.text = $"{itemName}";
        slotObject.SetActive(false);
    }

    public void SetBackpackSlotInfo(GameObject item)
    {
        isOccupy=true;
        slotItemSO=item.GetComponent<PickObject>().itemSO;
        itemName=item.GetComponent<PickObject>().itemSO.itemName;
        itemDescription=item.GetComponent<PickObject>().itemSO.itemDescription;
        canStack=item.GetComponent<PickObject>().itemSO.canStack;

        amount=1;
        amountText.text=$"{amount}";
        imgText.text=$"{itemName}";
    }
}
