using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] GameObject hintUI;
    public Transform Content { get { return content; } }

    public List<GameObject> hintUIList;
    [SerializeField] int selectIndex;
    public int SelectIndex { get { return selectIndex; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hintUI.activeSelf)
        {
            Vector2 h = hintUI.GetComponent<RectTransform>().sizeDelta;
            h.y = content.GetComponent<RectTransform>().sizeDelta.y;
            hintUI.GetComponent<RectTransform>().sizeDelta = h;
        }
    }
    //----------HintUI-----------//
    public void HintUISelect(int value)
    {
        selectIndex += value;
        if (selectIndex >= hintUIList.Count)
        {
            selectIndex = 0;
        }
        else if (selectIndex < 0)
        {
            selectIndex = hintUIList.Count - 1;
        }

        if (hintUIList.Count > 0)
        {
            foreach (GameObject i in hintUIList)
            {
                i.GetComponent<Hint>().ShowSelect(false);
            }
            hintUIList[selectIndex].GetComponent<Hint>().ShowSelect(true);
        }
    }
    //----------HintUI-----------//
    public void ResetHintInfo()
    {
        if (hintUIList.Count > 0)
        {
            foreach (GameObject i in hintUIList)
            {
                Destroy(i);
            }
            hintUIList.Clear();
            selectIndex=0;
        }
    }

}
