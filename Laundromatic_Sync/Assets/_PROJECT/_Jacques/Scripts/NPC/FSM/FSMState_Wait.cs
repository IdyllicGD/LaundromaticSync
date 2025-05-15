using System;
using DG.Tweening;
using UnityEngine;

public class FSMState_Wait : FSMState
{
    //Go to an open position to wait
    //When an order is completed they should leave and make space for a new npc
    //Change to navmesh later
    [SerializeField] GameObject npcGameObject;
    private bool isInPosition;
    private int currentPositionIndex;
    NpcManager npcManager;

    public FSMState_Wait(FSMBehaviour fsmBehaviour, NpcManager npcManager) : base(fsmBehaviour)
    {
        this.npcManager = npcManager;
    }

    public override void Enable()
    {
        base.Enable();
        GoToWaitPosition();
    }

    void GoToWaitPosition()
    {
        for (int i = 0; i < NpcManager.instance.npcSittingPositions.Capacity; i++)
        {
            if (!NpcManager.instance.isSittingPositionFull[i])
            {
                npcGameObject.transform.DOMove(npcManager.npcSittingPositions[i].position, 0.1f).OnComplete(() => isInPosition = true).OnComplete(()=> currentPositionIndex = i);
                NpcManager.instance.isSittingPositionFull[i] = true;
            }
        }
    }

    public override Type GetNextState()
    {
        return !stateStale ? null : typeof(FSMState_Leave);
    }

    public override void Update()
    {
        
    }
}
