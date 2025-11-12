using TMPro;
using UnityEngine;

public class VehicleUI : MonoBehaviour
{
    [SerializeField] GameObject vehicleUI;
    [SerializeField] TMP_Text vehicleSpeedText;
    [SerializeField] VehicleController vehicleController;
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
        vehicleSpeedText.text = $"CurrentSpeed:{Mathf.RoundToInt(vehicleController.CurrentSpeed)}";
    }
    public void ShowUI(bool t)
    {
        vehicleUI.SetActive(t);
    }
}
