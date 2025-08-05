using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using TMPro;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ShowCurrentDialogue", story: "Show next [Dialogue] at [this] int and set [DialogueText] and Enable [Canvas] and set [isDoneTalking] to true", category: "Action", id: "91f998e4a039e4258a01e2bd1072e64e")]
public partial class ShowCurrentDialogueAction : Action
{
    [SerializeReference] public BlackboardVariable<DialogueSO> Dialogue;
    [SerializeReference] public BlackboardVariable<int> This;
    [SerializeReference] public BlackboardVariable<TextMeshProUGUI> DialogueText; 
    [SerializeReference] public BlackboardVariable<GameObject> Canvas;
    [SerializeReference] public BlackboardVariable<bool> IsDoneTalking;
    [SerializeReference] public BlackboardVariable<DialogueSO> DialogueFromScriptableObject;
    protected override Status OnStart()
    {

        
        Debug.Log($"[ShowCurrentDialogueAction] current list count is {DialogueFromScriptableObject.Value.DialogieInOrder.Capacity.ToString()}");
        if (This.Value >= DialogueFromScriptableObject.Value.DialogieInOrder.Capacity)
        {
            This.Value = 0;
            IsDoneTalking.Value = true;
            return Status.Success;
        }
        
        DialogueText.Value.text = DialogueFromScriptableObject.Value.DialogieInOrder[This.Value];
        Canvas.Value.SetActive(true);
        This.Value++;

        return Status.Success;
    }

}

