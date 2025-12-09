using System.Collections;
using UnityEngine;

public class ThrowPlaceItem : MonoBehaviour, IUse
{
    [SerializeField] GameObject mainCam;
    [SerializeField] LayerMask placeLayerMask;
    PickObject pickObject;
    public bool canPlace;
    public bool canThrow;
    [SerializeField] bool isThrowAnim;
    ThirdPersonAnimation anim;
    Vector3 placePos;
    GameObject currentTarget;
    bool isAim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        pickObject=GetComponentInChildren<PickObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAim)
        {
            CheckPlaceAndThrow();
        }
    }

    public void UseObject(GameObject target)
    {
        if (anim == null)
        {
            anim = target.GetComponent<PlayerStatus>().anim;
        }
        if (currentTarget == null)
        {
            currentTarget = target;
        }

        isAim = true;
    }
    public void ResetUse()
    {
        ResetThrowPlace();
    }
    void CheckPlaceAndThrow()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 6f, placeLayerMask))
        {
            canPlace = true;
            canThrow = false;
            placePos = hit.point;
            placePos.y += 1f;
            if (isThrowAnim)
            {
                isThrowAnim = false;
                anim.ThrowAnim(false, false);
            }
        }
        else
        {
            canPlace = false;
            canThrow = true;
            //防止重複撥放動畫
            if (!isThrowAnim)
            {
                isThrowAnim = true;
                anim.ThrowAnim(true, false);
            }
        }
    }
    public void ResetThrowPlace()
    {
        canThrow = false;
        canPlace = false;
        isThrowAnim = false;
        isAim = false;
        if (anim != null)
        {
            anim.ThrowAnim(false, false);
            anim = null;
        }
        if (currentTarget != null)
        {
            currentTarget = null;
        }
    }

    public void Throw()
    {
        PlayerInventory playerInventory = currentTarget.GetComponent<PlayerStatus>().playerInventory;
        if (playerInventory != null)
        {
            playerInventory.handObj.transform.SetParent(null);
            playerInventory.handObj = null;

            UIManager.Instance.playerHand.handInventorySlots[playerInventory.SelectIndex].GetComponent<HandInventorySlot>().UpdateInfo(-1);
            UIManager.Instance.playerHand.HandSlotAmount--;
        }
        //更新動畫
        if (anim != null)
        {
            anim.ThrowAnim(true, true);
        }

        isThrowAnim = false;
        isAim = false;
        canThrow = false;

        //暫時關閉物件和玩家的偵測碰撞
        pickObject.ColliderAndRig(true);
        Physics.IgnoreCollision(pickObject.collider1, GameObject.FindWithTag("Player").GetComponent<Collider>(), true);
        Physics.IgnoreCollision(pickObject.triggerBox, GameObject.FindWithTag("PlayerInteract").GetComponent<Collider>(), true);

        Vector3 dir = mainCam.transform.forward * 10 + Vector3.up * 5;
        GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
        Debug.DrawRay(mainCam.transform.position, dir * 5, Color.red, 10f);
        StartCoroutine(WaitReset());
    }
    public void Place()
    {
        PlayerInventory playerInventory = currentTarget.GetComponent<PlayerStatus>().playerInventory;
        if (playerInventory != null)
        {
            playerInventory.handObj.transform.SetParent(null);
            playerInventory.handObj.transform.position = placePos;
            playerInventory.handObj.GetComponentInChildren<PickObject>().ColliderAndRig(true);
            playerInventory.handObj = null;

            UIManager.Instance.playerHand.handInventorySlots[playerInventory.SelectIndex].GetComponent<HandInventorySlot>().UpdateInfo(-1);
            UIManager.Instance.playerHand.HandSlotAmount--;
        }
        anim.ThrowAnim(false, false);
        isThrowAnim = false;
        canPlace = false;
        isAim = false;
    }

    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(1f);
        Physics.IgnoreCollision(pickObject.collider1, GameObject.FindWithTag("Player").GetComponent<Collider>(), false);
        Physics.IgnoreCollision(pickObject.triggerBox, GameObject.FindWithTag("PlayerInteract").GetComponent<Collider>(), false);
    }

}
