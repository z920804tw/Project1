using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public DialogueSO dialogueSO;
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
        //切換玩家狀態至對話模式
        PlayerStatus playerStatus = target.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.SetStatus(Status.Dialogue);
        }

        //開啟DialogueUI並傳遞參數給他
        UIManager.Instance.dialogueUI.SetDialogueInfo(dialogueSO);
        UIManager.Instance.dialogueUI.SetTarget(target);

    }
}
