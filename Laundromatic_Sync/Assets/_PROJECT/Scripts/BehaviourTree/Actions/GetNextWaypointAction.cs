using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetNextWaypoint", story: "[Self] Get Next [Waypoint]", category: "Action", id: "5dbddeeaa8d797268853d1da530bb0a0")]
public partial class GetNextWaypointAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> TargetWaypoint;

    protected override Status OnStart()
    {
        Transform _nextWaypoint = NpcManager.instance.GetNextWaypoint(Self.Value.transform);
        if (_nextWaypoint == null) return Status.Failure;

        TargetWaypoint.Value = _nextWaypoint;

        return Status.Success;
        
    }


}

