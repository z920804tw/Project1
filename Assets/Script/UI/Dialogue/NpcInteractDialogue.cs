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
    [SerializeField] bool canLook;
    public bool CanLook { get { return canLook; } }
    Vector3 defaultLook;
    public Vector3 DefaultLook { get { return defaultLook; } }


    [Header("NPC狀態參數設定")]
    [Tooltip("如果該NPC有任務的話請掛上對應的SO")]
    public QuestSO questSO;
    public NpcStatus currentStatus;
    public DialogueSO StartDialogueSO;
    public DialogueSO InQuestDialogueSO;
    public DialogueSO defaultDialogueSO;
    [Header("事件參數設定")]
    [Tooltip("對話需要使用的Event事件")]

    [SerializeField] DialogueEvent[] startEvents;
    [SerializeField] DialogueEvent[] inQuestEvents;
    [SerializeField] DialogueEvent[] defaultEvents;

    void Start()
    {
        if (questSO != null)
        {
            if (questSO.isComplet)
            {
                currentStatus = NpcStatus.Default;
            }
            else
            {
                currentStatus = NpcStatus.StartQuest;
            }
        }
        else
        {
            currentStatus = NpcStatus.Default;
        }
    }


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
        QuestOnRunTime questOnRunTime = UIManager.Instance.questManager.CurrentQuest;
        UIManager.Instance.questManager.UpdateQuestProgress(questOnRunTime.questSO.Quest[questOnRunTime.currentIndex].questStatusType, 1, 1);
    }
    //-----------更改NPC對話狀態-------------//

    //-----------檢查NPC的任務完成狀態-------//
    public void CheckQuestComplete()
    {
        bool isComplete = UIManager.Instance.questManager.GetQuestComplete();

        if (isComplete)
        {
            SetComplet();
            Debug.Log("完成任務");
        }
        else
        {
            //任務沒有完成，就切換DialogueUI的對話到指定的Index
            UIManager.Instance.dialogueUI.isCheck = true;
            DialogueUI dialogueUI = UIManager.Instance.dialogueUI;
            dialogueUI.StopAllCoroutines();
            UIManager.Instance.dialogueUI.SetDialogueIndex(dialogueUI.currentSO.dialogueContent[dialogueUI.dialogueIndex].choices[dialogueUI.dialogueIndex].notCompleteNextIndex[0]);
            Debug.Log("沒有完成任務");
        }

    }
}
