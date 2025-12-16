using UnityEngine;

public class testQuestItem : MonoBehaviour
{

    [SerializeField] QuestStatusType questStatusType;
    [SerializeField] int id;
    [SerializeField] int questIndex;
    [SerializeField] int addAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CollectQuestItem()
    {
        UIManager.Instance.questUI.UpdateQuestProgress(id, questStatusType, questIndex, addAmount);
    }
}
