using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class test1 : MonoBehaviour
{
    public Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 dir = target.transform.position - transform.position;
        transform.DOLookAt(target.transform.position, 1f, AxisConstraint.Y).SetEase(Ease.InOutSine);
        Debug.DrawRay(transform.position, dir, Color.red,10f);
    }

    // Update is called once per frame
    void Update()
    {


    }

}
