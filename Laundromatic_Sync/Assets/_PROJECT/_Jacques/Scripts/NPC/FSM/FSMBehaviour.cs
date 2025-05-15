using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBehaviour : MonoBehaviour
{
    [SerializeReference] FSMState currentState;
    [SerializeField] private FSMState_LineUp stateLineUp;
    [SerializeField] private FSMState_Wait stateWait;
    [SerializeField] private FSMState_Leave stateLeave;
    
    public bool isInPosition;
    public int currentPositionIndex;
    
    NpcManager npcManager;
    
    Dictionary<Type, FSMState> typeStateLookup = new Dictionary<Type, FSMState>();

    private void Start()
    {
        npcManager = FindFirstObjectByType<NpcManager>();
        if (npcManager != null)
        {
            Debug.Log("Man is manning");
        }
        stateLineUp = new FSMState_LineUp(this, npcManager);
        stateWait = new FSMState_Wait(this, npcManager);
        
        typeStateLookup[typeof(FSMState_LineUp)] = stateLineUp;
        typeStateLookup[typeof(FSMState_Wait)] = stateWait;
        
        SetState(stateLineUp);
    }

    void SetState(FSMState state)
    {
        currentState?.Disable();
        currentState = state;
        currentState.Enable();
    }

    void CheckStaleState()
    {
        if (currentState == null) return;
        if (currentState.StateStale)
        {
            SetState(typeStateLookup[currentState.GetNextState()]);
        }
    }


    private void Update()
    {
        currentState?.Update();
        CheckStaleState();
    }
    
}
