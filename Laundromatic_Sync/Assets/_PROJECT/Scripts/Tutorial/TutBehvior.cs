using UnityEngine;
using DG.Tweening;
using TMPro;

public class TutBehvior : MonoBehaviour
{
    public GameObject CurrTextGO;
    public GameObject CurrIconGO;
    public GameObject NextIconGO;
    public GameObject NextCollider;
    private void OnTriggerEnter(Collider other)
    {
        CurrTextGO.gameObject.SetActive(true);
        NextIconGO.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        CurrIconGO.gameObject.SetActive(false);
        CurrTextGO.gameObject.SetActive(false);
        NextCollider.gameObject.SetActive(true);
    }
    
}
