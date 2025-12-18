using UnityEngine;

public class testQuestItem : MonoBehaviour
{
    [SerializeField] QuestSO questSO;
    [SerializeField] int subId;
    [SerializeField] int addAmount;

    public void CollectQuestItem()
    {
        if (UIManager.Instance.questManager.CurrentQuest.questSO == null) return;
        //判斷是不是同個任務SO，不一樣就不能使用，一樣才能更新
        if (UIManager.Instance.questManager.CurrentQuest.questSO == questSO)
        {
            UIManager.Instance.questManager.UpdateQuestProgress(questSO.Quest[subId].questStatusType, subId, addAmount);
            Destroy(gameObject);
        }
    }
}
