using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToWaitingArea", story: "[Self] move to [WaitingArea] at [index] of the list", category: "Action", id: "f73e499f37aec75dec86f4078d07f59b")]
public partial class MoveToWaitingAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> WaitingArea;
    [SerializeReference] public BlackboardVariable<int> Index;
    List<Transform> waitingTransforms;

    protected override Status OnStart()
    {
        waitingTransforms = NpcManager.instance.waitingPositions;
        
        return Status.Success;
    }

}

