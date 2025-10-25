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
    float zoomValue;
    Vector2 look;
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
    public void OnZoom(InputValue value)
    {
        zoomValue = value.Get<float>();
    }
    public void ZoomView()
    {
        if (zoomValue != 0)
        {
            float endPos = Mathf.Clamp(cinemachineThirdPersonFollow.CameraDistance - zoomValue, 2f, 6f);
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


}
