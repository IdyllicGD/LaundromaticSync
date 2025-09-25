using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Manager Info", story: "Set Manager Info to [Bool] for Lifegaurd", category: "Action", id: "43aeeb0c326030fabced6520e1e9953e")]
public partial class SetManagerInfoAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Bool;

    protected override Status OnStart()
    {
        if (Bool)
        {
            DecisionManager.instance.helpedLifegaurd = true;
        }
        DecisionManager.instance.talkedToLifeGaurd = true;
        return Status.Running;
    }

}

