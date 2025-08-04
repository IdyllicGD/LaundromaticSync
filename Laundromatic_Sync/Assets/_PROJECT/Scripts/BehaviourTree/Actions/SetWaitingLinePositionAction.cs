using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetWaitingLinePosition", story: "Set [WaitingLinePosition] to Current Waiting Line Position", category: "Action", id: "bc1dc0fdc664fe4b03e48ab111e42877")]
public partial class SetWaitingLinePositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> WaitingLinePosition;

    protected override Status OnStart()
    {
        WaitingLinePosition.Value = NpcManager.instance.startLineUpPosition;
        if (WaitingLinePosition.Value == null)
        {
            return Status.Failure;
        }
        return Status.Success;
    }

}

