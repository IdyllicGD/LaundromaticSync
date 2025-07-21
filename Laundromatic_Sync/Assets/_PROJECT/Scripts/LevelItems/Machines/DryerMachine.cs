using System;
using System.Collections;
using System.Collections.Generic;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.Rhythm;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class DryerMachine : MonoBehaviour, IMachineInteractable, Iinteractable, RythmStages
{
    //When a basket is placed in, store what items have been used on it to set it again when the new one is spawned
    [SerializeField]ItemsUsed itemsUsed;
    CurrentStage currentStage;
    PlayerMovement currentPlayerMovement;
    public bool isBusy;
    [SerializeField] float machineTimeToComplete;
    [SerializeField] Transform raycastPosition;
    [SerializeField] Transform camPosition;
    [SerializeField] Transform camLookAtPosition;
    [SerializeField] Transform basketPosition;
    [SerializeField] ButtonSpawner buttonSpawner;
    [SerializeField] GameObject dryBasket;
    [SerializeField] StudioEventEmitter doneSound;
    [SerializeField] StudioEventEmitter busySound;
    [SerializeField] StudioEventEmitter buttonSound;


    void Update()
    {
        //Remembet to use this to set the idle bool to true when the machine is done and doesnt have a dry basket on top
    }
    bool HasDryBasket()
    {
        Debug.DrawRay(raycastPosition.position, Vector3.down, Color.blue, 1f);
        if (Physics.Raycast(raycastPosition.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.transform.TryGetComponent(out Basket basket))
            {
                return true;
            }
        }
        
        return false;
        
    }

    #region MachineInteractableFuncs
    public void Interact(Camera camObject)
    {
        //NOT USED
        throw new NotImplementedException();
    }

    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        //LIST:
        // - Check if the player is holding a clean basket
        // - Take clean basket and move it with the cam
        // - Start the rythm game
        // - On complete spawn a CleanDry basket
        // - If the original basket had fabric softner in it, mark that on the Clean Dry basket when adding it.
        //Go In focus mode

        //Check if the item is a basket
        if (HasDryBasket()) return;
        if (ReturnBasketType(secondaryObject) != BasketType.WetClean) return;
        itemsUsed = secondaryObject.GetComponent<Basket>().ItemsUsed;
        PlayerInteraction playerInteraction = camObject.transform.parent.GetComponentInParent<PlayerInteraction>();
        currentPlayerMovement = camObject.transform.parent.GetComponentInParent<PlayerMovement>();

        if (currentStage == CurrentStage.FillingUpStage)
        {
            currentStage = CurrentStage.DryingStage;
            currentPlayerMovement.cameraOriginalTransform = Camera.main.transform.position;
            playerInteraction.StopPlayerMovement();
            camObject.transform.DOMove(camPosition.position, 0.5f);
            camObject.transform.DODynamicLookAt(camLookAtPosition.position, 0.5f);
            secondaryObject.transform.SetParent(basketPosition);
            secondaryObject.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() => StartStage(currentStage));
        }



    }

        void StartStage(CurrentStage stage)
    {
        switch (stage)
        {
            case CurrentStage.FillingUpStage:
                break;
            case CurrentStage.DryingStage:
                StartButtonSpawning();
                break;
            case CurrentStage.WashingStage:
                break;
            case CurrentStage.FinishedStage:
                //exit to normal view
                StartCoroutine(EndMachineFunc(machineTimeToComplete));
                buttonSound.Play();
                busySound.Play();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
        }
    }

        void StartButtonSpawning()
    {
        buttonSpawner.currentStage = 0;
        buttonSpawner.SpawnButton();

    }

    IEnumerator EndMachineFunc(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        currentStage = CurrentStage.FillingUpStage;
        GameObject _basket = Instantiate(dryBasket, basketPosition);
        _basket.GetComponent<Basket>().ItemsUsed = itemsUsed;
        busySound.Stop();
        doneSound.Play();

    }

    public void Interact()
    {
        throw new NotImplementedException();
    }

    public bool IsBusy()
    {
        return isBusy;
    }

    public MachineType MachineType()
    {
        throw new NotImplementedException();
    }

    #endregion

    public void StageCheck()
    {
        if (buttonSpawner.currentStage == buttonSpawner.spawnPoints.Count)
        {
            currentStage = 0;
            currentStage = CurrentStage.FinishedStage;
            StartStage(currentStage);
            currentPlayerMovement.ResetCamera();
            Destroy(transform.GetChild(0).GetChild(0).gameObject);
            //Exit focus mode
            return;
        }
        buttonSpawner.SpawnButton();
        //Play current animation where needed
    }
    
    BasketType ReturnBasketType(GameObject basket)
    {
        return !basket.TryGetComponent(out Basket basketScript)
            ? BasketType.None
            : basketScript.ReturnBasketType();
    }

}
