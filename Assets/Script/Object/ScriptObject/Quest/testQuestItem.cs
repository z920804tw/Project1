using UnityEngine;

public class testQuestItem : MonoBehaviour
{
    [SerializeField] QuestSO questSO;
    [SerializeField] int subId;
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
        UIManager.Instance.questUI.UpdateQuestProgress(questSO.Quest[subId].questStatusType,subId, addAmount);
    }
}
