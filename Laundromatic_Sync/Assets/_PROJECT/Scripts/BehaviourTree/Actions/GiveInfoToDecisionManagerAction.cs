using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Give Info to DecisionManager", story: "Set Info in Manager from [Bool]", category: "Action", id: "5371dd680f8662f69da0d47668a82dc4")]
public partial class GiveInfoToDecisionManagerAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Bool;

    protected override Status OnStart()
    {
        if (Bool)
        {
            DecisionManager.instance.helpedCop = true;
        }
        DecisionManager.instance.talkedToCop = true;
        return Status.Running;
    }

}

