using TMPro;
using Unity.Properties;
using UnityEngine;

public class PlayerInventorySlot : MonoBehaviour
{
    [Header("物品格子參數狀態")]
    public string type;
    public int amount;
    public bool canStack;
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
            slotObject = null;
            isOccupy = false;
        }

    }
    public void SetInfo(GameObject item)
    {
        isOccupy = true;
        slotObject = item;
        type = item.GetComponent<InteractObject>().itemName;
        canStack = item.GetComponent<InteractObject>().canStack;
        amount = 1;
        amountText.text = $"{amount}";
        imgText.text = $"{type}";
        slotObject.SetActive(false);
    }
}
