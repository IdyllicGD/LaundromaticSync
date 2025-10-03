using System.ComponentModel;
using UnityEngine;

public class InteractionNameData : MonoBehaviour
{
    public bool canBeKicked;
    
    public string itemName;

    void Start()
    {
        if (itemName == "")
        {
            itemName = transform.name;
        }
    }
}
