using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeaveStore", story: "[Self] go [Outside]", category: "Action", id: "64c52e1bc5cbe799a851c6d4792e836c")]
public partial class LeaveStoreAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Outside;

    protected override Status OnStart()
    {
        return Status.Success;
    }

}

