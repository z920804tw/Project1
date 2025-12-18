using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class InteractDialogue : MonoBehaviour
{
    [Header("參數設定")]
    public DialogueSO dialogueSO;

    [Header("事件參數設定")]
    [Tooltip("對話需要使用的Event事件")]
    public DialogueEvent[] dialogueEvents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StarDialogue(GameObject target)
    {
        //切換玩家狀態至對話模式，並面向目標
        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.SetStatus(Status.Dialogue);
        }

        target.transform.DOLookAt(transform.position, 1f, AxisConstraint.Y).SetEase(Ease.InOutSine);


        //開啟DialogueUI並傳遞參數給他
        UIManager.Instance.dialogueUI.SetDialogueInfo(dialogueSO, dialogueEvents);
        UIManager.Instance.dialogueUI.SetTarget(target, this.gameObject);

    }
}
// [System.Serializable]
// public class DialogueEvent
// {
//     public int checkNumber=-1;
//     public UnityEvent[] options;
// }
