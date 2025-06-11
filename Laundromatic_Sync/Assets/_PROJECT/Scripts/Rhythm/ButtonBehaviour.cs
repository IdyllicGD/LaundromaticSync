using DG.Tweening;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{

void Awake()
    {
        RectTransform ButtonRect = GetComponent<RectTransform>();
        ButtonRect.DOScale(1.04f, 2f)
            .SetEase(Ease.InOutBack)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }
}
