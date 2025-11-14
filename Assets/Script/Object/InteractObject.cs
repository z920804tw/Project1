using UnityEngine;
using UnityEngine.Events;

public class InteractObject : MonoBehaviour, IInteractable
{
    [SerializeField] string hintText;
    public UnityEvent<GameObject> unityEvent;
    public UnityEvent unityEvent1;

    //-------IInteractable--------//
    public void Interact(GameObject target)
    {
        unityEvent.Invoke(target);
        unityEvent1.Invoke();
        ShowHint(false);
        Debug.Log("你執行了一個事件" + "，觸發者:" + target.name);
    }
    public void ShowHint(bool t)
    {
        UIManager.Instance.ShowInteractHint(t,hintText);
    }
    //-------IInteractable--------//

}
