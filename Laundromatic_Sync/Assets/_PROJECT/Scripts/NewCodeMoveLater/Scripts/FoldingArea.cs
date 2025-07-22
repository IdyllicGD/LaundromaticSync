using System;
using _PROJECT._Jacques.Scripts.Interfaces;
using _PROJECT.Scripts.Rhythm;
using _PROJECT.Scripts.temp.Scripts;
using DG.Tweening;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class FoldingArea : MonoBehaviour, IMachineInteractable, Iinteractable, RythmStages
{

    #region Vars

    private bool isBusy;
    private PlayerMovement currentPlayerMovement;
    [Title("Gameplay Vars", null, TitleAlignments.Centered)]
    [SerializeField][ReadOnly] int clothingAmount;
    [SerializeField][ReadOnly] int stagesRun = 0;
    
    [Title("Outside Refrences", null, TitleAlignments.Centered, bold: true)]
    [SerializeField] ButtonSpawner buttonSpawner;
    [Title("Sound Refrences", null, TitleAlignments.Left, false, bold: true)]
    [SerializeField] StudioEventEmitter foldingSound;
    [SerializeField] StudioEventEmitter buttonSound;

    [Title("Types", null, TitleAlignments.Centered, bold: true)]
    [SerializeField] MachineType machineType;
    [SerializeField] ItemsUsed itemsUsed = ItemsUsed.None;
    [Title("Prefabs", null, TitleAlignments.Centered, bold: true)]
    [SerializeField] GameObject foldedBasketPrefab;

    #region Positions
    [Title("Positions", null, TitleAlignments.Centered, bold: true)]
    [SerializeField] Transform basketNotFoldedPosition;
    [SerializeField] Transform basketFoldedPosition;
    [SerializeField] Transform camPosition;
    [SerializeField] Transform lookAtPosition;
    [SerializeField] Transform foldingPosition;


    #endregion
    [Title("LayerMask", null, TitleAlignments.Centered, bold: true)]
    [SerializeField] LayerMask layerMask;
    #endregion

    #region Interactions

    public void Interact()
    {

    }
    public void Interact(Camera camObject)
    {

    }

    #region Interaction with Machine
    public void Interact(Camera camObject, GameObject secondaryObject)
    {
        //Check if the basket is WetClean
        if (ReturnBasketType(secondaryObject) != BasketType.DryClean) return;
        if (basketFoldedPosition.childCount != 0) return;
        //Get a ref to the basket script
        Basket basket = secondaryObject.GetComponent<Basket>();
        //Set items used on that basket
        itemsUsed = basket.ItemsUsed;
        //Get player interaction and player movement Script
        PlayerInteraction playerInteraction = camObject.transform.parent.GetComponentInParent<PlayerInteraction>();
        currentPlayerMovement = camObject.transform.parent.GetComponentInParent<PlayerMovement>();
        clothingAmount = basket.amountOfClothing;

        currentPlayerMovement.cameraOriginalTransform = Camera.main.transform.position;
        if (!Physics.Raycast(camObject.transform.position, Vector3.forward, out RaycastHit hit, Mathf.Infinity, layerMask)) Debug.Log("[FoldingArea] raycast couldnt hit a position");
        currentPlayerMovement.cameraLookAtPosition = hit.point;

        playerInteraction.StopPlayerMovement();
        camObject.transform.DOMove(camPosition.position, 0.5f);
        camObject.transform.DODynamicLookAt(lookAtPosition.position, 0.5f);
        secondaryObject.transform.SetParent(basketNotFoldedPosition);
        secondaryObject.transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() => StartButtons());



    }
    #endregion

    #endregion

    void StartButtons()
    {
        buttonSpawner.currentStage = 0;
        buttonSpawner.SpawnButton();
        
    }

    void EndMachineActions()
    {
        stagesRun = 0;
        GameObject _finalBasket = Instantiate(foldedBasketPrefab, basketFoldedPosition);
        _finalBasket.GetComponent<Basket>().ItemsUsed = itemsUsed;
        currentPlayerMovement.ResetCamera();
        Destroy(basketNotFoldedPosition.GetChild(0).gameObject);

    }

    public bool IsBusy()
    {
        return isBusy;
    }

    public MachineType MachineType()
    {
        return machineType;
    }

    public void StageCheck()
    {
        if (buttonSpawner.currentStage == buttonSpawner.spawnPoints.Count)
        {
            stagesRun++;
            StartButtons();
            buttonSound.Play();
            return;
        }
        if (stagesRun >= clothingAmount) { EndMachineActions(); buttonSound.Play(); return; }
        buttonSpawner.SpawnButton();
        Debug.Log($"[FoldingArea] Spawn a peace of clothing");
    }
    
    BasketType ReturnBasketType(GameObject basket)
    {
        return !basket.TryGetComponent(out Basket basketScript)
            ? BasketType.None
            : basketScript.ReturnBasketType();
    }
}
