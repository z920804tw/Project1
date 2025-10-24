using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMove : MonoBehaviour
{
    Vector2 move;
    [Header("物件綁定")]
    public GameObject mainCam;
    CharacterController controller;
    [Header("參數設定")]
    public float speed;

    float rotationSmoothTime = 0.1f;
    float rotationVelocity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCam == null)
        {
            mainCam = GameObject.FindWithTag("MainCamera");
        }
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velecity = new Vector3(0, -9.81f, 0);
        if (move != Vector2.zero)
        {
            //輸入方向轉成空間方向
            Vector3 inputDir = new Vector3(move.x, 0, move.y).normalized;
            //目標旋轉角度 (input方向轉成角度 + 相機y軸角度)
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            Debug.Log(targetAngle);
            //角色旋轉和平滑
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);


            //計算移動方向(將Vector3.forward的Y軸旋轉到目標角度)
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Debug.DrawRay(transform.position, moveDir * 5, Color.red);
            //移動角色
            velecity += moveDir.normalized * speed;
        }
        controller.Move(velecity * Time.deltaTime);
    }
    public void OnMove(InputValue value) //新版Input System
    {
        move = value.Get<Vector2>();

    }
}
