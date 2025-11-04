using UnityEngine;
using UnityEngine.Events;

public class InteractObject : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject hint;
    public UnityEvent unityEvent;

    //-------IInteractable--------//
    public void Interact()
    {
        DoEvent();
    }
    public void ShowHint(bool t)
    {
        hint.SetActive(t);
    }
    //-------IInteractable--------//
    void DoEvent()
    {
        Debug.Log("你執行了一個事件");
        unityEvent.Invoke();
        ShowHint(false);
    }

}
