using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Climb : MonoBehaviour
{
    PlayerInput playerInput;
    [Header("物件套用")]
    [SerializeField] GameObject player;
    public LayerMask groundLayer;
    Transform ladderExitTop;
    Transform ladderExitbottom;
    [Header("參數設定")]
    [SerializeField] float climbSpeed;
    [SerializeField] bool isPress;

    [SerializeField] float sphereRadius;
    [SerializeField] float sphereDistanceBottom;
    [SerializeField] float sphereDistanceTop;


    Vector3 climbDir;
    bool isLeaveing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPress && !isLeaveing)
        {
            player.transform.position += climbSpeed * climbDir * Time.deltaTime;
        }

        if (CheckBottom() && !isLeaveing)
        {
            isLeaveing = true;
            StartCoroutine(DelayLeaveClimb(ladderExitbottom));
        }
        if (CheckTop() && !isLeaveing)
        {
            isLeaveing = true;
            StartCoroutine(DelayLeaveClimb(ladderExitTop));
        }
    }

    bool CheckBottom()
    {
        return Physics.CheckSphere(transform.position + -transform.up * sphereDistanceBottom, sphereRadius, groundLayer);
    }

    bool CheckTop()
    {
        return Physics.CheckSphere(transform.position + transform.up * sphereDistanceTop, sphereRadius, groundLayer);
    }

    void ClimbDir(InputAction.CallbackContext ctx)
    {
        climbDir = ctx.ReadValue<Vector2>();
        if (ctx.performed)
        {
            isPress = true;
        }
        else if (ctx.canceled)
        {
            isPress = false;
        }
    }
    public void SubClimbInput()
    {
        if (playerInput == null)
        {
            playerInput = GameManager.Instance.playerInput;
        }

        playerInput.actions["Move"].performed += ClimbDir;
        playerInput.actions["Move"].canceled += ClimbDir;

        Debug.Log("監聽玩家攀爬");
    }
    public void DisSubClimbInput()
    {
        playerInput.actions["Move"].performed -= ClimbDir;
        playerInput.actions["Move"].canceled -= ClimbDir;

        Debug.Log("取消監聽玩家攀爬");
    }

    public void SetExitPos(Transform top, Transform bottom)
    {
        ladderExitTop = top;
        ladderExitbottom = bottom;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * sphereDistanceBottom, sphereRadius);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * sphereDistanceTop, sphereRadius);
    }



    IEnumerator DelayLeaveClimb(Transform exitPos)
    {
        Debug.Log("可退出攀爬模式");
        player.transform.position = exitPos.position;
        yield return new WaitForSeconds(0.2f);

        DisSubClimbInput();
        player.GetComponent<PlayerStatus>().playerCam.DisSubAllCameraInput();
        player.GetComponent<PlayerStatus>().SetStatus(Status.Normal);

        ladderExitTop = null;
        ladderExitbottom = null;

        isPress = false;
        isLeaveing = false;
        climbDir = Vector3.zero;

        this.enabled = false;
    }
}
