using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [Header("物品欄(手部)")]
    public GameObject handObj;
    [SerializeField] Transform rightHand;
    [SerializeField] GameObject triggerObj;
    ThirdPersonMove thirdPersonMove;
    Vector3 placePos;

    bool canPick;
    bool canPlace;
    bool canThrow;

    GameObject mainCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        thirdPersonMove = GetComponent<ThirdPersonMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handObj != null)
        {
            if (thirdPersonMove.IsAim && handObj.tag == "PickObject")
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
            }
            else if (!thirdPersonMove.IsAim && handObj.tag == "PickObject")
            {
                canPlace = false;
                canThrow = false;
            }
        }

    }


    //按鍵偵測(撿取、放置、丟)
    public void OnPick(InputValue value)
    {
        if (canPick)
        {
            handObj = triggerObj;
            handObj.transform.SetParent(rightHand);
            handObj.transform.position = rightHand.transform.position;
            handObj.GetComponent<PickObject>().ColliderAndRig(false);
            handObj.gameObject.GetComponent<PickObject>().ShowCloseInfo(false);

            triggerObj = null;

            canPick = false;
        }
    }

    public void OnPlace(InputValue value)
    {
        if (canPlace)
        {
            handObj.transform.SetParent(null);
            handObj.transform.position = placePos;
            handObj.GetComponent<PickObject>().ColliderAndRig(true);
            handObj = null;

            canPlace = false;
            Debug.Log("放置");
        }
    }
    public void OnThrow(InputValue value)
    {
        if (canThrow)
        {
            handObj.transform.SetParent(null);
            handObj.GetComponent<PickObject>().Throw(mainCam.transform.forward * 10 + transform.up * 5f);
            handObj = null;
            canThrow = false;
            Debug.Log("丟出物品");
        }
    }

    //偵測可撿取物件
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickObject") && triggerObj == null && handObj == null)
        {
            Debug.Log(other.name);
            canPick = true;
            triggerObj = other.gameObject;
            other.GetComponent<PickObject>().ShowCloseInfo(true);
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickObject") && triggerObj != null && other.gameObject.name == triggerObj.name)
        {
            Debug.Log(other.name);
            canPick = false;
            triggerObj = null;
            other.gameObject.GetComponent<PickObject>().ShowCloseInfo(false);
        }
    }


}
