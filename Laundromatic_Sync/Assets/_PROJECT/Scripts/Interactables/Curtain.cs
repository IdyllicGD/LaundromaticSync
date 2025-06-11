using UnityEngine;
using DG.Tweening;

public class CurtainBehaviour : MonoBehaviour
{
    public Transform Curtain01;
    public Transform Curtain02;

    private Vector3 curtain01StartPos;
    private Vector3 curtain02StartPos;

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
        if (other.CompareTag("Player"))
        {
            Curtain01.DOMove(curtain01StartPos + new Vector3(0.8f, 0, 0), 1f).SetEase(Ease.InOutSine);
            Curtain02.DOMove(curtain02StartPos + new Vector3(-0.8f, 0, 0), 1f).SetEase(Ease.InOutSine);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        UnityEngine.Debug.Log("Player Exit");
        if (other.CompareTag("Player"))
        {
            Curtain01.DOMove(curtain01StartPos, 1f).SetEase(Ease.InOutSine);
            Curtain02.DOMove(curtain02StartPos, 1f).SetEase(Ease.InOutSine);
        }
    }
}
