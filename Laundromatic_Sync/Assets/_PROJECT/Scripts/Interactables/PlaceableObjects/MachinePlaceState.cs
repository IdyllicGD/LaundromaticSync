using System.Collections.Generic;
using UnityEngine;

public class MachinePlaceState : MonoBehaviour
{
    [SerializeField] List<MeshRenderer> materialsFromMeshRenderer = new();
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;
    [SerializeField] LayerMask badLayers;

    bool canBePlaced;
    public bool isTryingToPlaceObject;

    // void Update()
    // {
    //     if(!isTryingToPlaceObject) return;
    //     CheckIfObjectCanBePlaced();
    // }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == badLayers)
        {
            Reeeed();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == badLayers)
        {
            Greeen();
        }
    }

    void Greeen()
    {
        for (int i = 0; i < materialsFromMeshRenderer.Count; i++)
        {
            materialsFromMeshRenderer[i].materials[i] = greenMat;
        }
    }

    void Reeeed()
    {
        for (int i = 0; i < materialsFromMeshRenderer.Count; i++)
        {
            materialsFromMeshRenderer[i].materials[i] = redMat;
        }
    }
}
