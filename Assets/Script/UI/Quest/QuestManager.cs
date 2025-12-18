using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("組件套用")]
    [SerializeField] QuestUI questUI;
    [Header("參數")]
    [SerializeField] QuestOnRunTime currentQuest;
    public QuestOnRunTime CurrentQuest { get { return currentQuest; } }
    [SerializeField] List<QuestOnRunTime> activeQuest = new List<QuestOnRunTime>();


    void Start()
    {
        questUI = GetComponent<QuestUI>();
    }
    public void AddQuest(QuestSO quest)
    {
        QuestOnRunTime questOnRunTime = new QuestOnRunTime();
        questOnRunTime.SetQuest(quest);
        activeQuest.Add(questOnRunTime);
        currentQuest = questOnRunTime;

        UIManager.Instance.ShowQuestUI(true);
        questUI.UpdateQuestContent(currentQuest.questSO, questOnRunTime.currentIndex);
        questUI.UpdateConditionText();
        Debug.Log($"新增任務:{questOnRunTime.questSO.Quest[0].questName}");
    }



    //更新任務狀態
    public void UpdateQuestProgress(QuestStatusType type, int subID, int amount)
    {
        //檢查有沒有任務，如果沒有就return
        if (currentQuest.questSO == null) return;
        Quest questStep = currentQuest.questSO.Quest[currentQuest.currentIndex];
        //檢查QuestID和當前quest的id是否一樣，並判斷類型是否一樣
        if (subID == questStep.subID && type == questStep.questStatusType)
        {
            currentQuest.currentAmount += amount;
            questUI.UpdateConditionText();
            if (currentQuest.currentAmount >= questStep.requiredAmount)
            {
                currentQuest.currentAmount = questStep.requiredAmount;
                NextStepOrComplete();
            }
        }

    }

    //檢查要進入下一階段的任務還是結束任務
    public void NextStepOrComplete()
    {
        int count = currentQuest.questSO.Quest.Count();
        if (currentQuest.currentIndex < count - 1)
        {
            //進到下一個任務階段
            currentQuest.currentIndex++;
            currentQuest.currentAmount = 0;
            questUI.UpdateQuestContent(currentQuest.questSO, currentQuest.currentIndex);
            questUI.UpdateConditionText();
        }
        else
        {
            activeQuest.Remove(currentQuest);
            currentQuest.questSO.isComplet=true;
            currentQuest = null;
            questUI.UpdateQuestContent(null, 0);
            questUI.UpdateConditionText();
            UIManager.Instance.ShowQuestUI(false);
            Debug.Log("任務完成");
        }
    }



    public bool GetQuestComplete()
    {
        if (currentQuest == null) return false;

        if (currentQuest.currentAmount >= currentQuest.questSO.Quest[currentQuest.currentIndex].requiredAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
