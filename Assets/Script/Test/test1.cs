using UnityEngine;
using UnityEngine.InputSystem;

public class test1 : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void GetTarget(GameObject target)
    {
        Debug.Log("觸發GameObjectEvent，目標：" + target.name);
    }
    public void Debug1()
    {
        Debug.Log("觸發一般Event");
    }
}
