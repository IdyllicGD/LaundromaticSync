using UnityEngine;
using DG.Tweening;

public class ObjectHover : MonoBehaviour
{
    public float height = 1;
    public float speed = 1;

    public void Awake()
    {
        transform.DOLocalMoveY(.5f*height,1f*speed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }
}
