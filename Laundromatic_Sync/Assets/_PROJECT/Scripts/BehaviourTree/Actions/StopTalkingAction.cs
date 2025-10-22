using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopTalking", story: "Stop playing sound from [SoundManager]", category: "Action", id: "43789f27db02fc0c6dc2cbb222fadeea")]
public partial class StopTalkingAction : Action
{
    [SerializeReference] public BlackboardVariable<NpcSoundManager> SoundManager;

    protected override Status OnStart()
    {
        SoundManager.Value.StopSound();
        return Status.Running;
    }



    protected override void OnEnd()
    {
    }
}

