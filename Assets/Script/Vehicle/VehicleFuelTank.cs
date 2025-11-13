using UnityEngine;
using UnityEngine.UI;

public class VehicleFuelTank : MonoBehaviour
{
    [Header("組件設定")]
    [SerializeField] VehicleSetting vehicleSetting;
    [Header("參數設定")]
    [SerializeField] float fuelValue;
    [SerializeField] float maxFuel;
    public float fuelConsumption;
    public float FuelValue { get { return fuelValue; } }
    public float MaxFuel { get { return maxFuel; } }

    [SerializeField] bool haveFuel;
    public bool HaveFuel { get { return haveFuel; } }
    float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fuelValue = maxFuel;
        UpdateFuelCapacity(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!vehicleSetting.IsEngineStart)
        {
            timer = 0;
            return;
        }

        timer += Time.deltaTime;
        if (timer > 10)
        {
            timer = 0;
            UpdateFuelCapacity(-fuelConsumption);
        }
    }

    public void UpdateFuelCapacity(float value)
    {
        fuelValue += value;
        if (fuelValue <= 0)
        {
            fuelValue = 0;
            haveFuel = false;
        }
        else
        {
            haveFuel = true;
        }
    }
}
