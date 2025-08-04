using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "Scriptable Objects/Dialogue System")]
public class DialogueSO : ScriptableObject
{
    [TextArea(3, 50)]
    public List<string> DialogieInOrder;
    public GameObject basket;
}
