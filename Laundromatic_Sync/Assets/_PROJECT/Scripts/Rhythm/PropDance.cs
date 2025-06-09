using UnityEngine;
using DG.Tweening;

public class PropDance : MonoBehaviour
{
    public float bpm = 120f;
    public float moveDistance = 1f;

    private float beatDuration;

    void Start()
    {
        beatDuration = 60f / bpm;

        foreach (Transform child in transform)
        {
            // Animate each child (e.g., move up/down)
            child.DOMoveY(child.position.y + moveDistance, beatDuration / 2)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}
