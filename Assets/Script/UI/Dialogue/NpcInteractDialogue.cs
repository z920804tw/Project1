using DG.Tweening;
using UnityEngine;

public enum NpcStatus
{
    Default,
    StartQuest,
    InQuest,
    EndQuest,
}
public class NpcInteractDialogue : MonoBehaviour
{
    [Header("參數設定")]
    Vector3 defaultLook;
    public Vector3 DefaultLook { get { return defaultLook; } }
    [SerializeField] bool canLook;
    public bool CanLook { get { return canLook; } }
    [Header("NPC狀態參數設定")]
    public NpcStatus currentStatus;
    public DialogueSO StartDialogueSO;
    public DialogueSO InQuestDialogueSO;
    public DialogueSO defaultDialogueSO;
    [Header("事件參數設定")]
    [Tooltip("對話需要使用的Event事件")]

    [SerializeField] DialogueEvent[] startEvents;
    [SerializeField] DialogueEvent[] inQuestEvents;
    [SerializeField] DialogueEvent[] defaultEvents;



    public void UpdateNpcStatus(GameObject target)
    {
        switch (currentStatus)
        {
            case NpcStatus.StartQuest:
                StartDialogue(StartDialogueSO, target);
                break;
            case NpcStatus.InQuest:
                StartDialogue(InQuestDialogueSO, target);
                break;
            case NpcStatus.Default:
                StartDialogue(defaultDialogueSO, target);
                break;
        }
    }
    //互動功能觸發
    public void SetDialogue(GameObject target)
    {
        UpdateNpcStatus(target);
    }
    public void StartDialogue(DialogueSO dialogueSO, GameObject target)
    {
        if (dialogueSO == null) return;
        //切換玩家狀態至對話模式，並面向目標
        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.SetStatus(Status.Dialogue);
        }
        target.transform.DOLookAt(transform.position, 1f, AxisConstraint.Y).SetEase(Ease.InOutSine);
        if (canLook)
        {
            defaultLook = transform.position + transform.forward;
            transform.DOLookAt(target.transform.position, 1f, AxisConstraint.Y).SetEase(Ease.InOutSine);
        }

        //開啟DialogueUI並傳遞參數給他
        switch (currentStatus)
        {
            case NpcStatus.StartQuest:
                UIManager.Instance.dialogueUI.SetDialogueInfo(StartDialogueSO, startEvents);
                break;
            case NpcStatus.InQuest:
                UIManager.Instance.dialogueUI.SetDialogueInfo(InQuestDialogueSO, inQuestEvents);
                break;
            case NpcStatus.Default:
                UIManager.Instance.dialogueUI.SetDialogueInfo(defaultDialogueSO, defaultEvents);
                break;
        }
        UIManager.Instance.dialogueUI.SetTarget(target, this.gameObject);
    }



    //-----------更改NPC對話狀態-------------//
    public void SetStartQuest()
    {
        currentStatus = NpcStatus.StartQuest;
    }
    public void SetInQuest()
    {
        currentStatus = NpcStatus.InQuest;
    }
    public void SetComplet()
    {
        currentStatus = NpcStatus.Default;
        QuestOnRunTime questOnRunTime=UIManager.Instance.questUI.CurrentQuest;
        UIManager.Instance.questUI.UpdateQuestProgress(questOnRunTime.questSO.Quest[questOnRunTime.currentIndex].questStatusType,1,1);
    }
    //-----------更改NPC對話狀態-------------//

    //-----------檢查NPC的任務完成狀態-------//
    public void CheckQuestComplete()
    {
        bool isComplete = UIManager.Instance.questUI.GetQuestComplete();
        // UIManager.Instance.dialogueUI.isCheck = true;
        if (isComplete)
        {
            SetComplet();
            Debug.Log("完成任務");
        }
        else
        {
            SetInQuest();
            Debug.Log("沒有完成任務");
        }

    }
}
