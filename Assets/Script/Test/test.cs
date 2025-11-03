using UnityEngine;

public class test : MonoBehaviour
{
    public InventorySystem inventorySystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<item>() != null)
                {
                    inventorySystem.AddItemToInventory(hit.transform.gameObject);
                    Debug.Log(hit.transform.GetComponent<item>().type);
                }

            }
        }

    }

}
