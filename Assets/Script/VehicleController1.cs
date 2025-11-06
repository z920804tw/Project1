using Unity.Mathematics;
using UnityEngine;

public class VehicleController1 : MonoBehaviour
{
    [Header("車體參數設定")]
    public float maxMotorTorque = 1500f;   // 最大驅動扭力
    public float maxSteeringAngle = 30f;   // 最大轉向角度
    [SerializeField] float maxMotorTurn;  //扭力

    [Header("Debug")]
    [SerializeField] float leftTorque;

    [SerializeField] float rightTorque;


    [Header("輪子 Collider")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("輪子模型")]
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
        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;

        //馬達控制
        HandleMotor();
        Debug.Log(rb.linearVelocity.magnitude);


        // 更新輪子模型的位置與旋轉
        UpdateWheelPose(frontLeftWheel, frontLeftMesh);
        UpdateWheelPose(frontRightWheel, frontRightMesh);
        UpdateWheelPose(rearLeftWheel, rearLeftMesh);
        UpdateWheelPose(rearRightWheel, rearRightMesh);
    }

    void HandleMotor()
    {
        // 設定驅動扭力
        float motor = maxMotorTorque * verticalInput;
        float turnFactor = horizontalInput * maxMotorTurn; // 0.3 表示最大增加/減少 30% 扭力  

        if (motor != 0)
        {
            leftTorque = motor * (1 + turnFactor);
            rightTorque = motor * (1 - turnFactor);
        }
        else if (motor == 0)
        {
            if (horizontalInput > 0)
            {
                leftTorque = 100;
                rightTorque = 0;
            }
            else if (horizontalInput < 0)
            {
                leftTorque = 0;
                rightTorque = 100;
            }
            else
            {
                leftTorque = 0;
                rightTorque = 0;
            }

        }


        frontLeftWheel.motorTorque = leftTorque;
        frontRightWheel.motorTorque = rightTorque;
        rearLeftWheel.motorTorque = leftTorque;
        rearRightWheel.motorTorque = rightTorque;

        // //設定最小扭力，如果沒有前進就設定成minAssistTorque的數值，反之就0
        // float assistTorque = Mathf.Abs(verticalInput) < 0.01f ? minAssistTorque : 0f;
        // // 設定左右輪扭力加權 (前輪不驅動，只轉向)

    }

    private void UpdateWheelPose(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;

        Vector3 pos;
        Quaternion quat;
        col.GetWorldPose(out pos, out quat);
        mesh.position = pos;

        if (mesh != frontLeftMesh && mesh != frontRightMesh)
        {
            mesh.rotation = quat;
        }
    }
}

