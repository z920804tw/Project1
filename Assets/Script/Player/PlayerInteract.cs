using System.Collections;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("組件")]
    [SerializeField] ThirdPersonMove thirdPersonMove;
    [SerializeField] ThirdPersonAnimation anim;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] GameObject hintPrefab;
    GameObject mainCam;
    Vector3 placePos;

    [Header("Debug")]
    [SerializeField] LayerMask placeLayerMask;
    [SerializeField] bool canPlace;
    [SerializeField] bool canThrow;
    [SerializeField] bool canInteract;
    [SerializeField]bool isThrowAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        canInteract = true;
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
                if (Physics.Raycast(ray, out hit, 8f, placeLayerMask))
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
            else
            {
                canPlace = false;
                canThrow = false;

                if (isThrowAnim)
                {
                    isThrowAnim = false;
                    anim.ThrowAnim(false, false);
                }
            }
        }
    }
    //--------按鍵偵測----------//
    public void OnThrow()
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
                UIManager.Instance.handInventorySlots[playerInventory.SelectIndex].GetComponent<PlayerInventorySlot>().UpdateInfo(-1);
                playerInventory.SlotAmount = -1;
            }
            //更新動畫
            anim.ThrowAnim(true, true);
            isThrowAnim = false;
            canThrow = false;
        }
    }
    public void OnPlace()
    {
        if (canPlace)
        {
            if (playerInventory != null)
            {
                playerInventory.handObj.transform.SetParent(null);
                playerInventory.handObj.transform.position = placePos;
                playerInventory.handObj.GetComponent<PickObject>().ColliderAndRig(true);
                playerInventory.handObj = null;

                UIManager.Instance.handInventorySlots[playerInventory.SelectIndex].GetComponent<PlayerInventorySlot>().UpdateInfo(-1);
                playerInventory.SlotAmount = -1;
            }
            anim.ThrowAnim(false, false);
            isThrowAnim = false;
            canPlace = false;
        }
    }
    public void OnInteract()
    {
        //取得當前UI的選取
        if (UIManager.Instance != null && UIManager.Instance.interactUI.activeSelf && canInteract)
        {
            GameObject interactObj = UIManager.Instance.hintUIList[UIManager.Instance.SelectIndex].GetComponent<Hint>().HintGameObjcet;

            //UI部分
            GameObject hintUIGameObj = UIManager.Instance.hintUIList[UIManager.Instance.SelectIndex];
            UIManager.Instance.hintUIList.Remove(hintUIGameObj);
            Destroy(hintUIGameObj);

            //Interact部分
            interactObj.GetComponent<IInteractable>().Interact(transform.root.gameObject);

            if (UIManager.Instance.hintUIList.Count <= 0)
            {
                UIManager.Instance.ShowInteractHint(false);
            }
            UIManager.Instance.HintUISelect(0);

            StartCoroutine(InteractColdDown(0.5f));
        }
    }
    //--------按鍵偵測----------//
    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            if (UIManager.Instance.hintUIList.Count == 0)
            {
                UIManager.Instance.ShowInteractHint(true);
            }
            //新增HintUI物件並記錄
            GameObject hint = Instantiate(hintPrefab, UIManager.Instance.Content.position, Quaternion.identity);
            hint.GetComponent<Hint>().SetHintInfo(other.gameObject, interactable.GetHintText());
            hint.transform.SetParent(UIManager.Instance.Content);
            UIManager.Instance.hintUIList.Add(hint);
            UIManager.Instance.HintUISelect(0);

        }
    }
    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            //移除hintUI和物件
            if (UIManager.Instance != null)
            {
                foreach (GameObject i in UIManager.Instance.hintUIList)
                {
                    //找到相符的紀錄物件就代表是目標
                    if (i.GetComponent<Hint>().HintGameObjcet == other.gameObject)
                    {
                        UIManager.Instance.hintUIList.Remove(i);
                        Destroy(i);
                        break;
                    }
                }
            }

            if (UIManager.Instance.hintUIList.Count <= 0)
            {
                UIManager.Instance.ShowInteractHint(false);
            }
            UIManager.Instance.HintUISelect(0);
        }
    }


    IEnumerator InteractColdDown(float delay)
    {
        canInteract=false;
        yield return new WaitForSeconds(delay);
        canInteract=true;
    }
}
