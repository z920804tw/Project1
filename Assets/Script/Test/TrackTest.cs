using DG.Tweening;
using UnityEngine;

public class TrackTest : MonoBehaviour
{
    [SerializeField] Transform[] rightTracksPoint;
    [SerializeField] Transform track;
    [SerializeField] float defaultOffset;
    bool isMove;




    public LayerMask groundLayer;
    public float rayDistance = 0.5f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isMove)
        {
            isMove = true;
            track.transform.DOMove(new Vector3(track.position.x, track.position.y, track.position.z + 0.5f), 0.2f).SetEase(Ease.Linear);
            Vector3[] rightPos = new Vector3[rightTracksPoint.Length];
            Vector3[] rightRot = new Vector3[rightTracksPoint.Length];

            for (int i = 0; i < rightTracksPoint.Length; i++)
            {
                rightPos[i] = rightTracksPoint[i].localPosition;
                rightRot[i] = rightTracksPoint[i].localEulerAngles;
            }

            Sequence trackSeq = DOTween.Sequence();

            //右履帶更新
            for (int i = 0; i < rightTracksPoint.Length; i++)
            {
                int rightNext = i + 1;
                int current = i;
                if (rightNext >= rightTracksPoint.Length) rightNext = 0;
                trackSeq.Join(rightTracksPoint[i].DOLocalMove(rightPos[rightNext], 0.2f).SetEase(Ease.Linear));
                trackSeq.Join(rightTracksPoint[i].DOLocalRotateQuaternion(Quaternion.Euler(rightRot[rightNext]), 0.2f).SetEase(Ease.Linear));
            }

            trackSeq.OnComplete(() =>
            {
                isMove = false;

            });
        }
    }
    void UpdateTrackOrientation(Transform link)
    {
        RaycastHit hit;
        if (Physics.Raycast(link.position, link.up, out hit, rayDistance, groundLayer))
        {

            Vector3 pos = link.position;
            pos.y = hit.point.y + 0.02f;
            link.position = pos;
            Debug.Log(123);

            // float angle = Vector3.SignedAngle(Vector3.up, hit.normal, link.right);
            // float angle2 = angle + 180;
            // link.transform.eulerAngles = new Vector3(angle2, 0, 0);
            // Debug.Log(angle2 + " " + hit.normal);
        }

        Debug.DrawRay(link.transform.position, link.up * rayDistance, Color.red);
    }
}
