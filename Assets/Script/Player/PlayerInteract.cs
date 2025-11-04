using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("組件")]
    [SerializeField] ThirdPersonMove thirdPersonMove;
    [SerializeField] ThirdPersonAnimation anim;
    PlayerInventory playerInventory;
    GameObject mainCam;
    [SerializeField] GameObject currentTarget;
    Vector3 placePos;

    [Header("Debug")]
    [SerializeField] bool canPlace;
    [SerializeField] bool canThrow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlaceAndThrow();

    }

    void CheckPlaceAndThrow()
    {
        if (playerInventory != null && playerInventory.handObj != null)
        {
            if (thirdPersonMove.IsAim)
            {
                Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 5f))
                {
                    canPlace = true;
                    canThrow = false;
                    placePos = hit.point;
                    placePos.y += 1f;
                }
                else
                {
                    canPlace = false;
                    canThrow = true;
                }
                anim.ThrowAnim(true, false);
            }
            else
            {
                canPlace = false;
                canThrow = false;
                anim.ThrowAnim(false, false);
            }
        }
    }
    //--------按鍵偵測----------//
    public void OnThrow(InputValue value)
    {
        if (canThrow)
        {
            //給予撿取物件丟的功能
            Vector3 dir = mainCam.transform.forward * 10 + transform.up * 5;
            playerInventory.handObj.transform.SetParent(null);
            playerInventory.handObj.GetComponent<PickObject>().Throw(dir);
            playerInventory.handObj = null;

            //更新物品欄
            if (playerInventory != null)
            {
                playerInventory.slots[playerInventory.SelectIndex].GetComponent<PlayerInventorySlot>().UpdateInfo(-1);
                playerInventory.SlotAmount = -1;
            }
            anim.ThrowAnim(false, true);
            canThrow = false;
        }
    }
    public void OnPlace(InputValue value)
    {
        if (canPlace)
        {
            if (playerInventory != null)
            {
                playerInventory.handObj.transform.SetParent(null);
                playerInventory.handObj.transform.position = placePos;
                playerInventory.handObj.GetComponent<PickObject>().ColliderAndRig(true);
                playerInventory.handObj = null;

                playerInventory.slots[playerInventory.SelectIndex].GetComponent<PlayerInventorySlot>().UpdateInfo(-1);
                playerInventory.SlotAmount = -1;
            }
            anim.ThrowAnim(false, false);
            canPlace = false;
        }
    }
    void OnInteract(InputValue value)
    {
        if (currentTarget != null)
        {
            currentTarget.GetComponent<IInteractable>().Interact();
            currentTarget = null;
        }
    }
    //--------按鍵偵測----------//
    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (currentTarget == null)
            {
                currentTarget = other.gameObject;
                currentTarget.GetComponent<IInteractable>().ShowHint(true);
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (currentTarget != null && currentTarget.GetComponent<IInteractable>() == interactable)
            {
                currentTarget.GetComponent<IInteractable>().ShowHint(false);
                currentTarget = null;
            }
        }
    }
}
