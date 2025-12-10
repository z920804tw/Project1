using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] DialogueSO currentSO;
    [SerializeField] TMP_Text nameText;//物件名稱
    [SerializeField] TMP_Text contentText;//對話內容
    [SerializeField] Button closeBtn;

    GameObject currentTarget;
    string[] dialogueLines;
    [SerializeField] int currentIndex;
    [SerializeField] bool isTyping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                contentText.text = dialogueLines[currentIndex];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }

    }
    public void SetTarget(GameObject target)
    {
        currentTarget = target;
    }
    public void SetDialogueInfo(DialogueSO dialogueSO)
    {
        currentSO = dialogueSO;
        nameText.text = dialogueSO.targetName;
        dialogueLines = dialogueSO.dialogueLines;
        currentIndex = 0;
        contentText.text = string.Empty;
        StartCoroutine(DelayShowText());
    }

    public void NextLine()
    {
        if (currentIndex < dialogueLines.Length - 1)
        {
            currentIndex++;
            contentText.text = string.Empty;
            StartCoroutine(DelayShowText());
        }
        else
        {
            closeBtn.gameObject.SetActive(true);
        }
    }
    public void EndDialogue()
    {
        //關閉對話UI並清空資訊
        ResetInfo();
        this.gameObject.SetActive(false);
        //切換玩家狀態至普通模式
        if (currentTarget != null) currentTarget.GetComponent<PlayerStatus>().SetStatus(Status.Normal);
    }

    void ResetInfo()
    {
        currentSO = null;
        nameText.text = string.Empty;
        contentText.text = string.Empty;
        closeBtn.gameObject.SetActive(false);
        currentIndex = 0;
        dialogueLines = null;
        isTyping = false;
    }
    IEnumerator DelayShowText()
    {
        isTyping = true;
        Char[] chars = dialogueLines[currentIndex].ToCharArray();
        foreach (char c in chars)
        {
            contentText.text += c;
            yield return new WaitForSeconds(currentSO.typeingSpeed);
        }

        contentText.text = dialogueLines[currentIndex];
        isTyping = false;
        Debug.Log("當前對話跑完");


        if (currentIndex == dialogueLines.Length - 1)
        {
            closeBtn.gameObject.SetActive(true);
        }
    }
}
