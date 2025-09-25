using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetInfoInManagerKaren", story: "Set Info in manager to [Bool]", category: "Action", id: "c0bca7f423fc17f11ffd593626220a86")]
public partial class SetInfoInManagerKarenAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> Bool;

    protected override Status OnStart()
    {
        if (Bool)
        {
            DecisionManager.instance.helpedKaren = true;
        }
        DecisionManager.instance.talkedToKaren = true;
        return Status.Running;
    }

}

