using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonCamera : MonoBehaviour
{
    PlayerInput playerInput;
    [SerializeField] GameObject mainCam;
    [Header("Cinemachine")]
    [SerializeField] CinemachineThirdPersonFollow cinemachineThirdPersonFollow;
    public GameObject CamTarget;
    public float TopClamp;
    public float bottomClamp;
    public float camSensitivity;

    float cinemachineTargetYaw;
    float cinemachineTargetPitch;
    public float CinemachineTargetYaw { set { cinemachineTargetYaw = value; } }
    public float CinemachineTargetPitch { set { cinemachineTargetPitch = value; } }
    float zoomValue;
    Vector2 look;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        playerInput = GameManager.Instance.playerInput;
        cinemachineTargetYaw = 0;
        cinemachineTargetPitch = 20;
    }
    void Start()
    {
        //找攝影機
        if (mainCam == null)
        {
            mainCam = GameObject.FindWithTag("MainCamera");
            //儲存攝影機的Y軸
            cinemachineTargetYaw = mainCam.transform.eulerAngles.y;
        }
        cinemachineTargetYaw = 0;
        cinemachineTargetPitch = 20;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (look.sqrMagnitude >= 0.01f)
        {
            cinemachineTargetYaw += look.x * camSensitivity;
            cinemachineTargetPitch -= look.y * camSensitivity;
        }
        //Yaw處理X軸 Pitch處理Y軸
        cinemachineTargetPitch = Mathf.Clamp(cinemachineTargetPitch, bottomClamp, TopClamp);
        CamTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);
        ZoomView();


    }
    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }
    public void OnZoom(InputAction.CallbackContext ctx)
    {
        zoomValue = ctx.ReadValue<float>();
    }
    public void ZoomView()
    {
        if (zoomValue != 0)
        {
            float endPos = Mathf.Clamp(cinemachineThirdPersonFollow.CameraDistance - zoomValue, 2f, 5f);
            StartCoroutine(ZoomInOut(cinemachineThirdPersonFollow.CameraDistance, endPos, 0.05f));
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
        cinemachineThirdPersonFollow.CameraDistance = endPos;
    }
    public void Stop()
    {
        look=Vector2.zero;
    }
    public void SubAllCameraInput()
    {
        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Look"].canceled += OnLook;
        playerInput.actions["Zoom"].performed += OnZoom;
        playerInput.actions["Zoom"].canceled += OnZoom;
    }

    public void DisSubAllCameraInput()
    {
        playerInput.actions["Look"].performed -= OnLook;
        playerInput.actions["Look"].canceled -= OnLook;
        playerInput.actions["Zoom"].performed -= OnZoom;
        playerInput.actions["Zoom"].canceled -= OnZoom;
    }
}
