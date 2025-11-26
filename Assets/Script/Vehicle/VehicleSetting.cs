using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleSetting : MonoBehaviour
{
    [Header("組件設定")]
    [SerializeField] VehicleAudio vehicleAudio;
    [SerializeField] VehicleController vehicleController;
    [SerializeField] VehicleFuelTank vehicleFuelTank;
    PlayerInput vehicleInput;
    ThirdPersonCamera thirdPersonCamera;
    [Header("元件套用")]
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
        vehicleInput = GameManager.Instance.playerInput;
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


            //將載具功能打開
            VehicleStatus(true);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            //啟用載具、載具攝影機控制監聽
            GameManager.Instance.SwitchInputMode("Vehicle");
            SubAllVehicleInput();
            thirdPersonCamera.SubAllCameraInput();

            Target = target;
            isOccupy = true;
        }
        else
        {
            //下車動作
            //將載具功能關閉
            DisSubAllVehicleInput();
            thirdPersonCamera.DisSubAllCameraInput();
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
        vehicleController.enabled = t;
        thirdPersonCamera.enabled = t;
        UIManager.Instance.ShowVehecleUI(t);

        CameraManager.Instance.vehicleCam.GetComponent<CinemachineCamera>().Follow = lookTarget;
        interactPos.gameObject.SetActive(!t);
    }
    //-------按鍵偵測---------//
    //下車按鍵
    void OnLeave(InputAction.CallbackContext ctx)
    {
        if (isOccupy && vehicleController.CurrentSpeed < 1.5f)
        {
            VehicleStatus(Target);
        }
    }
    //引擎按鍵
    void OnEngineSwitch(InputAction.CallbackContext ctx)
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

    void OnMove(InputAction.CallbackContext ctx)
    {
        vehicleController.vehicleInput = ctx.ReadValue<Vector2>();
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

    public void SubAllVehicleInput()
    {
        vehicleInput.actions["Move"].performed += OnMove;
        vehicleInput.actions["Move"].canceled += OnMove;

        vehicleInput.actions["Leave"].performed += OnLeave;
        vehicleInput.actions["EngineSwitch"].performed += OnEngineSwitch;
        Debug.Log("監聽車輛控制");
    }

    public void DisSubAllVehicleInput()
    {
        vehicleInput.actions["Move"].performed -= OnMove;
        vehicleInput.actions["Move"].canceled -= OnMove;

        vehicleInput.actions["Leave"].performed -= OnLeave;
        vehicleInput.actions["EngineSwitch"].performed -= OnEngineSwitch;

        Debug.Log("取消監聽車輛控制");
    }
}
