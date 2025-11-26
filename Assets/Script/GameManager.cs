using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerInput playerInput;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowCursor(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowCursor(bool t)
    {
        if (t)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void SwitchInputMode(string mode)
    {
        playerInput.SwitchCurrentActionMap(mode);
        Debug.Log($"切換至:{mode} Input");
    }
}
