using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    PlayerInput playerInput;
    [Header("Player UI")]
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject aimHint;
    [Header("物品欄")]
    public HandInventory playerHand;
    public Inventory backpack;
    [Header("Vehicle UI")]
    public GameObject vehicleUI;
    [Header("Interaction UI")]
    public GameObject interactUI;
    [Header("Dialogue UI")]
    public DialogueUI dialogueUI;
    [Header("Quest UI")]
    public QuestUI questUI;
    public void Awake()
    {
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowPlayerUI(true);
        SetInventoryInfo();
        playerInput = GameManager.Instance.playerInput;

    }

    public void ShowInteractHint(bool t)
    {
        interactUI.SetActive(t);
    }
    public void ShowVehecleUI(bool t)
    {
        vehicleUI.SetActive(t);
        CloseHintUI();
    }
    public void ShowPlayerUI(bool t)
    {
        playerUI.SetActive(t);
        CloseHintUI();
    }
    public void ShowDialogueUI(bool t)
    {
        dialogueUI.gameObject.SetActive(t);
        CloseHintUI();
    }
    public void ShowAimHint(bool t)
    {
        aimHint.SetActive(t);
    }
    //----------物品欄-----------//
    public void ShowInventoryUI(bool t)
    {
        inventoryUI.SetActive(t);
        CloseHintUI();
    }
    public void SetInventoryInfo()
    {
        foreach (GameObject i in playerHand.handInventorySlots)
        {
            i.GetComponent<HandInventorySlot>().InitializationInfo();
        }
        foreach (GameObject i in backpack.backpackInventorySlots)
        {
            i.GetComponent<InventorySlot>().InitializationInfo();
        }
        backpack.ResetBackpackSlotInfo();
    }
    //----------物品欄-----------//

    void CloseHintUI()
    {
        if (interactUI.activeSelf)
        {
            interactUI.GetComponent<InteractUI>().ResetHintInfo();
            interactUI.SetActive(false);
        }
    }
}
