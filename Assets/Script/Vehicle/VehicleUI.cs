using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUI : MonoBehaviour
{
    [Header("組件設定")]
    [SerializeField] VehicleController vehicleController;
    [SerializeField] VehicleFuelTank vehicleFuelTank;
    [Header("物件設定")]
    [SerializeField] GameObject vehicleUI;
    [SerializeField] TMP_Text vehicleSpeedText;
    [SerializeField] Image fuelTankCapacityImg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateVehicleInfo();
    }

    void UpdateVehicleInfo()
    {
        vehicleSpeedText.text = $"CurrentSpeed:{Mathf.RoundToInt(vehicleController.CurrentSpeed)}km/h";
        fuelTankCapacityImg.fillAmount = vehicleFuelTank.FuelValue / vehicleFuelTank.MaxFuel;
    }
    public void ShowUI(bool t)
    {
        vehicleUI.SetActive(t);
    }
}
