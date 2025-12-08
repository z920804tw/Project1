using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] GameObject currentTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetDialogueInfo(GameObject target)
    {
        currentTarget = target;
    }

    public void CloseDialogue()
    {

        PlayerStatus playerStatus = currentTarget.GetComponent<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.playerCam.DisSubAllCameraInput();
            playerStatus.SetStatus(Status.Normal);
        }

        UIManager.Instance.ShowDialogueUI(false);
    }
}
