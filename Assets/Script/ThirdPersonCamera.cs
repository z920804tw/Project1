using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] GameObject mainCam;
    [Header("Cinemachine")]
    [SerializeField] CinemachineThirdPersonFollow cinemachineThirdPersonFollow;
    public GameObject CamTarget;
    public float TopClamp;
    public float bottomClamp;
    public float camSensitivity;

    float cinemachineTargetYaw;
    float cinemachineTargetPitch;
    Vector2 look;

    PlayerInputAction inputActions;


    void Awake()
    {
        inputActions = new PlayerInputAction();
    }
    #region - enable disable -
    void OnEnable()
    {
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //找攝影機
        if (mainCam == null)
        {
            mainCam = GameObject.FindWithTag("MainCamera");

            //儲存攝影機的Y軸
            cinemachineTargetYaw = mainCam.transform.eulerAngles.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (look.sqrMagnitude >= 0.01f)
        {
            cinemachineTargetYaw += look.x * camSensitivity;
            cinemachineTargetPitch -= look.y * camSensitivity;
        }
        //Yaw處理X軸 Pitch處理Y軸
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, TopClamp);
        CamTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);
        ZoomView();


    }
    static float ClampAngle(float angle, float min, float max)
    {
        return Mathf.Clamp(angle, min, max);
    }
    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    public void ZoomView()
    {
        float value = inputActions.Player.Zoom.ReadValue<float>();
        if (value != 0)
        {
            float endPos = Mathf.Clamp(cinemachineThirdPersonFollow.CameraDistance - value, 2f, 6f);
            StartCoroutine(ZoomInOut(cinemachineThirdPersonFollow.CameraDistance, endPos, 0.1f));
        }

    }

    IEnumerator ZoomInOut(float startPos, float endPos, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            cinemachineThirdPersonFollow.CameraDistance = Mathf.Lerp(startPos, endPos, timer / duration);
            yield return null;
        }
        cinemachineThirdPersonFollow.CameraDistance =endPos;
    }


}
