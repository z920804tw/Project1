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
    public int checkNumber=-1;
    public int dialogueIndex;
    public string[] options;
    public int[] nextDialogueIndex;
}
