using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] GameObject hintGameObjcet;
    public GameObject HintGameObjcet { get { return hintGameObjcet; } set { hintGameObjcet = value; } }
    [SerializeField] GameObject selectUI;
    [SerializeField] TMP_Text hintText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetHintInfo(GameObject target, string text)
    {
        hintGameObjcet = target;
        hintText.text = text;
    }
    public void ShowSelect(bool t)
    {
        selectUI.SetActive(t);
    }
}
