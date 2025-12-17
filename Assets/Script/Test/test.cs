using UnityEngine;

public class test : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    public void GiveA(string name)
    {
        Debug.Log($"Give {name}");
    }

    public void GiveQuest(QuestSO questSO)
    {
        UIManager.Instance.questUI.AddQuest(questSO);
    }
}
