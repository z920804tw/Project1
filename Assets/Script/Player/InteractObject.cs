using UnityEngine;
using UnityEngine.Events;

public class InteractObject : MonoBehaviour
{
    [SerializeField] GameObject hint;
    public UnityEvent unityEvent;

    public void ShowHint(bool t)
    {
        hint.SetActive(t);
    }
    public void DoEvent()
    {
        Debug.Log("你執行了一個事件");
        unityEvent.Invoke();
        ShowHint(false);
    }

}
