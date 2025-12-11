using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    PlayerInput playerInput;
    [Header("參數設定")]
    [SerializeField] DialogueSO currentSO;
    [SerializeField] TMP_Text nameText;//物件名稱
    [SerializeField] TMP_Text contentText;//對話內容
    [SerializeField] GameObject hintText;
    [SerializeField] Transform choiceBtnParent;
    [SerializeField] GameObject choiceBtnPrefab;

    GameObject currentTarget;
    string[] dialogueLines;
    [Header("Debug")]
    [SerializeField] int currentIndex;
    [SerializeField] bool isTyping;
    bool isChoice;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
        ClearChoiceBtn();
        StartCoroutine(DelayShowText());
    }

    public void NextLine()
    {
        //檢查當前的對話是不是結束對話
        if (currentSO.endDialogueLines[currentIndex])
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
    public void EndDialogue()
    {
        //關閉對話UI、清空資訊、解除按鍵監聽
        ResetInfo();
        DisSubDialogueInput();
        this.gameObject.SetActive(false);
        //切換玩家狀態至普通模式
        if (currentTarget != null) currentTarget.GetComponent<PlayerStatus>().SetStatus(Status.Normal);
    }

    void ResetInfo()
    {
        currentSO = null;
        nameText.text = string.Empty;
        contentText.text = string.Empty;
        currentIndex = 0;
        dialogueLines = null;
        isTyping = false;
        hintText.SetActive(false);
        ClearChoiceBtn();
    }

    void CheckShowChoice()
    {
        if (currentSO.chioces.Length > 0)
        {
            for (int i = 0; i < currentSO.chioces.Length; i++)
            {
                if (currentIndex == currentSO.chioces[i].dialogueIndex)
                {
                    isChoice = true;
                    //一樣就產生選項按鈕
                    for (int y = 0; y < currentSO.chioces[i].options.Length; y++)
                    {
                        GameObject btnObj = Instantiate(choiceBtnPrefab, choiceBtnParent);
                        btnObj.GetComponentInChildren<TMP_Text>().text = currentSO.chioces[i].options[y];

                        Button btn = btnObj.GetComponent<Button>();
                        int nextIndex = currentSO.chioces[i].nextDialogueIndex[y];


                        //當按鈕被按下去後會執行的功能
                        btn.onClick.AddListener(() =>
                        {
                            //如果該按鈕有Event就執行該按鈕的Event

                            currentIndex = nextIndex;
                            isChoice = false;
                            ClearChoiceBtn();
                            StartCoroutine(DelayShowText());
                        });
                    }
                    return;
                }
            }

            if (!isChoice)
            {
                hintText.SetActive(true);
            }
        }

    }

    //清空按鈕選項
    void ClearChoiceBtn()
    {
        foreach (Transform i in choiceBtnParent)
        {
            Destroy(i.gameObject);
        }
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
        Debug.Log("當前對話跑完");

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
