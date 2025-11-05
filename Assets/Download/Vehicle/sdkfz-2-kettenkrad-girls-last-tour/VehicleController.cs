using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("車體參數設定")]
    public float maxMotorTorque = 1500f;   // 最大驅動扭力
    public float maxSteeringAngle = 30f;   // 最大轉向角度
    [SerializeField] float breakForce;//煞車力度
    [SerializeField] float maxMotorTurn;  //扭力
    [SerializeField] float minAssistTorque = 200f;      // 停車輔助轉向最低扭力

    [Header("Debug")]
    [SerializeField] float leftTorque;

    [SerializeField] float rightTorque;


    [Header("輪子 Collider")]
    public WheelCollider frontWheel;
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("輪子模型 (可選)")]
    public Transform frontWheelMesh;
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    private float verticalInput;
    private float horizontalInput;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }
    void Update()
    {
        // 接收輸入
        verticalInput = Input.GetAxis("Vertical");   // W/S 或 ↑/↓
        horizontalInput = Input.GetAxis("Horizontal"); // A/D 或 ←/→
    }

    void FixedUpdate()
    {
        // 設定轉向
        float steering = maxSteeringAngle * horizontalInput;
        frontWheel.steerAngle = steering;

        //馬達控制
        HandleMotor();


        // 更新輪子模型的位置與旋轉
        UpdateWheelPose(frontWheel, frontWheelMesh);
        UpdateWheelPose(frontLeftWheel, frontLeftMesh);
        UpdateWheelPose(frontRightWheel, frontRightMesh);
        UpdateWheelPose(rearLeftWheel, rearLeftMesh);
        UpdateWheelPose(rearRightWheel, rearRightMesh);
    }

    void HandleMotor()
    {
        // 設定驅動扭力
        float motor = maxMotorTorque * verticalInput;

        //設定最小扭力，如果沒有前進就設定成minAssistTorque的數值，反之就0
        float assistTorque = Mathf.Abs(verticalInput) < 0.01f ? minAssistTorque : 0f;
        // 設定左右輪扭力加權 (前輪不驅動，只轉向)
        float turnFactor = horizontalInput * maxMotorTurn; // 0.3 表示最大增加/減少 30% 扭力  

        leftTorque = motor * (1 + turnFactor) + assistTorque * horizontalInput;
        rightTorque = motor * (1 - turnFactor) + assistTorque * -horizontalInput;


        frontLeftWheel.motorTorque = leftTorque;
        frontRightWheel.motorTorque = rightTorque;
        rearLeftWheel.motorTorque = leftTorque;
        rearRightWheel.motorTorque = rightTorque;


    }

    private void UpdateWheelPose(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;

        Vector3 pos;
        Quaternion quat;
        col.GetWorldPose(out pos, out quat);
        mesh.position = pos;
        mesh.rotation = quat;
    }


}

