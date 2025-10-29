using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonAnimation : MonoBehaviour
{
    public Animator animator;
    [Header("移動動畫參數")]
    [SerializeField] float moveVelocity;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

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
