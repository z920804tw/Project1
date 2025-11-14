using Unity.VisualScripting;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public PlayerInputAction vehicleInput;
    [SerializeField] VehicleSetting vehicleSetting;
    [SerializeField] Rigidbody rb;
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
    [Header("Debug")]
    [SerializeField] float leftTorque;
    [SerializeField] float rightTorque;
    [SerializeField] float smoothInputSpeed;
    [SerializeField] float currentSpeed;
    public float CurrentSpeed { get { return currentSpeed; } }
    float horizontalInput;
    float verticalInput;
    [SerializeField] Vector2 currentInput;
    public Vector2 CurrentInput { get { return currentInput; } }
    Vector2 smoothInputVelocity;

    [SerializeField] float pitch;
    float rotateValue;

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
        rb.centerOfMass = new Vector3(0, -0.5f, 0);

        horizontalInput = 0;
        verticalInput = 0;
    }


    void Update()
    {
        // 接收輸入
        VehicleInput();
    }

    void FixedUpdate()
    {
        // 設定轉向
        float steering = maxSteeringAngle * horizontalInput;
        frontWheel.steerAngle = steering;
        leftWheels[0].steerAngle = steering;
        rightWheels[0].steerAngle = steering;

        rotateValue = leftWheelsTransform[1].eulerAngles.x;
        frontBody.transform.localRotation = Quaternion.Euler(0, steering, 0);
        UpdateTurnWheelPose(frontWheelMesh, rotateValue);
        UpdateTurnWheelPose(leftWheelsTransform[0].transform, rotateValue);
        UpdateTurnWheelPose(rightWheelsTransform[0].transform, rotateValue);

        //左輪子
        for (int i = 0; i < leftWheels.Length; i++)
        {
            UpdateWheelPose(leftWheels[i], leftWheelsTransform[i]);
        }

        //右輪
        for (int i = 0; i < rightWheels.Length; i++)
        {
            UpdateWheelPose(rightWheels[i], rightWheelsTransform[i]);
        }

        //車輛速度控制

        HandleMotor();
        BreakVehicle();
        LimitSpeed();
    }
    void VehicleInput()
    {
        currentInput = Vector2.SmoothDamp(currentInput, vehicleInput.Vehicle.Move.ReadValue<Vector2>(), ref smoothInputVelocity, smoothInputSpeed);

        currentInput.x = Number(currentInput.x);
        currentInput.y = Number(currentInput.y);


        Vector3 move = new Vector3(currentInput.x, 0, currentInput.y);
        horizontalInput = move.x;
        verticalInput = move.z;
    }

    float Number(float value)
    {
        if (value > 0.997f)
        {
            value = 1;
        }
        else if (value < -0.997)
        {
            value = -1;
        }
        else if (value < 0.001f && value > -0.001f)
        {
            value = 0;
        }
        return value;
    }

    void HandleMotor()
    {
        //計算角度
        pitch = Vector3.SignedAngle(Vector3.up, transform.up, transform.right);
        float accelerationMultiplier = 1f;
        if (pitch < -slopeAngleThreshold)
        {
            accelerationMultiplier = slopeTorqueBoost;
        }
        else if (pitch > slopeAngleThreshold)
        {
            accelerationMultiplier = 0.8f;
        }
        //檢查引擎是否啟動，如果沒有就不會給予車輪動力
        if (!vehicleSetting.IsEngineStart)
        {
            leftTorque = 0;
            rightTorque = 0;
            foreach (var wheel in leftWheels)
            {
                wheel.motorTorque = leftTorque;
            }
            foreach (var wheel in rightWheels)
            {
                wheel.motorTorque = rightTorque;
            }
            return;
        }
        // 設定驅動扭力
        float currentAcceleration = accelerationMultiplier * maxMotorTorque * verticalInput;
        float turnFactor = horizontalInput * maxMotorTurn; // 0.3 表示最大增加/減少 30% 扭力  

        leftTorque = currentAcceleration * (1 + turnFactor);
        rightTorque = currentAcceleration * (1 - turnFactor);

        //左輪
        for (int i = 0; i < leftWheels.Length; i++)
        {
            leftWheels[i].motorTorque = leftTorque;
        }

        //右輪
        for (int i = 0; i < rightWheels.Length; i++)
        {
            rightWheels[i].motorTorque = leftTorque;
        }
    }

    void BreakVehicle()
    {
        float currentBreakForce = 0;
        float verticalInputValue = vehicleInput.Vehicle.Move.ReadValue<Vector2>().y;
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = breakForce;
        }
        else
        {
            if (verticalInputValue == 0)
            {

                if (pitch < -slopeAngleThreshold)
                {
                    currentBreakForce = 1000;
                }
                else
                {
                    currentBreakForce = 300;
                }
            }
            else
            {
                if (!vehicleSetting.IsEngineStart)
                {
                    currentBreakForce = 300;
                }
                else
                {
                    currentBreakForce = 0;
                }
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
        Vector3 speed = rb.linearVelocity;
        Vector3 limitSpeed;
        if (leftTorque > 0 && rightTorque > 0)
        {
            if (pitch > slopeAngleThreshold)
            {
                forwardMaxSpeed = 6;
            }
            else
            {
                forwardMaxSpeed = 4;
            }
            //前進
            if (speed.magnitude > forwardMaxSpeed)
            {
                limitSpeed = speed.normalized * forwardMaxSpeed;
                rb.linearVelocity = new Vector3(limitSpeed.x, rb.linearVelocity.y, limitSpeed.z);
            }
        }
        else if (leftTorque < 0 && rightTorque < 0)
        {
            //後退
            if (speed.magnitude > backwardMaxSpeed)
            {
                limitSpeed = speed.normalized * backwardMaxSpeed;
                rb.linearVelocity = new Vector3(limitSpeed.x, rb.linearVelocity.y, limitSpeed.z);
            }
        }
        currentSpeed = rb.linearVelocity.magnitude * 5f;
        if (currentSpeed < 0.01f)
        {
            currentSpeed = 0;
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