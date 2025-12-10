using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSO", menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string targetName;
    [TextArea]public string[] dialogueLines;
    public float typeingSpeed=0.05f;
    
}
