using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Animations;
using DG.Tweening;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LookAtPlayer", story: "[Self] look at [Player] with [Bounce]", category: "Action", id: "c32cc2428bce40a3be5d0e11cea39e9d")]
public partial class LookAtPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Player;
    [SerializeReference] public BlackboardVariable<float> Bounce;
    protected override Status OnStart()
    {
        Player.Value = NpcManager.instance.playerTransform;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Self.Value.transform.DODynamicLookAt(Player.Value.position, Bounce.Value, AxisConstraint.Y);
        return Status.Success;
    }


}

