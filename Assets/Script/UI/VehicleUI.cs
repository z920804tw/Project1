using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleUI : MonoBehaviour
{
    [SerializeField] TMP_Text vehicleName;
    [SerializeField] TMP_Text vehicleSpeedText;
    [SerializeField] Image vehicleSpeedImg;

    [SerializeField] TMP_Text fuelTankCapacityText;
    [SerializeField] Image fuelTankCapacityImg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateVehecleUI(float speed, float maxSpeed, float fuelValue, float maxFuel)
    {
        vehicleSpeedText.text = $"Speed:{Mathf.RoundToInt(speed)}km/h";

        if (maxSpeed != 0) vehicleSpeedImg.fillAmount = speed / maxSpeed;

        fuelTankCapacityText.text = $"{fuelValue}/{maxFuel}";
        fuelTankCapacityImg.fillAmount = fuelValue / maxFuel;
    }

    public void SetVehicleInfo(string name)
    {
        vehicleName.text=name;
    }
}
