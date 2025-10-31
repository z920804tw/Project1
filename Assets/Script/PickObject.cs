using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PickObject : MonoBehaviour
{
    [Header("組件套用")]
    public SphereCollider triggerBox;
    public BoxCollider boxCollider;
    Rigidbody rb;
    [Header("物件套用")]

    public GameObject pickHint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ShowCloseInfo(bool t)
    {
        pickHint.SetActive(t);
    }

    public void ColliderAndRig(bool t)
    {
        triggerBox.enabled = t;
        rb.isKinematic = !t;
    }

    public void Throw(Vector3 dir)
    {
        ColliderAndRig(true);
        Physics.IgnoreCollision(boxCollider, GameObject.FindWithTag("Player").GetComponent<Collider>(), true);
        Physics.IgnoreCollision(triggerBox, GameObject.FindWithTag("Player").GetComponent<Collider>(), true);
        rb.AddForce(dir, ForceMode.Impulse);
        StartCoroutine(WaitReset());
    }


    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(0.5f);
        Physics.IgnoreCollision(boxCollider, GameObject.FindWithTag("Player").GetComponent<Collider>(), false);
        Physics.IgnoreCollision(triggerBox, GameObject.FindWithTag("Player").GetComponent<Collider>(), false);
    }

}
