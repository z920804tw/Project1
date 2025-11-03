using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [Header("物品格子參數狀態")]
    public string type;
    public int amount;
    public bool canStack;
    public bool isOccupy;

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
}
