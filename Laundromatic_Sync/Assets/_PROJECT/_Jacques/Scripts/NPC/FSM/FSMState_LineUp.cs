using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class FSMState_LineUp : FSMState
{
    //Go to open position in line in order
    //Change state to place order when at front of the line
    //Change to navmesh later
    [SerializeField] GameObject npcGameObject;

    [SerializeField] bool isInPosition;
    private bool movingHasEnded;
    private bool hasBasket = true;
    [SerializeField] int currentPositionIndex;
    
    
    NpcManager npcManager;

    public FSMState_LineUp(FSMBehaviour fsmBehaviour, NpcManager npcManager) : base(fsmBehaviour)
    {
        this.npcManager = npcManager;
    }

    public override void Enable()
    {
        base.Enable();
        npcGameObject = fsmBehaviour.gameObject;
        //LineUp();
        GoToLinePosition();
    }

    #region OLD
    
    
    public void FrontLinePosition()
    {
        //check if is in the first position
        //move basket to thingy
        if(!movingHasEnded || !hasBasket) return;
        if (fsmBehaviour.gameObject.transform.GetChild(0) != null)
        {
            Transform basketObject = fsmBehaviour.gameObject.transform.GetChild(0);
            basketObject.transform.DOMove(npcManager.basketPlacePosition.position, 0.2f).OnComplete(() => FrontLineEnd(basketObject));
            
            
        }
        if (fsmBehaviour.isInPosition && fsmBehaviour.currentPositionIndex == 0)
        {
            Debug.Log("State Stale");
            stateStale = true;
            fsmBehaviour.isInPosition = false;
        }
    
        void FrontLineEnd(Transform basketObject)
        {
            basketObject.SetParent(npcManager.basketPlacePosition);
            npcManager.isLiningUpPositionFull[currentPositionIndex] = false;
            npcManager.tableHasBasket = true;
            hasBasket = false;
            stateStale = true;
        }
    }
    
    public void GoToLinePosition()
    {
        for (int i = 0; i < npcManager.npcLiningUpPositions.Capacity; i++)
        {
            if (npcManager.tableHasBasket) return;
            if (!npcManager.isLiningUpPositionFull[i])
            {
                npcGameObject.transform.DOMove(npcManager.npcLiningUpPositions[i].position, 1f).OnComplete(() => OnEndMove(i));
                break;
            }
        }
    
        int placeInLine;
        //
        // if (!npcManager.isLiningUpPositionFull[npcManager.npcLiningUpPositions.Capacity - 1])
        // {
        //     npcGameObject.transform.DOMove(npcManager.npcLiningUpPositions[npcManager.npcLiningUpPositions.Capacity - 1].position, 1f).OnComplete(() => OnEndMove(npcManager.npcLiningUpPositions.Capacity - 1));
        // }else if (!npcManager.isLiningUpPositionFull[npcManager.npcLiningUpPositions.Capacity - 2])
        // {
        //     npcGameObject.transform.DOMove(npcManager.npcLiningUpPositions[npcManager.npcLiningUpPositions.Capacity - 2].position, 1f).OnComplete(() => OnEndMove(npcManager.npcLiningUpPositions.Capacity - 2));
        // }else if (!npcManager.isLiningUpPositionFull[npcManager.npcLiningUpPositions.Capacity - 2])
        // {
        //     npcGameObject.transform.DOMove(npcManager.npcLiningUpPositions[npcManager.npcLiningUpPositions.Capacity - 3].position, 1f).OnComplete(() => OnEndMove(npcManager.npcLiningUpPositions.Capacity - 3));
        // }
        //
    }
    
    
    public void OnEndMove(int i)
    {
        movingHasEnded = true;
        npcManager.isLiningUpPositionFull[i] = true;
        currentPositionIndex = i;
        isInPosition = true;
        Debug.Log($"isInPosition is marked as {isInPosition}");
        Debug.Log(currentPositionIndex);
    }
    
    public void GoNextInLinePosition()
    {
        if (currentPositionIndex == 0 && movingHasEnded)
        {
            FrontLinePosition();
        }
        
        int nextPositionIndex = currentPositionIndex - 1;
        if (currentPositionIndex == 0)return;
        if (!npcManager.isLiningUpPositionFull[nextPositionIndex])
        {
            movingHasEnded = false;
            isInPosition = false;
            npcManager.isLiningUpPositionFull[currentPositionIndex] = false;
            npcGameObject.transform.DOMove(npcManager.npcLiningUpPositions[nextPositionIndex].position, 1f).OnComplete(GoNextEnd);
           
        }
    }
    
    void GoNextEnd()
    {
        movingHasEnded = true;
        isInPosition = true;
        currentPositionIndex--;
    }
    
    #endregion

    // void LineUp()
    // {
    //     for (int i = 0; i < 5; i++)
    //     {
    //         if (!npcManager.isLiningUpPositionFull[i])
    //         {
    //             int j = i;
    //             fsmBehaviour.transform.DOMove(npcManager.npcLiningUpPositions[i].position, 0.1f).OnComplete(() => npcManager.isLiningUpPositionFull[j] = true);
    //             if (i == 0)
    //             {
    //                 MoveBasket();
    //             }
    //         }
    //         
    //     }
    //     
    // }
    //
    // void MoveBasket()
    // {
    //     if (npcManager.tableHasBasket)return;
    //     fsmBehaviour.transform.GetChild(0).transform.DOMove(npcManager.basketPlacePosition.position, 0.1f)
    //         .OnComplete(() => npcManager.tableHasBasket = true);
    // }
    
    public override Type GetNextState()
    {
        //RETURN WHAT NEXT STATE NEEDS TO BE USED
        return !stateStale ? null : typeof(FSMState_Wait);
    }
    
    


    public override void Update()
    {
        if (isInPosition) GoNextInLinePosition();
        GetNextState();
    }
}
