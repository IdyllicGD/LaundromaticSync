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

    private bool isInPosition;
    private bool movingHasEnded;
    private bool hasBasket = true;
    private int currentPositionIndex;
    
    
    NpcManager npcManager;

    public FSMState_LineUp(FSMBehaviour fsmBehaviour, NpcManager npcManager) : base(fsmBehaviour)
    {
        this.npcManager = npcManager;
    }

    public override void Enable()
    {
        base.Enable();
        npcGameObject = fsmBehaviour.gameObject;
        GoToLinePosition();
        
    }



    public void FrontLinePosition()
    {
        //check if is in the first position
        //move basket to thingy
        if(!movingHasEnded || !hasBasket) return;
        if (fsmBehaviour.gameObject.transform.GetChild(0) != null)
        {
            Transform basketObject = fsmBehaviour.gameObject.transform.GetChild(0);
            basketObject.transform.DOMove(npcManager.basketPlacePosition.position, 0.2f).OnComplete(() => basketObject.SetParent(npcManager.basketPlacePosition));
            hasBasket = false;
        }
        if (fsmBehaviour.isInPosition && fsmBehaviour.currentPositionIndex == 0)
        {
            stateStale = true;
            fsmBehaviour.isInPosition = false;
        }
    }

    public void GoToLinePosition()
    {
        for (int i = 0; i < npcManager.npcLiningUpPositions.Capacity; i++)
        {
            if (!npcManager.isLiningUpPositionFull[i])
            {
                npcGameObject.transform.DOMove(npcManager.npcLiningUpPositions[i].position, 1f).OnComplete(() => movingHasEnded = true);
                npcManager.isLiningUpPositionFull[i] = true;
                currentPositionIndex = i;
                isInPosition = true;
                Debug.Log($"isInPosition is marked as {isInPosition}");
                Debug.Log(currentPositionIndex);
                return;
            }
        }
        
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
            npcGameObject.transform.DOMove(npcManager.npcLiningUpPositions[nextPositionIndex].position, 1f).OnComplete(() => movingHasEnded = true);
            isInPosition = true;
            currentPositionIndex--;
        }
    }
    
    public override Type GetNextState()
    {
        //RETURN WHAT NEXT STATE NEEDS TO BE USED
        return !stateStale ? null : typeof(FSMState_Wait);
    }
    
    


    public override void Update()
    {
        if (isInPosition) GoNextInLinePosition();
    }
}
