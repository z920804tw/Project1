using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleSetting : MonoBehaviour
{
    [Header("組件設定")]
    [SerializeField] PlayerInput vehicleInput;
    [SerializeField] VehicleAudio vehicleAudio;
    [SerializeField] VehicleController vehicleController;
    [SerializeField] VehicleUI vehicleUI;
    ThirdPersonCamera thirdPersonCamera;
    [Header("元件套用")]
    [SerializeField] GameObject vehicleCamera;
    [SerializeField] Transform interactPos;
    [SerializeField] Transform exitPos;
    [SerializeField] Transform setPos;
    [Header("DeBug")]
    [SerializeField] GameObject Target;
    [SerializeField] float delayTime;
    public bool isOccupy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thirdPersonCamera = GetComponent<ThirdPersonCamera>();
    }

    // Update is called once per frame
    void Update()
    {

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


            //將載具功能打開

            StartCoroutine(DelayVehicleStart(delayTime));
            vehicleInput.enabled = true;

            thirdPersonCamera.enabled = true;
            thirdPersonCamera.CinemachineTargetYaw = 0;
            thirdPersonCamera.CinemachineTargetPitch = 20;
            vehicleCamera.SetActive(true);

            vehicleUI.ShowUI(true);
            interactPos.gameObject.SetActive(false);

            vehicleAudio.StartEngine(delayTime);
            Target = target;
        }
        else
        {
            //下車動作
            //將載具功能關閉
            vehicleInput.enabled = false;
            vehicleController.enabled = false;
            thirdPersonCamera.enabled = false;
            vehicleCamera.SetActive(false);
            vehicleUI.ShowUI(false);

            StartCoroutine(DelayVehicleOff(delayTime));

            vehicleAudio.OffEngine();

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

    void OnLeave(InputValue value)
    {
        if (isOccupy && vehicleController.CurrentSpeed < 1.5f)
        {
            VehicleStatus(Target);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    IEnumerator DelayVehicleStart(float value)
    {
        yield return new WaitForSeconds(value);
        vehicleController.enabled = true;
        isOccupy = true;
    }
    IEnumerator DelayVehicleOff(float value)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(value);
        GetComponent<Rigidbody>().isKinematic = false;
        interactPos.gameObject.SetActive(true);
        isOccupy = false;
    }
}
