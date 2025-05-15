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
    private int currentPositionIndex;
    
    public FSMState_LineUp(FSMBehaviour fsmBehaviour) : base(fsmBehaviour) { }

    public override void Enable()
    {
        base.Enable();
        GoToLinePosition();
    }

    public void FrontLinePosition()
    {
        //check if is in the first position
        //move basket to thingy
        if (fsmBehaviour.gameObject.transform.GetChild(0) != null)
        {
            Transform basketObject = fsmBehaviour.gameObject.transform.GetChild(0);
            basketObject.transform.DOMove(NpcManager.instance.basketPlacePosition.position, 0.2f);
            basketObject.SetParent(NpcManager.instance.basketPlacePosition);
        }
        if (isInPosition && currentPositionIndex == 0)
        {
            stateStale = true;
            isInPosition = false;
        }
    }

    public void GoToLinePosition()
    {
        for (int i = 0; i < NpcManager.instance.npcLiningUpPositions.Capacity; i++)
        {
            if (!NpcManager.instance.isLiningUpPositionFull[i])
            {
                npcGameObject.transform.DOMove(NpcManager.instance.npcLiningUpPositions[i].position, 1f).OnComplete(() => isInPosition = true).OnComplete(() => currentPositionIndex = i);
                NpcManager.instance.isLiningUpPositionFull[i] = true;
            }
        }
        
    }

    public void GoNextInLinePosition()
    {
        if (currentPositionIndex == 0)
        {
            FrontLinePosition();
        }
        int nextPositionIndex = currentPositionIndex - 1;
        if (!NpcManager.instance.isLiningUpPositionFull[nextPositionIndex])
        {
            isInPosition = false;
            NpcManager.instance.isLiningUpPositionFull[currentPositionIndex] = false;
            npcGameObject.transform.DOMove(NpcManager.instance.npcLiningUpPositions[nextPositionIndex].position, 1f).OnComplete(() => isInPosition =  true);
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
