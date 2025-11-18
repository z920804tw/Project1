using System.Collections;
using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class VehicleSetting : MonoBehaviour
{
    [Header("組件設定")]
    [SerializeField] PlayerInput vehicleInput;
    [SerializeField] VehicleAudio vehicleAudio;
    [SerializeField] VehicleController vehicleController;
    [SerializeField] VehicleFuelTank vehicleFuelTank;
    ThirdPersonCamera thirdPersonCamera;
    [Header("元件套用")]
    [SerializeField] GameObject vehicleCamera;
    [SerializeField] Transform interactPos;
    [SerializeField] Transform exitPos;
    [SerializeField] Transform setPos;
    [SerializeField] Transform lookTarget;
    [Header("DeBug")]
    [SerializeField] GameObject Target;
    [SerializeField] float delayTime;
    [SerializeField] bool isEngineStart;
    public bool IsEngineStart { get { return isEngineStart; } }
    bool hasStart;
    [SerializeField] bool isOccupy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thirdPersonCamera = GetComponent<ThirdPersonCamera>();
        isEngineStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOccupy)
        {
            CheckFuelTankCapacity();
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateVehecleUI(vehicleController.CurrentSpeed, vehicleFuelTank.fuelValue);
            }
        }
    }
    public void VehicleStatus(GameObject target)
    {
        if (!isOccupy)
        {
            //上車動作
            //先將玩家相關設定關閉
            target.GetComponent<PlayerStatus>().SetStatus(Status.InVehicle);
            target.GetComponent<PlayerStatus>().anim.GetOnVehicle(true);

            target.transform.SetParent(setPos);
            target.transform.position = setPos.position;
            target.transform.rotation = setPos.rotation;

            UIManager.Instance.ShowInteractHint(false);
            foreach (GameObject i in UIManager.Instance.hintUIList)
            {
                Destroy(i);
            }
            UIManager.Instance.hintUIList.Clear();
            target.GetComponent<PlayerStatus>().playerInteract.hintGameObjectList.Clear();

            //將載具功能打開
            VehicleStatus(true);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            thirdPersonCamera.CinemachineTargetYaw = 0;
            thirdPersonCamera.CinemachineTargetPitch = 20;

            Target = target;
            isOccupy = true;
        }
        else
        {
            //下車動作
            //將載具功能關閉
            VehicleStatus(false);
            //將玩家相關設定關閉
            target.transform.SetParent(null);
            target.transform.position = exitPos.position;
            target.transform.rotation = exitPos.rotation;

            target.GetComponent<PlayerStatus>().SetStatus(Status.Normal);
            target.GetComponent<PlayerStatus>().anim.GetOnVehicle(false);


            isOccupy = false;
            Target = null;
        }
    }
    void CheckFuelTankCapacity()
    {
        //沒有油就關閉引擎
        if (!vehicleFuelTank.HaveFuel && isEngineStart)
        {
            Debug.Log("沒油，關閉引擎");
            StartCoroutine(DelayVehicleOff(delayTime));
        }
    }

    void VehicleStatus(bool t)
    {
        vehicleInput.enabled = t;
        vehicleController.enabled = t;
        thirdPersonCamera.enabled = t;
        UIManager.Instance.ShowVehecleUI(t);

        vehicleCamera.SetActive(t);
        vehicleCamera.GetComponent<CinemachineCamera>().Follow = lookTarget;
        interactPos.gameObject.SetActive(!t);
    }
    //-------按鍵偵測---------//
    //下車按鍵
    void OnLeave(InputValue value)
    {
        if (isOccupy && vehicleController.CurrentSpeed < 1.5f)
        {
            VehicleStatus(Target);
        }
    }
    //引擎按鍵
    void OnEngineSwitch(InputValue value)
    {
        if (vehicleFuelTank.HaveFuel)
        {
            if (!IsEngineStart && !hasStart)
            {
                hasStart = !isEngineStart;
                StartCoroutine(DelayVehicleStart(delayTime));
                Debug.Log("啟動引擎");
            }
            else if (isEngineStart)
            {
                hasStart = true;
                StartCoroutine(DelayVehicleOff(delayTime));
                Debug.Log("關閉引擎");
            }
        }
        else
        {
            Debug.Log("油箱沒有油，無法啟動引擎");
        }
    }
    //-------按鍵偵測---------//
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
    IEnumerator DelayVehicleStart(float value)
    {
        vehicleAudio.StartEngine(delayTime);
        yield return new WaitForSeconds(value);
        isEngineStart = true;
        hasStart = false;
    }
    IEnumerator DelayVehicleOff(float value)
    {
        vehicleAudio.OffEngine();
        isEngineStart = false;
        yield return new WaitForSeconds(value);
        hasStart = false;
    }
}
