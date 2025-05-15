using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBehaviour : MonoBehaviour
{
    [SerializeReference] FSMState currentState;
    [SerializeField] private FSMState_LineUp stateLineUp;
    [SerializeField] private FSMState_Wait stateWait;
    
    Dictionary<Type, FSMState> typeStateLookup = new Dictionary<Type, FSMState>();

    private void Initialize()
    {
        
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
