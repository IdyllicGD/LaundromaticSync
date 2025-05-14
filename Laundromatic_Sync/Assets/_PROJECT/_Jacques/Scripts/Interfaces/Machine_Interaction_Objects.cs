using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Machine_Interaction_Objects : MonoBehaviour, I_Interactable_Machine_Objects
{
        [SerializeField] Machine_Interactible_Types machine_Interactible_Type;
        public Machine_Interactible_Types Machine_Interactible_Type => machine_Interactible_Type;
        public List<TMP_Text> machineText = new List<TMP_Text>();
}


public enum Machine_Interactible_Types
{
    Dial, Lever, Door, MachineBasket
}