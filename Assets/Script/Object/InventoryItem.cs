using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform parentTransform;
    public InventorySlot currentSlot;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTransform=transform.parent;
        transform.SetParent(transform.parent.parent.parent.parent);
        transform.SetAsLastSibling();

        //將原本的slot欄位資訊清空

    }

    public void OnDrag(PointerEventData eventData)
    {
       transform.position=Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentTransform);
        transform.GetComponent<RectTransform>().localPosition=new Vector3(0,0,0);
        transform.SetSiblingIndex(1);

        currentSlot=parentTransform.GetComponent<InventorySlot>();
    }

}
