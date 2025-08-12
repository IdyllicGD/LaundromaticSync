using UnityEngine;
using DG.Tweening;

public class CurtainBehaviour : MonoBehaviour
{
    public Transform Curtain01;
    public Transform Curtain02;

    private Vector3 curtain01StartPos;
    [SerializeField] Vector3 curtain01EndPos;
    private Vector3 curtain02StartPos;
    [SerializeField] Vector3 curtain02EndPos;

    private void Start()
    {
        curtain01StartPos = Curtain01.position;
        curtain02StartPos = Curtain02.position;
    }

    private void Awake()
    {
        DOTween.Init();
    }

    public void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("Player Enter");
        // if (other.CompareTag("Player"))
        // {
            Curtain01.DOMove(curtain01StartPos + curtain01EndPos, 1f).SetEase(Ease.InOutSine);
            Curtain02.DOMove(curtain02StartPos + curtain02EndPos, 1f).SetEase(Ease.InOutSine);
        //}
    }

    public void OnTriggerExit(Collider other)
    {
        UnityEngine.Debug.Log("Player Exit");
        // if (other.CompareTag("Player"))
        // {
            Curtain01.DOMove(curtain01StartPos, 1f).SetEase(Ease.InOutSine);
            Curtain02.DOMove(curtain02StartPos, 1f).SetEase(Ease.InOutSine);
        // }
    }
}
