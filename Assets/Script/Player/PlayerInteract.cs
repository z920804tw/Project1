using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] InteractObject interactObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnInteract(InputValue value)
    {
        if (interactObject != null)
        {
            interactObject.DoEvent();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractObject"))
        {
            if (other.GetComponent<InteractObject>() != null && interactObject == null)
            {
                interactObject = other.GetComponent<InteractObject>();
                interactObject.ShowHint(true);
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractObject"))
        {
            if (interactObject != null && interactObject == other.GetComponent<InteractObject>())
            {
                interactObject.ShowHint(false);
                interactObject = null;
            }
        }
    }
}
