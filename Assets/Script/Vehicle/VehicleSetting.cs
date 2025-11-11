using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleSetting : MonoBehaviour
{
    [Header("組件設定")]
    [SerializeField] PlayerInput vehicleInput;
    VehicleController vehicleController;
    ThirdPersonCamera thirdPersonCamera;
    [SerializeField] GameObject vehicleCamera;
    [SerializeField] Transform interactPos;
    [SerializeField] Transform exitPos;
    [SerializeField] Transform setPos;
    [Header("DeBug")]
    [SerializeField] GameObject Target;
    public bool isOccupy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vehicleController = GetComponent<VehicleController>();
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

            vehicleInput.enabled = true;
            vehicleController.enabled = true;
            thirdPersonCamera.enabled = true;
            thirdPersonCamera.CinemachineTargetYaw = 0;
            thirdPersonCamera.CinemachineTargetPitch = 20;
            vehicleCamera.SetActive(true);
            interactPos.gameObject.SetActive(false);
            isOccupy = true;
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
            interactPos.gameObject.SetActive(true);


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
}
