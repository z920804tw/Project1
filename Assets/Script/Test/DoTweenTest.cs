using DG.Tweening;
using UnityEngine;

public class DoTweenTest : MonoBehaviour
{
    Tween moveTween;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MoveObject()
    {
        if (moveTween == null)
        {
            moveTween = transform.DOMove(new Vector3(transform.position.x + 7, transform.position.y, transform.position.z), 5).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            Debug.Log(123);
        }
        else
        {
            if (moveTween.IsPlaying())
            {
                moveTween.Pause();
            }
            else
            {
                moveTween.Play();
            }
        }
    }
}
