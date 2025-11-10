using UnityEngine;
using UnityEngine.InputSystem;

public enum Status { Normal, InVehicle }
public class PlayerStatus : MonoBehaviour
{
    public Status currentPlayerStatus;
    [SerializeField] ThirdPersonAnimation anim;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerInventory playerInventory;

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
        GetComponent<ThirdPersonCamera>().enabled = isNormal;
        GetComponent<ThirdPersonMove>().enabled = isNormal;
        GetComponent<PlayerInventory>().enabled = isNormal;
        GetComponent<PlayerInteract>().enabled = isNormal;
        GetComponent<CharacterController>().enabled = isNormal;
        //組件設定
        //攝影機設定
        GetComponent<ThirdPersonMove>().followCam.SetActive(isNormal);
        GetComponent<ThirdPersonMove>().aimCam.SetActive(false);
        //攝影機設定

        anim.MoveVelocity = 0;
        anim.animator.SetFloat("moveVelocity", 0);


    }
}
