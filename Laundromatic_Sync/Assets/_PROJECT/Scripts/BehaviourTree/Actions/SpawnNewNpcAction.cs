using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Spawn New Npc", story: "Spawn New Npc", category: "Action", id: "f2b4a396ed47ab7d970cb5941fe2ed0a")]
public partial class SpawnNewNpcAction : Action
{

    protected override Status OnStart()
    {
        NpcManager.instance.SpawnNpc();
        return Status.Success;
    }

}

