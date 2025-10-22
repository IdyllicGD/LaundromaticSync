using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Reset Canvas text", story: "Reset [Canvas] text", category: "Action", id: "7082f3b4b9adc1757b7b0d8f623c29d6")]
public partial class ResetCanvasTextAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Canvas;

    protected override Status OnStart()
    {
        ResetUiLayout resetThisUi = Canvas.Value.GetComponent<ResetUiLayout>();
        resetThisUi.ResetUi();
        return Status.Running;
    }

}

