using UnityEngine;
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest Data")]
public class QuestSO : ScriptableObject
{
    [Header("任務參數")]
    //任務ID
    public int questID;
    // public bool[] endLine;
    [Header("任務內容")]
    public Quest[] Quest;
    public bool isComplet;
}
[System.Serializable]
public class Quest
{
    public string questName;
    public int subID;
    [TextArea]
    public string description;
    public QuestStatusType questStatusType;
    public int requiredAmount; //任務需求量
}

[System.Serializable]
public class QuestOnRunTime
{
    public QuestSO questSO;
    public int currentAmount;
    public int currentIndex;
    // public bool isComplet;
    public void SetQuest(QuestSO quest)
    {
        questSO = quest;
        currentAmount = 0;
        currentIndex = 0;
    }
}
public enum QuestStatusType
{
    CollectItem,
    ReachLocation,
    InteractObject
}
