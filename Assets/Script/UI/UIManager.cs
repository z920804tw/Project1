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
    [SerializeField] GameObject backpackUI;
    [SerializeField] GameObject aimHint;
    public List<GameObject> handInventorySlots;
    public PlayerBackpack backpack;
    [Header("Vehicle UI")]
    [SerializeField] GameObject vehicleUI;
    [SerializeField] TMP_Text vehicleSpeedText;
    [SerializeField] Image vehicleSpeedImg;

    [SerializeField] TMP_Text fuelTankCapacityText;
    [SerializeField] Image fuelTankCapacityImg;

    [Header("Interaction UI")]
    public GameObject interactUI;
    public List<GameObject> hintUIList;
    [SerializeField] int selectIndex;
    public int SelectIndex { get { return selectIndex; } }
    [SerializeField] Transform content;
    [SerializeField] GameObject hintUI;
    public Transform Content { get { return content; } }

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
        selectIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactUI.activeSelf)
        {
            Vector2 h = hintUI.GetComponent<RectTransform>().sizeDelta;
            h.y = content.GetComponent<RectTransform>().sizeDelta.y;
            hintUI.GetComponent<RectTransform>().sizeDelta = h;
        }
    }

    public void ShowInteractHint(bool t)
    {
        interactUI.SetActive(t);
    }
    public void UpdateVehecleUI(float speed, float maxSpeed, float fuelValue, float maxFuel)
    {
        vehicleSpeedText.text = $"Speed:{Mathf.RoundToInt(speed)}km/h";

        if (maxSpeed != 0) vehicleSpeedImg.fillAmount = speed / maxSpeed;

        fuelTankCapacityText.text = $"{fuelValue}/{maxFuel}";
        fuelTankCapacityImg.fillAmount = fuelValue / maxFuel;
    }

    public void ShowVehecleUI(bool t)
    {
        vehicleUI.SetActive(t);
    }
    public void ShowPlayerUI(bool t)
    {
        playerUI.SetActive(t);
    }
    public void ShowAimHint(bool t)
    {
        aimHint.SetActive(t);
    }
    //----------物品欄-----------//
    public void ShowBackpackUI(bool t)
    {
        backpackUI.SetActive(t);
    }
    public void SetInventoryInfo()
    {
        foreach (GameObject i in handInventorySlots)
        {
            i.GetComponent<PlayerInventorySlot>().InitializationInfo();
        }
        foreach (GameObject i in backpack.backpackInventorySlots)
        {
            i.GetComponent<PlayerInventorySlot>().InitializationInfo();
        }
        backpack.ResetBackpackSlotInfo();
        Debug.Log("物品欄初始化完成");
    }


    //----------物品欄-----------//



    //----------HintUI-----------//
    public void HintUISelect(int value)
    {
        selectIndex += value;
        if (selectIndex >= hintUIList.Count)
        {
            selectIndex = 0;
        }
        else if (selectIndex < 0)
        {
            selectIndex = hintUIList.Count - 1;
        }

        if (hintUIList.Count > 0)
        {
            foreach (GameObject i in hintUIList)
            {
                i.GetComponent<Hint>().ShowSelect(false);
            }
            hintUIList[selectIndex].GetComponent<Hint>().ShowSelect(true);
        }
    }
    //----------HintUI-----------//


    //訂閱其他UI監聽事件
}
