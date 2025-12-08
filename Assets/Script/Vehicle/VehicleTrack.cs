
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Splines;

public class VehicleTrack : MonoBehaviour
{
    [SerializeField] VehicleController controller;
    [SerializeField] VehicleSetting vehicleSetting;
    [SerializeField] Transform[] rightTracksPoint;
    [SerializeField] Transform[] leftTrackPoint;
    [SerializeField] float trackSpeed;
    [SerializeField] LayerMask groundLayer;
    float timer;
    [SerializeField] bool isMove;
    [SerializeField] Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DOTween.SetTweensCapacity(500, 20);
    }

    // Update is called once per frame
    void Update()
    {

        if (vehicleSetting.IsEngineStart && controller.CurrentSpeed > 0.01)
        {
            float move = controller.vehicleInput.y; // W/S
            float turn = controller.vehicleInput.x; // A/D

            SetTrackToNextPosition(move, turn);
        }
    }
    void SetTrackToNextPosition(float forward, float turn)
    {
        if (!isMove)
        {
            isMove = true;
            // 記錄當前每個履帶的位置與旋轉
            Transform[] rightTransform = new Transform[rightTracksPoint.Length];
            Transform[] lefttransform = new Transform[leftTrackPoint.Length];
            for (int i = 0; i < rightTracksPoint.Length; i++)
            {
                rightTransform[i] = rightTracksPoint[i].transform;
                lefttransform[i] = leftTrackPoint[i].transform;
            }
            //記錄當前每個履帶的位置與旋轉

            // 決定哪一邊履帶要動
            bool leftActive = true;
            bool rightActive = true;

            if (turn > 0)// D 動左履帶 右履帶不動
            {
                rightActive = false;
            }

            else if (turn < 0) // A  動右履帶
            {
                leftActive = false;
            }

            // 移動方向：前(+1)，後(-1)，不動(0)
            int leftDir;
            if (leftActive)
            {
                if (forward > 0) leftDir = 1;
                else if (forward < 0) leftDir = -1;
                else leftDir = 0;
            }
            else leftDir = 0;

            int rightDir;
            if (rightActive)
            {
                if (forward > 0) rightDir = 1;
                else if (forward < 0) rightDir = -1;
                else rightDir = 0;
            }
            else rightDir = 0;

            // 將所有節點旋轉加入 Sequence（同時旋轉）
            Sequence trackSeq = DOTween.Sequence();

            if (leftActive)
            {
                //左履帶更新
                for (int i = 0; i < leftTrackPoint.Length; i++)
                {
                    int leftNext = GetNextIndex(i, leftTrackPoint.Length, leftDir);
                    trackSeq.Join(leftTrackPoint[i].DOLocalMove(lefttransform[leftNext].localPosition, trackSpeed).SetEase(Ease.Linear));
                    trackSeq.Join(leftTrackPoint[i].DOLocalRotateQuaternion(Quaternion.Euler(lefttransform[leftNext].localEulerAngles), trackSpeed).SetEase(Ease.Linear));

                }
            }

            if (rightActive)
            {
                //右履帶更新
                for (int i = 0; i < rightTracksPoint.Length; i++)
                {
                    int rightNext = GetNextIndex(i, rightTracksPoint.Length, rightDir); ;
                    trackSeq.Join(rightTracksPoint[i].DOLocalMove(rightTransform[rightNext].localPosition, trackSpeed).SetEase(Ease.Linear));
                    trackSeq.Join(rightTracksPoint[i].DOLocalRotateQuaternion(Quaternion.Euler(rightTransform[rightNext].localEulerAngles), trackSpeed).SetEase(Ease.Linear));
                }
            }

            trackSeq.OnComplete(() =>
            {
                isMove = false;
            });
            trackSeq.SetAutoKill(true);
        }
    }
    int GetNextIndex(int i, int length, int dir)
    {
        if (dir == 1)     // 前進
            return (i + 1) % length;
        else if (dir == -1) // 後退
            return (i - 1 + length) % length;

        return i; // 不動
    }

    // void UpdateTrackOrientation(Transform link)
    // {
    //     RaycastHit hit;

    //     if (Physics.Raycast(link.position, link.up, out hit, 0.05f, groundLayer))
    //     {

    //         Vector3 pos = link.position;
    //         pos.y = hit.point.y + 0.02f;
    //         link.position = pos;

    //         float angle = Vector3.SignedAngle(Vector3.up, hit.normal, link.right);
    //         float angle2 = angle + 180;
    //         link.transform.localEulerAngles = new Vector3(angle2, 0, 0);
    //         // Debug.Log(angle2 + " " + hit.normal);
    //     }
    //     else
    //     {
    //         // link.position=defalutPos;
    //         // link.localEulerAngles=defalutRot;
    //     }

    //     Debug.DrawRay(link.transform.position, link.up * 0.05f, Color.red);
    // }

}
