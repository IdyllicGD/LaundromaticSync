using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetNextAvailableWaitingTransform", story: "[Self] get next position", category: "Action", id: "efaf3a0d5d5756ff32f930bcf5fedd2a")]
public partial class GetNextAvailableWaitingTransformAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> transformToChange;
    int currentCount;

    protected override Status OnStart()
    {
        if (currentCount >= NpcManager.instance.waitingPositions.Count)
        {
            currentCount = 0;
        }

        transformToChange.Value = NpcManager.instance.waitingPositions[currentCount];
        currentCount++;
        return Status.Success;
    }

}

