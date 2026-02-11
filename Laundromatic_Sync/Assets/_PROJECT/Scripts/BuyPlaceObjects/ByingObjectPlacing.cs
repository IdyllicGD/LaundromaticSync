using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ByingObjectPlacing : MonoBehaviour
{
    public Material transparentMat;
    public Vector3 objectSize;
    GameObject colliderRef; //this will have a trigger on it
    [SerializeField] PlayerInteraction playerInteraction;
    [SerializeField] Color greenTrans;
    [SerializeField] Color redTrans;
    [SerializeField] LayerMask allLayers;
    [SerializeField] LayerMask floor;
    public GameObject objectToPlace;
    public bool placingObjects;

    void Update()
    {
        
        if (placingObjects)
        {
            CheckIfCanBePlaced();
            colliderRef.SetActive(true);
        }
        else
        {
            colliderRef.SetActive(false);
        }

    }

    void CheckIfCanBePlaced()
    {
        if (!placingObjects) return;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, playerInteraction.raycastDistance, allLayers))
        {
            
            colliderRef.transform.position = hit.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != floor)
        {
            transparentMat.color = greenTrans;
        }
        else
        {
            transparentMat.color = redTrans;
        }
    }




    void ChangeColour(Color color)
    {
        transparentMat.color = color;
    }

    public void PlaceObject()
    {
        placingObjects = false;
    }
}
