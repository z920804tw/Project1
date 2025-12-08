using System.Collections.Generic;
using UnityEngine;

public class HandInventory : MonoBehaviour
{
    public List<GameObject> handInventorySlots;
    [SerializeField] int handSlotAmount;
    public int HandSlotAmount{get{return handSlotAmount;}set{handSlotAmount=value;}}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handInventorySlots[0].GetComponent<HandInventorySlot>().selectImg.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
