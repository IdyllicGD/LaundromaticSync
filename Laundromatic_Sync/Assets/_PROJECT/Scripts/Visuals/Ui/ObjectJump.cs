using DG.Tweening;
using DG.Tweening.Core.Easing;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ObjectJump : MonoBehaviour
{
    [Title("Hover Settings", null, TitleAlignments.Centered)]
    [SerializeField] float jumpDistance;
    [SerializeField] float jumpDuration;
    [SerializeField] float amountToSundevide;
    [Title("Animation Settings", null, TitleAlignments.Centered)]
    [SerializeField] Ease preMadeEase;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] bool usePremadeEase;
    void Start()
    {
        if (usePremadeEase)
        {
            transform.DOLocalMoveY(transform.position.y + jumpDistance, jumpDuration / amountToSundevide).SetLoops(-1, LoopType.Yoyo).SetEase(preMadeEase);
        }
        else
        {
            transform.DOLocalMoveY(transform.position.y + jumpDistance, jumpDuration / amountToSundevide).SetLoops(-1, LoopType.Yoyo).SetEase(animationCurve);
        }
        
    }
}
