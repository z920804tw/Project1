using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    public PlayerInputAction vehicleInput;
    [Header("車體參數設定")]
    public float maxMotorTorque = 1500f;   // 最大驅動扭力
    public float maxSteeringAngle = 30f;   // 最大轉向角度
    [SerializeField] float forwardMaxSpeed;
    [SerializeField] float backwardMaxSpeed;
    [SerializeField] float maxMotorTurn;  //扭力
    [SerializeField] float breakForce;
    [SerializeField] float slopeTorqueBoost = 1.5f; // 在爬坡時增加多少倍扭力
    [SerializeField] float slopeAngleThreshold = 5f; // 超過多少度視為坡道


    [Header("輪子 Collider")]
    public WheelCollider frontWheel;
    public WheelCollider[] leftWheels;
    public WheelCollider[] rightWheels;

    [Header("輪子模型")]
    [SerializeField] GameObject frontBody;
    public Transform frontWheelMesh;
    public Transform[] leftWheelsTransform;
    public Transform[] rightWheelsTransform;
    Vector2 currentInput;
    Vector2 smoothInputVelocity;
    [Header("Debug")]
    [SerializeField] float leftTorque;
    [SerializeField] float rightTorque;

    [SerializeField] float hInputValue;
    [SerializeField] float vInputValue;
    [SerializeField] float smoothInputSpeed;
    [SerializeField] float rotateValue;
    Rigidbody rb;
    void Awake()
    {
        vehicleInput = new PlayerInputAction();
    }
    void OnEnable()
    {
        vehicleInput.Enable();
    }
    void ODisable()
    {
        vehicleInput.Disable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);

        vInputValue = 0;
        hInputValue = 0;
    }


    void Update()
    {
        // 接收輸入
        VehicleInput();


    }

    void FixedUpdate()
    {
        // 設定轉向
        float steering = maxSteeringAngle * hInputValue;
        frontWheel.steerAngle = steering;
        leftWheels[0].steerAngle = steering;
        rightWheels[0].steerAngle = steering;

        rotateValue = leftWheelsTransform[1].eulerAngles.x;
        frontBody.transform.localRotation = Quaternion.Euler(0, steering, 0);
        UpdateTurnWheelPose(frontWheelMesh, rotateValue);
        UpdateTurnWheelPose(leftWheelsTransform[0].transform, rotateValue);
        UpdateTurnWheelPose(rightWheelsTransform[0].transform, rotateValue);

        //馬達控制 更新輪子模型的位置與旋轉
        HandleMotor();
        BreakVehicle();
        LimitSpeed();


    }
    void VehicleInput()
    {
        currentInput = Vector2.SmoothDamp(currentInput, vehicleInput.Vehicle.Move.ReadValue<Vector2>(), ref smoothInputVelocity, smoothInputSpeed);
        Vector3 move = new Vector3(currentInput.x, 0, currentInput.y);
        hInputValue = Number(move.x);
        vInputValue = Number(move.z);
    }

    float Number(float value)
    {
        if (value > 0.95f)
        {
            value = 1;
        }
        else if (value < -0.95)
        {
            value = -1;
        }
        else if (value < 0.1f && value > -0.1f)
        {
            value = 0;
        }
        return value;
    }

    void HandleMotor()
    {
        //計算角度
        float pitch = Vector3.SignedAngle(Vector3.up, transform.up, transform.right);
        float accelerationMultiplier = 1f;
        if (pitch < -slopeAngleThreshold)
        {
            accelerationMultiplier = slopeTorqueBoost;
        }
        else if (pitch > slopeAngleThreshold)
        {
            accelerationMultiplier = 0.8f;
        }
        // 設定驅動扭力
        float currentAcceleration = accelerationMultiplier * maxMotorTorque * vInputValue;
        float turnFactor = hInputValue * maxMotorTurn; // 0.3 表示最大增加/減少 30% 扭力  

        leftTorque = currentAcceleration * (1 + turnFactor);
        rightTorque = currentAcceleration * (1 - turnFactor);

        //左輪子
        for (int i = 0; i < leftWheels.Length; i++)
        {
            leftWheels[i].motorTorque = leftTorque;
            UpdateWheelPose(leftWheels[i], leftWheelsTransform[i]);
        }

        //右輪
        for (int i = 0; i < rightWheels.Length; i++)
        {
            rightWheels[i].motorTorque = leftTorque;
            UpdateWheelPose(rightWheels[i], rightWheelsTransform[i]);
        }
        // UpdateWheelPose(frontWheel, frontWheelMesh);


    }

    void BreakVehicle()
    {
        float currentBreakForce = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = breakForce;
        }
        else
        {
            if (vInputValue == 0)
            {
                currentBreakForce = 200;
            }
            else
            {
                currentBreakForce = 0;
            }
        }
        foreach (WheelCollider wcollider in leftWheels)
        {
            wcollider.brakeTorque = currentBreakForce;
        }
        foreach (WheelCollider wcollider in rightWheels)
        {
            wcollider.brakeTorque = currentBreakForce;
        }
    }

    void LimitSpeed()
    {
        Vector3 currentSpeed = rb.linearVelocity;
        if (leftTorque > 0 && rightTorque > 0)
        {
            //前進
            if (currentSpeed.magnitude > forwardMaxSpeed)
            {
                Vector3 limitSpeed = currentSpeed.normalized * forwardMaxSpeed;
                rb.linearVelocity = new Vector3(limitSpeed.x, rb.linearVelocity.y, limitSpeed.z);
            }
        }
        else if (leftTorque < 0 && rightTorque < 0)
        {
            //後退
            if (currentSpeed.magnitude > backwardMaxSpeed)
            {
                Vector3 limitSpeed = currentSpeed.normalized * backwardMaxSpeed;
                rb.linearVelocity = new Vector3(limitSpeed.x, rb.linearVelocity.y, limitSpeed.z);
            }
        }
    }

    void UpdateTurnWheelPose(Transform mesh, float rotationValue)
    {
        Quaternion quat = Quaternion.Euler(rotationValue, 0, 0);
        mesh.localRotation = quat;
    }

    private void UpdateWheelPose(WheelCollider col, Transform mesh)
    {
        if (mesh == null) return;

        Vector3 pos;
        Quaternion quat;
        col.GetWorldPose(out pos, out quat);
        mesh.position = pos;



        if (mesh != leftWheelsTransform[0] && mesh != rightWheelsTransform[0])
        {
            mesh.rotation = quat;
        }
    }
}

