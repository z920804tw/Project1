using System.Collections;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("梯子起始位置設定")]
    [SerializeField] Transform bottomPos;
    [SerializeField] Transform topPos;
    [Header("梯子離開位置設定")]
    [SerializeField] Transform topEndPos;
    [SerializeField] Transform bottomEndPos;

    [Header("Debug")]
    [SerializeField] GameObject currentTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //-------------UnityEvent使用---------------//
    //給底部Trigger用的
    public void EnterBottomLadder(GameObject target)
    {
        PlayerStatus player = target.GetComponent<PlayerStatus>();
        if (player != null)
        {
            currentTarget = player.gameObject;
            Vector3 btmPos = new Vector3(bottomPos.position.x, player.transform.position.y, bottomPos.position.z);
            player.SetStatus(Status.Climb);
            player.playerClimb.SetClimbInfo(this, topEndPos, bottomEndPos);

            Quaternion rot = transform.rotation;
            StartCoroutine(SmoothTranslate(player.gameObject, player.transform.position, btmPos, rot, 0.5f));
        }
    }

    //給頂部Trigger用的
    public void EnterTopLadder(GameObject target)
    {
        PlayerStatus player = target.GetComponent<PlayerStatus>();
        if (player != null)
        {
            currentTarget = player.gameObject;
            player.SetStatus(Status.Climb);
            player.playerClimb.SetClimbInfo(this, topEndPos, bottomEndPos);

            Quaternion rot = transform.rotation;
            StartCoroutine(SmoothTranslate(player.gameObject, player.transform.position, topPos.position, rot, 0.5f));
        }
    }
    //-------------UnityEvent使用---------------//

    public void ExitLadder(Transform exitPos) //當玩家觸碰到離開區域時會使用到
    {
        if (currentTarget != null)
        {
            Quaternion rot = transform.rotation;

            StartCoroutine(SmoothTranslate(currentTarget, currentTarget.transform.position, exitPos.position, rot, 0.5f));
            StartCoroutine(ResetInfo());
        }
    }

    //Smooth功能
    IEnumerator SmoothTranslate(GameObject target, Vector3 start, Vector3 end, Quaternion rot, float duration)
    {
        float timer = 0;
        Quaternion starRot = target.transform.rotation;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            target.transform.position = Vector3.Lerp(start, end, timer / duration);
            target.transform.rotation = Quaternion.Slerp(starRot, rot, timer / duration);
            yield return null;
        }
        target.transform.position = end;
    }
    IEnumerator ResetInfo()
    {
        yield return new WaitForSeconds(0.5f);
        currentTarget = null;
    }

}
