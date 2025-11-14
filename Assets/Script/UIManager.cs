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
    [SerializeField] TMP_Text hintText;

    public void Awake()
    {
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowPlayerUI(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInteractHint(bool t, string text)
    {
        interactUI.SetActive(t);
        hintText.text = text;
    }
    public void UpdateVehecleUI(float speed, float fuelValue)
    {
        vehicleSpeedText.text=$"CurrentSpeed:{Mathf.RoundToInt(speed)}km/h";
        fuelTankCapacityImg.fillAmount=fuelValue;
    }

    public void ShowVehecleUI(bool t)
    {
        vehicleUI.SetActive(t);
    }
    public void ShowPlayerUI(bool t)
    {
        playerUI.SetActive(t);
    }
}
