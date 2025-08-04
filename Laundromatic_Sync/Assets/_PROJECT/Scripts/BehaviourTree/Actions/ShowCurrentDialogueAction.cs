using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using TMPro;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ShowCurrentDialogue", story: "Show next [Dialogue] at [this] int and set [DialogueText] and Enable [Canvas]", category: "Action", id: "91f998e4a039e4258a01e2bd1072e64e")]
public partial class ShowCurrentDialogueAction : Action
{
    [SerializeReference] public BlackboardVariable<DialogueSO> Dialogue;
    [SerializeReference] public BlackboardVariable<int> This;
    [SerializeReference] public BlackboardVariable<TextMeshProUGUI> DialogueText;
    [SerializeReference] public BlackboardVariable<GameObject> Canvas;
    [SerializeReference] public BlackboardVariable<DialogueSO> DialogueFromScriptableObject;
    [SerializeReference] public BlackboardVariable<int> Change;
    protected override Status OnStart()
    {
        List<string> dialogueList = new();
        dialogueList = DialogueFromScriptableObject.Value.DialogieInOrder;
        Canvas.Value.SetActive(true);
        Debug.Log($"[ShowCurrentDialogueAction] current list count is {dialogueList.Capacity}");
        if (This.Value >= dialogueList.Capacity)
        {
            This.Value = 0;
            return Status.Failure;
        }
        DialogueText.Value.text = dialogueList[This.Value];
        This.Value++;

        return Status.Success;
    }

}

