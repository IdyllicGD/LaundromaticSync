using System;
using UnityEngine;

[System.Serializable]
public abstract class FSMState
{
    protected bool stateEnabled = false;
    public bool StateEnabled => stateEnabled;
    protected bool stateStale = false;
    [SerializeField]public bool StateStale => stateStale;
    [SerializeField]protected FSMBehaviour fsmBehaviour;
    public FSMBehaviour FSMBehaviour => fsmBehaviour;

    protected FSMState(FSMBehaviour fsmBehaviour)
    {
        this.fsmBehaviour = fsmBehaviour;
    }

    public virtual void Enable()
    {
        stateEnabled = true;
        Debug.Log($"[{GetType()}] State Enabled");
    }

    public virtual void Disable()
    {
        stateEnabled = false;
        stateStale = false;
        Debug.Log($"[{GetType()}] State Disabled");
    }

    public virtual Type GetNextState()
    {
        return typeof(FSMState);
    }
    
    public abstract void Update();
}
