using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("參數")]
    [SerializeField] QuestOnRunTime currentQuest;
    public QuestOnRunTime CurrentQuest { get { return currentQuest; } }
    [SerializeField] List<QuestOnRunTime> activeQuest = new List<QuestOnRunTime>();
    [Header("組件套用")]
    [SerializeField] TMP_Text questTitle;
    [SerializeField] TMP_Text questDescription;
    [SerializeField] TMP_Text questCondition;

    public void AddQuest(QuestSO quest)
    {
        QuestOnRunTime questOnRunTime = new QuestOnRunTime();
        questOnRunTime.SetQuest(quest);
        activeQuest.Add(questOnRunTime);

        currentQuest = questOnRunTime;
        UpdateQuestContent(currentQuest.questSO, questOnRunTime.currentIndex);
        UpdateConditionText();
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
            UpdateConditionText();
            if (currentQuest.currentAmount >= questStep.requiredAmount)
            {
                currentQuest.currentAmount = questStep.requiredAmount;
                NextStepOrComplete();
            }
        }

    }

    public void NextStepOrComplete()
    {
        int count = currentQuest.questSO.Quest.Count();
        if (currentQuest.currentIndex < count - 1)
        {
            //進到下一個任務階段
            currentQuest.currentIndex++;
            currentQuest.currentAmount = 0;
            UpdateQuestContent(currentQuest.questSO, currentQuest.currentIndex);
            UpdateConditionText();
        }
        else
        {
            activeQuest.Remove(currentQuest);
            currentQuest = null;
            UpdateQuestContent(null, 0);
            UpdateConditionText();
            Debug.Log("任務完成");
        }
    }

    void UpdateConditionText()
    {
        if (currentQuest == null)
        {
            questCondition.text = "";
            return;
        }

        QuestStatusType type = currentQuest.questSO.Quest[currentQuest.currentIndex].questStatusType;
        switch (type)
        {
            case QuestStatusType.CollectItem:
                questCondition.text = $"Item Collect:{currentQuest.currentAmount}/{currentQuest.questSO.Quest[currentQuest.currentIndex].requiredAmount}";
                break;
            case QuestStatusType.ReachLocation:
                questCondition.text = $"";
                break;
            case QuestStatusType.InteractObject:
                questCondition.text = $"";
                break;
        }
    }
    public void UpdateQuestContent(QuestSO questSO, int index)
    {
        if (questSO == null)
        {
            questTitle.text = "";
            questDescription.text = "";
            questCondition.text = "";
        }
        else
        {
            questTitle.text = questSO.Quest[index].questName;
            questDescription.text = questSO.Quest[index].description;
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
