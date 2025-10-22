using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TalkingSound", story: "Play talking sound from [This]", category: "Action", id: "1e7f6dfb62a363e576bfba89fb724160")]
public partial class TalkingSoundAction : Action
{
    [SerializeReference] public BlackboardVariable<NpcSoundManager> This;

    protected override Status OnStart()
    {
        This.Value.PlaySound();
        return Status.Running;
    }


    protected override void OnEnd()
    {
        
    }
}

