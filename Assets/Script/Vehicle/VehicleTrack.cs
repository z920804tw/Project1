
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
    float timer;
    [SerializeField] bool isMove;

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
            Vector3[] rightPos = new Vector3[rightTracksPoint.Length];
            Vector3[] rightRot = new Vector3[rightTracksPoint.Length];

            Vector3[] leftPos = new Vector3[leftTrackPoint.Length];
            Vector3[] leftRot = new Vector3[leftTrackPoint.Length];
            for (int i = 0; i < rightTracksPoint.Length; i++)
            {
                rightPos[i] = rightTracksPoint[i].localPosition;
                rightRot[i] = rightTracksPoint[i].localEulerAngles;

                leftPos[i] = leftTrackPoint[i].localPosition;
                leftRot[i] = leftTrackPoint[i].localEulerAngles;
            }
            //記錄當前每個履帶的位置與旋轉

            // 決定哪一邊履帶要動
            bool leftActive = false;
            bool rightActive = false;

            if (turn > 0)// D 動左履帶
            {
                leftActive = true;
            }

            else if (turn < 0) // A  動右履帶
            {
                rightActive = true;
            }

            else // 沒按左右  兩邊都動
            {
                leftActive = true;
                rightActive = true;
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
                    trackSeq.Join(leftTrackPoint[i].DOLocalMove(leftPos[leftNext], trackSpeed).SetEase(Ease.Linear));
                    trackSeq.Join(leftTrackPoint[i].DOLocalRotateQuaternion(Quaternion.Euler(leftRot[leftNext]), trackSpeed).SetEase(Ease.Linear));

                }
            }

            if (rightActive)
            {
                //右履帶更新
                for (int i = 0; i < rightTracksPoint.Length; i++)
                {
                    int rightNext = GetNextIndex(i, rightTracksPoint.Length, rightDir); ;
                    trackSeq.Join(rightTracksPoint[i].DOLocalMove(rightPos[rightNext], trackSpeed).SetEase(Ease.Linear));
                    trackSeq.Join(rightTracksPoint[i].DOLocalRotateQuaternion(Quaternion.Euler(rightRot[rightNext]), trackSpeed).SetEase(Ease.Linear));
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
}
