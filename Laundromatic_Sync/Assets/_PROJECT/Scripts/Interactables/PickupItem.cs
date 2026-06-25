using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Rigidbody))]
public class PickupItem : MonoBehaviour, IPickupable
{
    public Transform stackPosition;
    Collider collider;
    Rigidbody rigidbody;
    public CleanOrDirty cleanOrDirty;
    public TypeOfMachine machineTypefApplicable;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    public void Pickup(Transform placePosition, float moveAmount,bool hasObjectInHand)
    {
        #region Old Code

        collider.enabled = false;
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        if(placePosition.childCount > 0)
        {
            for (int i = 0; i < placePosition.childCount; i++)
            {
                
                    Transform currentChild = placePosition.GetChild(i);
                    currentChild.DOLocalMove(new Vector3(0, currentChild.localPosition.y + moveAmount, 0), 0.2f);
                
                Debug.Log("Tehe");
            }
        }
        transform.SetParent(placePosition);
        transform.DOLocalMove(Vector3.zero, 0.2f);

        transform.DOLocalRotate(new Vector3(0f,-137.66f,0f), 0.2f).OnComplete(() => rigidbody.constraints = RigidbodyConstraints.FreezeRotation);
        rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        #endregion
    }

    
}




