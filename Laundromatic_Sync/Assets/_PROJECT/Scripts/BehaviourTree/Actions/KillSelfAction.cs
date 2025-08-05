using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "KillSelf", story: "Kill [Self]", category: "Action", id: "26a5b48dbe2cd102b61a2a505c7266ed")]
public partial class KillSelfAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    protected override Status OnStart()
    {
        NpcManager.instance.currentNpcs.Remove(Self.Value);
        return Status.Success;
    }


    protected override void OnEnd()
    {
        
        NpcManager.instance.KillNpc(Self.Value);
        
    }
}

