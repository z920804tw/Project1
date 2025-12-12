using UnityEngine;

public class IuseTest : MonoBehaviour,IUse
{
    public void ResetUse()
    {
        Debug.Log("取消使用");
    }

    public void UseObject(GameObject target)
    {
        Debug.Log("使用");
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
