using UnityEngine;
using UnityEngine.InputSystem;

public enum Status { Normal, Climb, InVehicle, Inventory, Dialogue, }
public class PlayerStatus : MonoBehaviour
{
    public Status currentPlayerStatus;
    public ThirdPersonAnimation anim;
    [SerializeField] CapsuleCollider interactCollider;
    [Header("玩家控制項")]
    PlayerInput playerInput;
    public ThirdPersonMove playerMove;
    public ThirdPersonCamera playerCam;
    public PlayerInventory playerInventory;
    public PlayerInteract playerInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GameManager.Instance.playerInput;
        SetStatus(Status.Normal);
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

        playerMove.enabled = t;
        playerCam.enabled = t;
        playerInventory.enabled = t;
        playerInteract.enabled = t;
        GetComponent<CharacterController>().enabled = t;
        interactCollider.enabled = t;


    }
    void SetNormalStatus()
    {
        //切換控制模式並監聽玩家、玩家攝影機控制
        GameManager.Instance.SwitchInputMode("Player");
        SubPlayerAllInput();
        playerCam.SubAllCameraInput();
        
        SetAllComponet(true);
        
        CameraManager.Instance.SetCameraMode(CameraMode.Normal);
        UIManager.Instance.ShowPlayerUI(true);
        GameManager.Instance.ShowCursor(false);
    }
    void SetVehicleStatus()
    {
        //取消玩家、攝影機監聽，並切換控制模式為載具
        DisSubPlayerAllInput();
        playerCam.DisSubAllCameraInput();
        SetAllComponet(false);

        UIManager.Instance.ShowPlayerUI(false);
        CameraManager.Instance.SetCameraMode(CameraMode.InVehicle);
    }
    //開啟背包
    void SetOpenInventoryStatus()
    {
        //取消玩家按鍵監聽、攝影機按鍵監聽
        DisSubPlayerAllInput();
        playerCam.Stop();
        playerCam.DisSubAllCameraInput();

        //停止玩家相關設定
        playerMove.Stop();
        playerInteract.ResetThrowPlace();

        CameraManager.Instance.SetCameraMode(CameraMode.Normal);

        GameManager.Instance.ShowCursor(true);
        GameManager.Instance.SwitchInputMode("UI");

        UIManager.Instance.SubAllUIInput();
        UIManager.Instance.ShowBackpackUI(true);

    }
    //-------------------------------狀態設定------------------------------//

    //--------玩家移動--------//
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value1 = ctx.ReadValue<Vector2>();
        playerMove.OnMove(value1);
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        playerMove.OnJump();
    }
    public void OnRun(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            playerMove.OnRun();
        }

    }
    public void OnAim(InputAction.CallbackContext ctx)
    {
        playerMove.OnAim();
    }
    //--------玩家移動--------//

    //--------物品欄--------//
    public void OnSelect(InputAction.CallbackContext ctx)
    {
        float value1 = ctx.ReadValue<float>();
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
    public void OnOpenBackpack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SetStatus(Status.Inventory);
        }
    }
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        playerInteract.OnInteract();
    }
    public void OnThrow(InputAction.CallbackContext ctx)
    {
        playerInteract.OnThrow();
    }
    public void OnPlace(InputAction.CallbackContext ctx)
    {
        playerInteract.OnPlace();
    }
    //--------玩家互動--------//


    public void SubPlayerAllInput()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;

        playerInput.actions["Jump"].performed += OnJump;
        playerInput.actions["Jump"].canceled += OnJump;

        playerInput.actions["Run"].performed += OnRun;
        playerInput.actions["Run"].canceled += OnRun;

        playerInput.actions["Aim"].performed += OnAim;
        playerInput.actions["Aim"].canceled += OnAim;

        playerInput.actions["Select"].performed += OnSelect;
        playerInput.actions["Select"].canceled += OnSelect;

        playerInput.actions["OpenBackpack"].performed += OnOpenBackpack;
        playerInput.actions["OpenBackpack"].canceled += OnOpenBackpack;

        playerInput.actions["Interact"].performed += OnInteract;
        playerInput.actions["Interact"].canceled += OnInteract;

        playerInput.actions["Throw"].performed += OnThrow;
        playerInput.actions["Throw"].canceled += OnThrow;

        playerInput.actions["Place"].performed += OnPlace;
        playerInput.actions["Place"].canceled += OnPlace;
        Debug.Log("監聽玩家控制");
    }

    void DisSubPlayerAllInput()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;

        playerInput.actions["Jump"].performed -= OnJump;
        playerInput.actions["Jump"].canceled -= OnJump;

        playerInput.actions["Run"].performed -= OnRun;
        playerInput.actions["Run"].canceled -= OnRun;

        playerInput.actions["Aim"].performed -= OnAim;
        playerInput.actions["Aim"].canceled -= OnAim;

        playerInput.actions["Select"].performed -= OnSelect;
        playerInput.actions["Select"].canceled -= OnSelect;

        playerInput.actions["OpenBackpack"].performed -= OnOpenBackpack;
        playerInput.actions["OpenBackpack"].canceled -= OnOpenBackpack;

        playerInput.actions["Interact"].performed -= OnInteract;
        playerInput.actions["Interact"].canceled -= OnInteract;

        playerInput.actions["Throw"].performed -= OnThrow;
        playerInput.actions["Throw"].canceled -= OnThrow;

        playerInput.actions["Place"].performed -= OnPlace;
        playerInput.actions["Place"].canceled -= OnPlace;
        Debug.Log("取消監聽玩家控制");
    }
}
