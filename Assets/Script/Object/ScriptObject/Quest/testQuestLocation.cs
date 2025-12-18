using UnityEngine;

public class testQuestLocation : MonoBehaviour
{
    [SerializeField] QuestSO questSO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (UIManager.Instance.questManager.CurrentQuest != null)
            {
                if (UIManager.Instance.questManager.CurrentQuest.questSO == questSO)
                {
                    UIManager.Instance.questManager.UpdateQuestProgress(questSO.Quest[2].questStatusType, 2, 1);
                    // Destroy(gameObject);
                }
            }

        }
    }
}
