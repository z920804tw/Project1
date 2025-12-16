using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [Header("參數")]
    [SerializeField] QuestOnRunTime currentQuest;
    [SerializeField] List<QuestOnRunTime> activeQuest = new List<QuestOnRunTime>();
    [Header("組件套用")]
    [SerializeField] TMP_Text questTitle;
    [SerializeField] TMP_Text questDescription;
    [SerializeField] TMP_Text questCondition;

    public void AddQuest(QuestSO quest)
    {
        //如果該任務已經完成，就不重複接取
        if (quest.isComplet) return;

        QuestOnRunTime questOnRunTime = new QuestOnRunTime();
        questOnRunTime.SetQuest(quest);
        activeQuest.Add(questOnRunTime);

        currentQuest = questOnRunTime;
        UpdateQuestContent(currentQuest.questSO, questOnRunTime.currentIndex);
        UpdateConditionText();
        Debug.Log($"新增任務:{questOnRunTime.questSO.Quest[0].questName}");
    }



    //更新任務狀態
    public void UpdateQuestProgress(int questID, QuestStatusType questStatusType, int questIndex, int amount)
    {
        //檢查有沒有任務，如果沒有就return
        if (activeQuest.Count == 0 || currentQuest == null) return;

        QuestSO current=currentQuest.questSO;
        //檢查互動物件傳入的ID和任務類型是不是與當前的任務是一樣的
        if (current.questID == questID && current.Quest[currentQuest.currentIndex].questStatusType == questStatusType)
        {
            //跟新當前進度
            currentQuest.currentAmount += amount;
            UpdateConditionText();

            if (currentQuest.currentAmount == current.Quest[currentQuest.currentIndex].requiredAmount)
            {
                //該任務完成，檢查是不是尾端，就更新執行下一個任務，否則就刪除該任務
                if (current.endLine.Length != 0)
                {
                    if (!current.endLine[questIndex])
                    {
                        //下一個任務
                        currentQuest.currentIndex++;
                        currentQuest.currentAmount = 0;
                        UpdateQuestContent(currentQuest.questSO, currentQuest.currentIndex);
                        UpdateConditionText();
                    }
                    else
                    {
                        current.isComplet = true;
                        activeQuest.Remove(currentQuest);
                        currentQuest = null;
                        UpdateQuestContent(null, 0);
                        Debug.Log("當前任務完成");
                    }
                }
            }
        }
    }

    void UpdateConditionText()
    {
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
                questCondition.text=$"";
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
}
