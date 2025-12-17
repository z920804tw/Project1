using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    PlayerInput playerInput;
    [Header("參數設定")]
    [SerializeField] DialogueSO currentSO;
    [SerializeField] DialogueEvent[] dialogueEvents;
    GameObject currentTarget;
    GameObject interactTarget;
    string[] dialogueLines;
    [Header("UI物件套用")]
    [SerializeField] TMP_Text nameText;//物件名稱
    [SerializeField] TMP_Text contentText;//對話內容
    [SerializeField] GameObject hintText;
    [SerializeField] Transform choiceBtnParent;
    [SerializeField] GameObject choiceBtnPrefab;
    [Header("Debug")]
    [SerializeField] int dialogueIndex;
    int currentIndex;
    [SerializeField] bool isTyping;
    public bool isCheck;
    bool isChoice;

    //設定目標
    public void SetTarget(GameObject talk, GameObject interact)
    {
        currentTarget = talk;
        interactTarget = interact;
    }
    //設定基礎訊息
    public void SetDialogueInfo(DialogueSO dialogueSO, DialogueEvent[] events)
    {
        currentSO = dialogueSO;
        dialogueIndex = 0;
        currentIndex = 0;
        nameText.text = dialogueSO.targetName;
        dialogueLines = dialogueSO.dialogueContent[dialogueIndex].dialogueLines;
        contentText.text = string.Empty;

        //套用事件內容
        dialogueEvents = events;

        //先清空按鈕
        ClearChoiceBtn();
        StartCoroutine(DelayShowText());
    }

    //下一行
    public void NextLine()
    {
        bool[] endDialogueLines = currentSO.dialogueContent[dialogueIndex].endDialogueLines;
        //檢查當前的對話是不是結束對話
        if (endDialogueLines.Length != 0 && endDialogueLines[currentIndex])
        {
            EndDialogue();
            return;
        }
        if (currentIndex < dialogueLines.Length - 1)
        {
            currentIndex++;
            StartCoroutine(DelayShowText());
        }
    }
    //結束對話
    public void EndDialogue()
    {
        NpcInteractDialogue npcInteractDialogue = interactTarget.GetComponent<NpcInteractDialogue>();
        if (npcInteractDialogue != null && npcInteractDialogue.CanLook)
        {
            interactTarget.transform.DOLookAt(npcInteractDialogue.DefaultLook, 1f, AxisConstraint.Y).SetEase(Ease.InOutSine);
        }
        //解除按鍵監聽
        DisSubDialogueInput();
        //切換玩家狀態至普通模式
        if (currentTarget != null) currentTarget.GetComponent<PlayerStatus>().SetStatus(Status.Normal);
        //關閉對話UI、清空資訊、解除按鍵監聽
        ResetInfo();
    }

    //重置資訊
    void ResetInfo()
    {
        currentSO = null;
        nameText.text = string.Empty;
        contentText.text = string.Empty;
        dialogueIndex = 0;
        currentIndex = 0;
        dialogueLines = null;
        dialogueEvents = null;
        currentTarget = null;
        interactTarget = null;

        isTyping = false;
        hintText.SetActive(false);
        this.gameObject.SetActive(false);
        ClearChoiceBtn();
    }

    //檢查有沒有選項可用
    void CheckShowChoice()
    {
        DialogueChioce[] dialogueChioce = currentSO.dialogueContent[dialogueIndex].choices;
        if (dialogueChioce.Length > 0)
        {
            for (int x = 0; x < dialogueChioce.Length; x++)
            {
                if (currentIndex == dialogueChioce[x].dialogueIndex) //檢查當前index是否與chioces[i].dialogueIndex的參數一樣，如果一樣就代表當前對話有選擇
                {
                    isChoice = true;
                    //找當前Choice的選項有沒有符合的條件
                    int currentEventIndex = FindTrueEvent(x);
                    for (int y = 0; y < dialogueChioce[x].options.Length; y++)
                    {
                        //生成按鈕、給予按鈕按下時的功能
                        SpawnOptionButton(x, y, currentEventIndex);
                    }
                    return;
                }
            }

            //判定有沒有選擇，如果沒有就顯示提示
            if (!isChoice)
            {
                hintText.SetActive(true);
            }
        }
    }
    int FindTrueEvent(int x)
    {
        DialogueChioce dialogueChioce = currentSO.dialogueContent[dialogueIndex].choices[x];
        for (int i = 0; i < dialogueEvents.Length; i++)
        {
            if (dialogueEvents[i].checkNumber == dialogueChioce.checkNumber)
            {
                return i;
            }
        }
        return -1;
    }
    void SpawnOptionButton(int x, int y, int currentEventIndex)
    {
        DialogueChioce[] dialogueChioce = currentSO.dialogueContent[dialogueIndex].choices;
        GameObject btnObj = Instantiate(choiceBtnPrefab, choiceBtnParent);
        btnObj.GetComponentInChildren<TMP_Text>().text = dialogueChioce[x].options[y];

        Button btn = btnObj.GetComponent<Button>();
        int nextIndex = dialogueChioce[x].nextDialogueIndex[y];


        //當按鈕被按下去後會執行的功能
        btn.onClick.AddListener(() =>
        {
            //如果有事件，就綁定事件到按鈕上
            if (currentEventIndex != -1 && dialogueEvents.Length != 0 && y < dialogueEvents[x].options.Length)
            {
                dialogueEvents[currentEventIndex].options[y].Invoke();
            }

            if (isCheck) return;

            dialogueIndex = nextIndex;
            dialogueLines = currentSO.dialogueContent[dialogueIndex].dialogueLines;
            currentIndex = 0;
            isChoice = false;
            ClearChoiceBtn();
            StartCoroutine(DelayShowText());
        });
    }

    //清空按鈕選項
    void ClearChoiceBtn()
    {
        foreach (Transform i in choiceBtnParent)
        {
            Destroy(i.gameObject);
        }
    }
    public void SetDialogueIndex(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueLines = currentSO.dialogueContent[dialogueIndex].dialogueLines;
        isChoice=false;
        ClearChoiceBtn();
        StartCoroutine(DelayShowText());
    }
    IEnumerator DelayShowText()
    {
        isTyping = true;
        contentText.text = string.Empty;
        hintText.SetActive(false);

        Char[] chars = dialogueLines[currentIndex].ToCharArray();
        foreach (char c in chars)
        {
            contentText.text += c;
            yield return new WaitForSeconds(currentSO.typeingSpeed);
        }

        contentText.text = dialogueLines[currentIndex];
        isTyping = false;
        //檢查是否有選項可以顯示選擇
        CheckShowChoice();
    }


    //-------------對話按鍵功能----------------//
    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (!isChoice)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                contentText.text = dialogueLines[currentIndex];
                isTyping = false;
                CheckShowChoice();
            }
            else
            {
                NextLine();
            }
        }
    }
    //-------------對話按鍵功能----------------//
    //-------------對話按鍵監聽綁定、取消----------------//
    public void SubDialogueInput()
    {
        if (playerInput == null)
        {
            playerInput = GameManager.Instance.playerInput;
        }
        playerInput.actions["Click"].performed += OnClick;
        Debug.Log("啟用對話監聽");
    }

    void DisSubDialogueInput()
    {
        playerInput.actions["Click"].performed -= OnClick;
        Debug.Log("取消對話監聽");
    }
}
