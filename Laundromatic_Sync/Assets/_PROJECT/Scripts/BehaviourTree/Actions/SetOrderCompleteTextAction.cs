using System;
using TMPro;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetOrderCompleteText", story: "Set dialogue [Text] to correct or wrong and [New]", category: "Action", id: "70b49c18f09f4f6099be2fd22138a984")]
public partial class SetOrderCompleteTextAction : Action
{
    [SerializeReference] public BlackboardVariable<TextMeshProUGUI> Text;
    [SerializeReference] public BlackboardVariable<NpcOrderData> New;
    [SerializeReference] public BlackboardVariable<DialogueSO> dialogueSo;
    [SerializeReference] public BlackboardVariable<bool> isCorrect;


    protected override Status OnStart()
    {
        NpcID iD = New.Value.npcID;
        if (isCorrect)
        {
            Text.Value.text = dialogueSo.Value.correctDialogue;
            NpcManager.instance.AddNpcBack(iD);
        }
        else
        {
            Text.Value.text = dialogueSo.Value.wrongDialogue;
            NpcManager.instance.AddNpcBack(iD);
        }
        return Status.Success;
        
    }


}

