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

    [Header("Debug")]
    [SerializeField] Ladder currentLadder;
    [SerializeField] float delayTime;
    [SerializeField] bool isTranslate;
    [SerializeField] bool drawVisualSphere;

    Vector3 climbDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //檢查是否在切換中，如果不是才能執行以下的內容
        if (!isTranslate)
        {
            if (isPress)
            {
                player.transform.position += climbSpeed * climbDir * Time.deltaTime;
                float ClimbSpeed = climbDir.y;
                player.GetComponent<PlayerStatus>().anim.PlayAnim(true);
                player.GetComponent<PlayerStatus>().anim.animator.SetFloat("climbSpeed", ClimbSpeed);
            }
            else
            {
                player.GetComponent<PlayerStatus>().anim.PlayAnim(false);
            }



            if (CheckBottom())
            {
                currentLadder.ExitLadder(ladderExitbottom);
                StartCoroutine(DelayLeaveClimb("Bottom"));
            }
            if (CheckTop())
            {
                currentLadder.ExitLadder(ladderExitTop);
                StartCoroutine(DelayLeaveClimb("Top"));
            }
        }

    }

    //-------檢查是否有觸碰到離開點----------//
    bool CheckBottom()
    {
        return Physics.CheckSphere(transform.position + -transform.up * sphereDistanceBottom, sphereRadius, groundLayer);
    }

    bool CheckTop()
    {
        return Physics.CheckSphere(transform.position + transform.up * sphereDistanceTop, sphereRadius, groundLayer);
    }
    //-------檢查是否有觸碰到離開點----------//

    //-------按鍵監聽-----------//
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
    public void SubClimbInput() //訂閱攀爬需要用的按鍵
    {
        if (playerInput == null)
        {
            playerInput = GameManager.Instance.playerInput;
        }

        playerInput.actions["Move"].performed += ClimbDir;
        playerInput.actions["Move"].canceled += ClimbDir;

        Debug.Log("監聽玩家攀爬");
    }
    public void DisSubClimbInput() //取消訂閱
    {
        playerInput.actions["Move"].performed -= ClimbDir;
        playerInput.actions["Move"].canceled -= ClimbDir;

        Debug.Log("取消監聽玩家攀爬");
    }
    //-------按鍵監聽-----------//
    public void SetClimbInfo(Ladder target, Transform top, Transform bottom) //設定攀爬的基本資訊
    {
        currentLadder = target;
        ladderExitTop = top;
        ladderExitbottom = bottom;

        isTranslate = true;

        StartCoroutine(DelayReset());
    }

    void OnDrawGizmos()
    {
        if(!drawVisualSphere) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * sphereDistanceBottom, sphereRadius);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * sphereDistanceTop, sphereRadius);
    }



    IEnumerator DelayLeaveClimb(string exitPos) //延遲退出攀爬模式
    {
        Debug.Log("退出攀爬模式");
        isTranslate = true;
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        playerStatus.anim.SetClimbAnim(false,exitPos);
        playerStatus.anim.PlayAnim(true);

        yield return new WaitForSeconds(delayTime);
        DisSubClimbInput();
        playerStatus.playerCam.DisSubAllCameraInput();
        playerStatus.SetStatus(Status.Normal);


        ladderExitTop = null;
        ladderExitbottom = null;
        currentLadder = null;

        isPress = false;
        isTranslate = false;
        climbDir = Vector3.zero;

        this.enabled = false;
    }

    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(delayTime);
        isTranslate = false;
    }
}
