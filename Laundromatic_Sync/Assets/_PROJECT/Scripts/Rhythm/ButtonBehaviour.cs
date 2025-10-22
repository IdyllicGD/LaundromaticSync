using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class ButtonBehaviour : MonoBehaviour
{
    public float bps = 5f;
    public float endScaleValue;
void Awake()
    {
        RectTransform ButtonRect = GetComponent<RectTransform>();
        ButtonRect.DOScale(endScaleValue, bps)
            .SetEase(Ease.InOutBack)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }
}
