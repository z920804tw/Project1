using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class ThirdPersonAnimation : MonoBehaviour
{
    Animator animator;
    GameObject mainCam;
    public Rig headRig;
    public GameObject lookPoint;

    [Header("移動動畫參數")]
    [SerializeField] float moveVelocity;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = GameObject.FindWithTag("MainCamera");
        headRig.weight = 0;
    }
    void Update()
    {

    }
    public void HeadLook()
    {
        headRig.weight = 1;
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
        Vector3 endPoint;
        if (Physics.Raycast(ray, out hit, 10f))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint = mainCam.transform.position + mainCam.transform.forward * 10;
        }
        lookPoint.transform.position = endPoint;
        Debug.DrawRay(mainCam.transform.position, endPoint - mainCam.transform.position, Color.red);
    }

    public void ResetHeadLook()
    {
        if (headRig.weight != 0)
        {
            headRig.weight -= 0.8f * Time.deltaTime;
            if (headRig.weight < 0)
            {
                headRig.weight = 0;
            }
        }
    }

    public void MoveAnimState(Vector3 moveInput, bool isRun)
    {
        //代表有移動
        if (moveInput != Vector3.zero)
        {
            if (isRun && moveVelocity < 1f)
            {
                moveVelocity += acceleration * Time.deltaTime;
            }
            else if (!isRun)
            {
                if (moveVelocity < 0.5f)
                {
                    moveVelocity += acceleration * Time.deltaTime;
                }
                else if (moveVelocity > 0.5f)
                {
                    moveVelocity -= deceleration * Time.deltaTime;
                    if (moveVelocity < 0.5f)
                    {
                        moveVelocity = 0.5f;
                    }
                }

            }
        }
        else
        {
            if (moveVelocity > 0f)
            {
                moveVelocity -= deceleration * Time.deltaTime;

            }
            else if (moveVelocity < 0)
            {
                moveVelocity = 0;
            }
        }
        animator.SetFloat("moveVelocity", moveVelocity);
    }
    public void JumpAnimState(bool isGround, bool wasGround)
    {

        if (!isGround && wasGround) //在空中
        {
            animator.SetBool("isLand", false);
            animator.SetBool("isJump", true);

        }
        else if (isGround && !wasGround) //落地
        {
            animator.SetBool("isJump", false);
            animator.SetBool("isLand", true);

        }
    }

}
