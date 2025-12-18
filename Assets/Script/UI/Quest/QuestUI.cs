using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{

    [Header("組件套用")]
    [SerializeField] QuestManager questManager;
    [SerializeField] TMP_Text questTitle;
    [SerializeField] TMP_Text questDescription;
    [SerializeField] TMP_Text questCondition;

    void Start()
    {
        questManager=GetComponent<QuestManager>();
    }
    public void UpdateConditionText()
    {
        if (questManager.CurrentQuest == null)
        {
            questCondition.text = "";
            return;
        }

        QuestStatusType type = questManager.CurrentQuest.questSO.Quest[questManager.CurrentQuest.currentIndex].questStatusType;
        switch (type)
        {
            case QuestStatusType.CollectItem:
                questCondition.text = $"Item Collect:{questManager.CurrentQuest.currentAmount}/{questManager.CurrentQuest.questSO.Quest[questManager.CurrentQuest.currentIndex].requiredAmount}";
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
}
