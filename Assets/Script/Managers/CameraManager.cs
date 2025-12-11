using UnityEngine;

public enum CameraMode
{
    Normal,
    Aim,
    InVehicle,

}
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] CameraMode currentCameraMode;
    [Header("玩家攝影機")]
    public GameObject playerThirdPersonNormalCam;
    public GameObject playerAimCam;

    [Header("載具攝影機")]
    public GameObject vehicleCam;
    public void Awake()
    {
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCameraMode(CameraMode cameraMode)
    {
        currentCameraMode = cameraMode;
        switch (currentCameraMode)
        {
            case CameraMode.Normal:
                playerThirdPersonNormalCam.SetActive(true);
                playerAimCam.SetActive(false);
                vehicleCam.SetActive(false);
                break;
            case CameraMode.Aim:
                playerThirdPersonNormalCam.SetActive(false);
                playerAimCam.SetActive(true);
                break;

            case CameraMode.InVehicle:
                playerThirdPersonNormalCam.SetActive(false);
                playerAimCam.SetActive(false);
                vehicleCam.SetActive(true);
                break;
        }
    }
}
