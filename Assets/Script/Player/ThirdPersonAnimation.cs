using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class ThirdPersonAnimation : MonoBehaviour
{
    [Header("組件")]
    public Animator animator;
    [SerializeField] Rig headRig;
    [SerializeField] GameObject lookPoint;
    GameObject mainCam;
    [Header("一般參數設定")]
    [SerializeField] LayerMask lookLayer;

    [Header("移動動畫參數")]
    [SerializeField] float moveVelocity;
    public float MoveVelocity { get { return moveVelocity; } set { moveVelocity = value; } }
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        if (Physics.Raycast(ray, out hit, 10f, lookLayer))
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
    public void GetOnVehicle(bool t)
    {
        animator.SetBool("isSit", t);
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

    public void ThrowAnim(bool isAim, bool isThrow)
    {
        StopCoroutine("AnimLayerDelay");
        if (isAim)
        {
            animator.SetBool("aimThrow", true);
            StartCoroutine(AnimLayerDelay(1, 0, 1, 1f));
        }
        else
        {
            animator.SetBool("aimThrow", false);
            StartCoroutine(AnimLayerDelay(1, 1, 0, 1f));
        }

        if (isThrow)
        {
            animator.SetTrigger("isThrow");
            StartCoroutine(AnimLayerDelay(1, 1, 0, 1f));
        }
    }
    

    //負責開關Layer的權重延遲
    IEnumerator AnimLayerDelay(int layer, float start, float end, float duration)
    {
        float timer = 0;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float currentValue = Mathf.Lerp(start, end, timer / duration);
            animator.SetLayerWeight(layer, currentValue);
            yield return null;
        }

        animator.SetLayerWeight(layer, end);
    }

}
