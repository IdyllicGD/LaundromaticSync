using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Machine_Interaction_Objects : MonoBehaviour, I_Interactable_Machine_Objects
{
        [SerializeField] Machine_Interactible_Types machine_Interactible_Type;
        public Machine_Interactible_Types Machine_Interactible_Type => machine_Interactible_Type;
        public List<TextMeshProUGUI> machineText = new List<TextMeshProUGUI>();
}


public enum Machine_Interactible_Types
{
    Dial, Lever, Door, MachineBasket
}