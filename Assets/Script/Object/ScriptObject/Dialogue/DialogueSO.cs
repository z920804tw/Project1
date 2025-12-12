using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogueSO", menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string targetName;
    [TextArea] public string[] dialogueLines;
    public bool[] endDialogueLines;
    public float typeingSpeed = 0.05f;
    public DialogueChioce[] choices;
}

[System.Serializable]
public class DialogueChioce
{
    public int dialogueIndex;
    public string[] options;
    public int[] nextDialogueIndex;
}
