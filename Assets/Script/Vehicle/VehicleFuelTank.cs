using UnityEngine;
using UnityEngine.UI;

public class VehicleFuelTank : MonoBehaviour
{
    [Header("組件設定")]
    [SerializeField] VehicleSetting vehicleSetting;
    [Header("參數設定")]
    [SerializeField] float currentFuel;
    public float CurrentFuel { get { return currentFuel; } }
    [SerializeField] float maxFuel;
    public float MaxFuel { get { return maxFuel; } }
    [SerializeField] float fuelConsumption;

    [SerializeField] bool haveFuel;
    public bool HaveFuel { get { return haveFuel; } }
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentFuel = maxFuel;
        UpdateFuelCapacity(0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckHaveFuel();
        
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
        currentFuel += value;
        if (currentFuel <= 0)
        {
            currentFuel = 0;
        }
    }
    void CheckHaveFuel()
    {
        if (currentFuel > 0)
        {
            haveFuel = true;
        }
        else
        {
            haveFuel = false;
        }
    }
}
