using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMove : MonoBehaviour
{
    Vector2 moveInput;
    Vector3 inputDir;
    [Header("物件綁定")]
    public GameObject mainCam;
    CharacterController controller;
    [Header("移動參數設定")]
    public float speed;
    [SerializeField] float rotationSmoothTime = 1f;

    [Header("跳躍參數設定")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -9.81f;
    Vector3 jumpVelecity;


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

        Move();
        Jump();
        TurnFace();

    }
    //移動方法
    void Move()
    {
        Vector3 moveDir = Vector3.zero;
        if (inputDir != Vector3.zero)
        {
            //目標旋轉角度 (input方向轉成角度 + 相機y軸角度)
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            //計算移動方向(將Vector3.forward的Y軸旋轉到目標角度)
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Debug.DrawRay(transform.position, moveDir * 5, Color.red);
            //移動角色
            moveDir = moveDir.normalized * speed;
        }
        controller.Move(moveDir * Time.deltaTime);


    }
    void Jump()
    {
        jumpVelecity.y += gravity * Time.deltaTime;
        jumpVelecity.y = Mathf.Max(jumpVelecity.y, gravity);
        controller.Move(jumpVelecity * Time.deltaTime);
    }

    //轉向方法
    void TurnFace()
    {
        if (inputDir != Vector3.zero)
        {
            //目標旋轉角度 (input方向轉成角度 + 相機y軸角度)
            float turnAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            //角色旋轉和平滑
            Quaternion turnRotation = Quaternion.Euler(0f, turnAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, turnRotation, rotationSmoothTime * Time.deltaTime);
        }
    }

    public void OnMove(InputValue value) //新版Input System的移動按鍵偵測
    {
        moveInput = value.Get<Vector2>();

        //將輸入方向轉成世界向量
        inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    }

    public void OnJump(InputValue value)
    {
        float jumpValue = Mathf.Sqrt(-2f * gravity * jumpHeight);
        jumpVelecity = new Vector3(0, jumpValue, 0);
    }
}
