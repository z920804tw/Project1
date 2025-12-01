using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("梯子起始位置設定")]
    [SerializeField] Transform bottomPos;
    [SerializeField] Transform topPos;
    [Header("梯子離開位置設定")]
    [SerializeField] Transform topEndPos;
    [SerializeField] Transform bottomEndPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //給底部Trigger用的
    public void EnterBottomLadder(GameObject target)
    {
        PlayerStatus player = target.GetComponent<PlayerStatus>();
        if (player != null)
        {

            player.transform.position = new Vector3(bottomPos.position.x, player.transform.position.y, bottomPos.position.z);
            player.transform.eulerAngles = transform.localEulerAngles;
            player.SetStatus(Status.Climb);

            player.playerClimb.SetExitPos(topEndPos, bottomEndPos);

        }
    }

    //給頂部Trigger用的
    public void EnterTopLadder(GameObject target)
    {
        PlayerStatus player = target.GetComponent<PlayerStatus>();
        if (player != null)
        {
            player.transform.position = topPos.position;
            player.transform.eulerAngles = transform.localEulerAngles;
            player.SetStatus(Status.Climb);

            player.playerClimb.SetExitPos(topEndPos, bottomEndPos);
        }
    }

}
