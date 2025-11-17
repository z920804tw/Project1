using UnityEngine;
using UnityEngine.InputSystem;

public enum Status { Normal, InVehicle }
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
        bool isNormal;
        if (status == Status.Normal)
        {
            isNormal = true;
        }
        else
        {
            isNormal = false;
        }

        if (playerInventory.handObj != null)
        {
            playerInventory.handObj.SetActive(isNormal);
        }
        //組件設定
        playerInput.enabled = isNormal;
        playerCam.enabled = isNormal;
        playerMove.enabled = isNormal;
        playerInventory.enabled = isNormal;
        playerInteract.enabled = isNormal;
        GetComponent<CharacterController>().enabled = isNormal;
        interactCollider.enabled=isNormal;
        //組件設定

        //攝影機設定
        playerMove.followCam.SetActive(isNormal);
        playerMove.aimCam.SetActive(false);
        //攝影機設定

        //UI設定
        UIManager.Instance.ShowPlayerUI(isNormal);
        //UI設定

        anim.MoveVelocity = 0;
        anim.animator.SetFloat("moveVelocity", 0);
    }
    //--------玩家移動--------//
    public void OnMove(InputValue value)
    {
        playerMove.OnMove(value.Get<Vector2>());
    }
    public void OnJump(InputValue value)
    {
        playerMove.OnJump();
    }
    public void OnRun(InputValue value)
    {
        playerMove.OnRun();
    }
    public void OnAim(InputValue value)
    {
        playerMove.OnAim();
    }
    //--------玩家移動--------//
    //--------攝影機控制--------//
    // public void OnLook(InputValue value)
    // {
    //     playerCam.OnLook(value.Get<Vector2>());
    // }
    // public void OnZoom(InputValue value)
    // {
    //     playerCam.OnZoom(value.Get<float>());
    // }
    //--------攝影機控制--------//
    //--------物品欄--------//
    public void OnSelect(InputValue value)
    {
        playerInventory.OnSelect(value.Get<float>());
    }
    //--------物品欄--------//
    //--------玩家互動--------//
    public void OnInteract(InputValue value)
    {
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
