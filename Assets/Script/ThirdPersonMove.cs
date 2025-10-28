using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMove : MonoBehaviour
{
    Vector2 moveInput;
    Vector3 inputDir;
    [Header("物件綁定")]
    public GameObject mainCam;
    Animator animator;
    CharacterController controller;
    [Header("移動參數設定")]
    public float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSmoothTime = 1f;

    bool isRun = false;

    [Header("跳躍參數設定")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -9.81f;

    public LayerMask groundLayer;
    [SerializeField] float sphereRadius;
    [SerializeField] float sphereDistance;
    [SerializeField] bool isGround = false;
    [SerializeField] bool isJump = false;
    [SerializeField] bool wasGround = false; //紀錄上一楨是否在地面上
    Vector3 jumpVelecity;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainCam == null)
        {
            mainCam = GameObject.FindWithTag("MainCamera");
        }
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        isGround = CheckIsGround();



        Move();
        Jump();
        TurnFace();

        LandGround();
        wasGround = isGround;

    }
    //移動方法
    void Move()
    {
        Vector3 moveDir = Vector3.zero;
        if (inputDir != Vector3.zero)
        {
            animator.SetBool("isWalk", true);
            float speed = 0;
            //目標旋轉角度 (input方向轉成角度 + 相機y軸角度)
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            //計算移動方向(將Vector3.forward的Y軸旋轉到目標角度)
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Debug.DrawRay(transform.position, moveDir * 5, Color.red);
            //移動角色
            if (isRun)
            {
                animator.SetBool("isRun", true);
                speed = runSpeed;
            }
            else
            {
                animator.SetBool("isRun", false);
                speed = walkSpeed;
            }
            moveDir = moveDir.normalized * speed;
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
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

    void LandGround()
    {
        if (isJump) //跳躍
        {
            if (isGround && !wasGround)
            {
                isJump = false;
                animator.SetTrigger("isLand");
                Debug.Log("跳起來後降落");
            }
        }
        // else if(!isJump) 
        // {
        //     if (isGround && !wasGround)
        //     {
        //         animator.SetTrigger("isLand");
        //         Debug.Log("掉下去後落地");
        //     }
        // }
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

        animator.SetTrigger("isJump");
        isJump = true;
        float jumpValue = Mathf.Sqrt(-2f * gravity * jumpHeight);
        jumpVelecity = new Vector3(0, jumpValue, 0);
    }
    public void OnRun(InputValue value)
    {
        isRun = !isRun;
    }
}
