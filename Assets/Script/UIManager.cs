using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Player UI")]
    [SerializeField] GameObject playerUI;
    public List<GameObject> inventorySlots;
    [Header("Vehicle UI")]
    [SerializeField] GameObject vehicleUI;
    [SerializeField] TMP_Text vehicleSpeedText;
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
        selectIndex=0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 h= hintUI.GetComponent<RectTransform>().sizeDelta;
        h.y=content.GetComponent<RectTransform>().sizeDelta.y;
        hintUI.GetComponent<RectTransform>().sizeDelta=h;
    }

    public void ShowInteractHint(bool t)
    {
        interactUI.SetActive(t);
    }
    public void UpdateVehecleUI(float speed, float fuelValue)
    {
        vehicleSpeedText.text = $"CurrentSpeed:{Mathf.RoundToInt(speed)}km/h";
        fuelTankCapacityImg.fillAmount = fuelValue;
    }

    public void ShowVehecleUI(bool t)
    {
        vehicleUI.SetActive(t);
    }
    public void ShowPlayerUI(bool t)
    {
        playerUI.SetActive(t);
    }

    public void UpdateSelect(int value)
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
}
