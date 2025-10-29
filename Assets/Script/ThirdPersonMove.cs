using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMove : MonoBehaviour
{
    [Header("物件綁定")]
    GameObject mainCam;
    CharacterController controller;
    [Header("移動參數設定")]
    public float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSmoothTime = 1f;
    Vector2 moveInput;
    Vector3 inputDir;
    bool isRun = false;

    [Header("跳躍參數設定")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -9.81f;
    public LayerMask groundLayer;
    [SerializeField] float sphereRadius;
    [SerializeField] float sphereDistance;
    bool isGround = true;
    bool wasGround = true; //紀錄上一楨是否在地面上
    Vector3 jumpVelecity;

    [Header("瞄準")]
    public GameObject followCam;
    public GameObject aimCam;
    [SerializeField] bool isAim;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCam == null)
        {
            mainCam = GameObject.FindWithTag("MainCamera");
        }
        // animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGround = CheckIsGround();

        Move();
        Jump();
        TurnFace();

        wasGround = isGround;

    }
    //移動方法
    void Move()
    {
        Vector3 moveDir = Vector3.zero;

        if (inputDir != Vector3.zero)
        {
            float speed = 0;
            //目標旋轉角度 (input方向轉成角度 + 相機y軸角度)
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            //計算移動方向(將Vector3.forward的Y軸旋轉到目標角度)
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Debug.DrawRay(transform.position, moveDir * 5, Color.red);
            //移動角色
            if (isRun)
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }

            moveDir = moveDir.normalized * speed;
        }
        GetComponent<ThirdPersonAnimation>().MoveAnimState(inputDir, isRun);
        controller.Move(moveDir * Time.deltaTime);


    }
    void Jump()
    {
        jumpVelecity.y += gravity * Time.deltaTime;
        jumpVelecity.y = Mathf.Max(jumpVelecity.y, gravity);
        controller.Move(jumpVelecity * Time.deltaTime);

        GetComponent<ThirdPersonAnimation>().JumpAnimState(isGround, wasGround);
    }


    //轉向方法
    void TurnFace()
    {   
        //瞄準模式
        if (isAim)
        {
            //切換瞄準攝影機
            followCam.SetActive(false);
            aimCam.SetActive(true);
            //產生射線
            Ray ray = new Ray(aimCam.transform.position, aimCam.transform.forward);
            RaycastHit hit;
            Vector3 endPoint;
            if (Physics.Raycast(ray, out hit, 50))
            {
                endPoint = hit.point;
            }
            else
            {
                endPoint = aimCam.transform.forward * 50;
            }
            //設定玩家旋轉
            Vector3 lookDir = endPoint - aimCam.transform.position;
            lookDir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSmoothTime * Time.deltaTime);
            Debug.DrawRay(aimCam.transform.position, lookDir, Color.red);

        }
        else
        {
            //一般移動模式
            if (inputDir != Vector3.zero)
            {
                //目標旋轉角度 (input方向轉成角度 + 相機y軸角度)
                float turnAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
                //角色旋轉和平滑
                Quaternion turnRotation = Quaternion.Euler(0f, turnAngle, 0f);
                transform.rotation = Quaternion.Slerp(transform.rotation, turnRotation, rotationSmoothTime * Time.deltaTime);
            }
            followCam.SetActive(true);
            aimCam.SetActive(false);
        }

    }


    bool CheckIsGround()
    {
        if (Physics.CheckSphere(transform.position + Vector3.down * sphereDistance, sphereRadius, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * sphereDistance, sphereRadius);
    }

    public void OnMove(InputValue value) //新版Input System的移動按鍵偵測
    {
        moveInput = value.Get<Vector2>();

        //將輸入方向轉成世界向量
        inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    }

    public void OnJump(InputValue value)
    {
        if (!isGround) return;
        float jumpValue = Mathf.Sqrt(-2f * gravity * jumpHeight);
        jumpVelecity = new Vector3(0, jumpValue, 0);
    }
    public void OnRun(InputValue value)
    {
        isRun = !isRun;
    }
    public void OnAim(InputValue value)
    {
        isAim = !isAim;
    }
}
