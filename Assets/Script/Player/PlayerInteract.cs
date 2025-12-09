using System.Collections;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("組件")]
    [SerializeField] ThirdPersonMove thirdPersonMove;
    [SerializeField] ThirdPersonAnimation anim;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] GameObject hintPrefab;
    [Header("Debug")]
    [SerializeField] bool canInteract;
    bool isUse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // mainCam = GameObject.FindWithTag("MainCamera");
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInventory.handObj != null)
        {
            if (thirdPersonMove.IsAim && !isUse)
            {
                IUse item = playerInventory.handObj.GetComponent<IUse>();
                if (item != null)
                {
                    isUse = true;
                    item.UseObject(transform.root.gameObject);
                }

            }
            else if (!thirdPersonMove.IsAim && isUse)
            {

                IUse item = playerInventory.handObj.GetComponent<IUse>();
                if (item != null)
                {
                    isUse = false;
                    item.ResetUse();
                }

            }
        }

    }
    public void OnInteract()
    {
        //取得當前UI的選取
        if (UIManager.Instance != null && UIManager.Instance.interactUI.activeSelf && canInteract)
        {
            InteractUI interactUI = UIManager.Instance.interactUI.GetComponent<InteractUI>();
            GameObject interactObj = interactUI.hintUIList[interactUI.SelectIndex].GetComponent<Hint>().HintGameObjcet;

            //UI部分
            GameObject hintUIGameObj = interactUI.hintUIList[interactUI.SelectIndex];
            interactUI.hintUIList.Remove(hintUIGameObj);
            Destroy(hintUIGameObj);

            //Interact部分
            interactObj.GetComponent<IInteractable>().Interact(transform.root.gameObject);

            if (interactUI.hintUIList.Count <= 0)
            {
                UIManager.Instance.ShowInteractHint(false);
            }
            interactUI.HintUISelect(0);

            StartCoroutine(InteractColdDown(0.2f));
        }
    }
    //--------按鍵偵測----------//
    void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        InteractUI interactUI = UIManager.Instance.interactUI.GetComponent<InteractUI>();
        if (interactable != null)
        {
            if (interactUI.hintUIList.Count == 0)
            {
                UIManager.Instance.ShowInteractHint(true);
            }
            //新增HintUI物件並記錄
            GameObject hint = Instantiate(hintPrefab, interactUI.Content.position, Quaternion.identity);
            hint.GetComponent<Hint>().SetHintInfo(other.gameObject, interactable.GetHintText());
            hint.transform.SetParent(interactUI.Content);
            interactUI.hintUIList.Add(hint);
            interactUI.HintUISelect(0);

        }
    }
    void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        InteractUI interactUI = UIManager.Instance.interactUI.GetComponent<InteractUI>();
        if (interactable != null)
        {
            //移除hintUI和物件
            if (UIManager.Instance != null)
            {
                foreach (GameObject i in interactUI.hintUIList)
                {
                    //找到相符的紀錄物件就代表是目標
                    if (i.GetComponent<Hint>().HintGameObjcet == other.gameObject)
                    {
                        interactUI.hintUIList.Remove(i);
                        Destroy(i);
                        break;
                    }
                }
            }

            if (interactUI.hintUIList.Count <= 0)
            {
                UIManager.Instance.ShowInteractHint(false);
            }
            interactUI.HintUISelect(0);
        }
    }


    IEnumerator InteractColdDown(float delay)
    {
        canInteract = false;
        yield return new WaitForSeconds(delay);
        canInteract = true;
    }
}
