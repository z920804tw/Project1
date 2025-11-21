using UnityEngine;
using UnityEngine.InputSystem;

public enum Status { Normal, Climb, InVehicle, Inventory, Dialogue, }
public class PlayerStatus : MonoBehaviour
{
    public Status currentPlayerStatus;
    public ThirdPersonAnimation anim;
    [SerializeField] CapsuleCollider interactCollider;
    [Header("玩家控制項")]
    public PlayerInput playerInput;
    public ThirdPersonMove playerMove;
    public ThirdPersonCamera playerCam;
    public PlayerInventory playerInventory;
    public PlayerInteract playerInteract;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPlayerStatus = Status.Normal;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetStatus(Status status)
    {
        currentPlayerStatus = status;
        switch (currentPlayerStatus)
        {
            case Status.Normal:
                SetNormalStatus();
                break;
            case Status.Climb:
                break;

            case Status.InVehicle:
                SetVehicleStatus();
                break;

            case Status.Inventory:
                SetOpenInventoryStatus();
                break;

            case Status.Dialogue:
                break;

            default:
                break;
        }

        anim.MoveVelocity = 0;
        anim.animator.SetFloat("moveVelocity", 0);
    }
    //-------------------------------狀態設定------------------------------//
    //一般行走功能，大部分功能都開啟，包含了移動、攝影機移動、背包、互動、碰撞相等等功能
    void SetAllComponet(bool t) //玩家功能全域控制
    {
        if (playerInventory.handObj != null)
        {
            playerInventory.handObj.SetActive(t);
        }
        playerInput.enabled = t;
        playerMove.enabled = t;
        playerCam.enabled = t;
        playerInventory.enabled = t;
        playerInteract.enabled = t;
        GetComponent<CharacterController>().enabled = t;
        interactCollider.enabled = t;

        // playerCam.CinemachineTargetYaw = 0;
        // playerCam.CinemachineTargetPitch = 20;

        UIManager.Instance.ShowPlayerUI(t);
    }
    void SetNormalStatus()
    {
        SetAllComponet(true);
        CameraManager.Instance.SetCameraMode(CameraMode.Normal);

    }
    void SetVehicleStatus()
    {
        SetAllComponet(false);
        CameraManager.Instance.SetCameraMode(CameraMode.InVehicle);
    }
    void SetOpenInventoryStatus()
    {
        playerMove.IsAim = false;
        playerInteract.ResetThrowPlace();
        CameraManager.Instance.SetCameraMode(CameraMode.Normal);

        playerMove.enabled = false;
        playerCam.enabled = false;
        playerInteract.enabled = false;

    }
    //-------------------------------狀態設定------------------------------//

    //--------玩家移動--------//
    public void OnMove(InputValue value)
    {
        Vector2 value1 = value.Get<Vector2>();
        playerMove.OnMove(value1);
    }
    public void OnJump(InputValue value)
    {
        if (UIManager.Instance.IsOpenBackpack) return;
        playerMove.OnJump();
    }
    public void OnRun(InputValue value)
    {
        playerMove.OnRun();
    }
    public void OnAim(InputValue value)
    {
        if (UIManager.Instance.IsOpenBackpack) return;
        playerMove.OnAim();
    }
    //--------玩家移動--------//

    //--------物品欄--------//
    public void OnSelect(InputValue value)
    {
        float value1 = value.Get<float>();
        if (!UIManager.Instance.interactUI.activeSelf)
        {
            playerInventory.OnSelect(value1);
        }


        if (UIManager.Instance != null && UIManager.Instance.interactUI.activeSelf)
        {
            UIManager.Instance.HintUISelect(-(int)value1);
        }
    }
    //--------物品欄--------//
    //--------玩家互動--------//
    public void OnOpenBackpack(InputValue value)
    {
        UIManager.Instance.IsOpenBackpack = !UIManager.Instance.IsOpenBackpack;
        UIManager.Instance.ShowBackpackUI(UIManager.Instance.IsOpenBackpack);
        if (!UIManager.Instance.IsOpenBackpack)
        {
            SetStatus(Status.Normal);
        }
        else
        {
            SetStatus(Status.Inventory);
        }
    }
    public void OnInteract(InputValue value)
    {
        if (UIManager.Instance.IsOpenBackpack || playerMove.IsAim) return;
        playerInteract.OnInteract();
    }
    public void OnThrow(InputValue value)
    {
        playerInteract.OnThrow();
    }
    public void OnPlace(InputValue value)
    {
        playerInteract.OnPlace();
    }
    //--------玩家互動--------//

}
