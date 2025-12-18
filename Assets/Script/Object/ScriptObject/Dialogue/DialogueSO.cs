using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogueSO", menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string targetName;
    public float typeingSpeed = 0.05f;
    public DialogueContent[] dialogueContent;
}
[System.Serializable]
public class DialogueContent
{
    [TextArea] public string[] dialogueLines;
    public bool[] endDialogueLines;
    public DialogueChioce[] choices;
}
[System.Serializable]
public class DialogueChioce
{
    [Header("參數")]
    public int checkNumber = -1;
    public int triggerIndex;
    [Header("選項")]
    public string[] options;
    public int[] nextDialogueIndex;

    [Tooltip("如果有選項是需要判定有沒有完成任務時使用")]
    public int[] notCompleteNextIndex;
}
