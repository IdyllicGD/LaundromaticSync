using DG.Tweening;
using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class ButtonBehaviour : MonoBehaviour
{
    private float bps;
void Awake()
    {
        RectTransform ButtonRect = GetComponent<RectTransform>();
        ButtonRect.DOScale(1.04f, bps)
            .SetEase(Ease.InOutBack)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }
}
