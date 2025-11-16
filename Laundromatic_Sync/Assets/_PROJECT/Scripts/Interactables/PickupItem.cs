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
    //public MachineUsableItemsType machineUsableItemsType;

    private void Awake()
    {
        //machineUsableItemsType = MachineUsableItemsType.None;
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

        //     collider.enabled = false;
        //     rigidbody.useGravity = false;
        //     rigidbody.isKinematic = true;
        // if (!hasObjectInHand)
        // {
        //     //Normal Pickup
            
        //     transform.SetParent(placePosition);
        //     transform.DOLocalMove(Vector3.zero, 0f);
        //     transform.DOLocalRotate(new Vector3(0f,-137.66f,0f), 0.2f).OnComplete(() => rigidbody.constraints = RigidbodyConstraints.FreezeRotation);
        //     rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        // }
        // else
        // {
            
        //     Transform oldObject = placePosition.GetChild(0);
        //     ///Move old one to new ones stack position
        //     ///New one now is at the bottom

        //     ///Move stack parent to the next items stack position
        //     ///Move Object to hand
        //     transform.SetParent(placePosition);
        //     transform.DOLocalMove(Vector3.zero, 0f);
        //     transform.DOLocalRotate(new Vector3(0f,-137.66f,0f), 0.2f).OnComplete(() => rigidbody.constraints = RigidbodyConstraints.FreezeRotation);
        //     rigidbody.constraints = RigidbodyConstraints.FreezePosition;

        //     stackParent.SetParent(stackPosition);
        //     stackParent.DOLocalMove(Vector3.zero, 0f).OnComplete(() => stackParent.SetParent(placePosition.parent));
        //     oldObject.SetParent(stackParent);
        //     if(stackParent.childCount < 0)
        //     {
        //         for (int i = 0; i < stackParent.childCount; i++)
        //         {
        //             int nextIndex = i + 1;
        //             if(nextIndex >= stackParent.childCount) return;
        //             Transform nextMovePos = stackParent.GetChild(i).GetComponent<PickupItem>().stackPosition;
        //             nextMovePos.DOMove(stackParent.GetChild(nextIndex).GetComponent<PickupItem>().stackPosition.position, 0f);
        //         }
                
        //     }
        //     else
        //     {
        //         oldObject.DOLocalMove(Vector3.zero, 0f);
        //     }
            
            
            // else
            // {
            //     Transform newStackPosition = 
            // }



        //}


        // if (hasObjectInHand)
        // {
        //     placePosition.GetChild(placePosition.childCount)
        // }
        
        
    }

    
}


