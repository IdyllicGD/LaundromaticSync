using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SignInteraction : MonoBehaviour, Iinteractable
{
    public TextMeshProUGUI textMesh;
    private bool open = false;
    public void Interact()
    {
        //transform.DOLocalRotate(new Vector3(0f,transform.localRotation.y + 180f,0f), 2f).WaitForCompletion();
        if (!open)
        {
            textMesh.text = "Open";
            open = true;
        }else if (open)
        {
            textMesh.text = "Close";
            open = false;
        }
        if (BasketSpawner.instance.canSpawnBaskets)
        {
            BasketSpawner.instance.canSpawnBaskets = false;
        }else if (!BasketSpawner.instance.canSpawnBaskets)
        {
            BasketSpawner.instance.canSpawnBaskets = true;
        }
        
    }
}
