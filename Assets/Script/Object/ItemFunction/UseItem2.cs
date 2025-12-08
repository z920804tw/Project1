using UnityEngine;

public class UseItem2 : MonoBehaviour, IUse
{
    public void ResetUse()
    {
        Debug.Log("物品取消使用");
    }

    public void UseObject(GameObject target)
    {
        Debug.Log("物品使用");

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
