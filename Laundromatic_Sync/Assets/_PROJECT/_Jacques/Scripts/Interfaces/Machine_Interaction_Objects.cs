using System;
using UnityEngine;

public class Machine_Interaction_Objects : MonoBehaviour, I_Interactable_Machine_Objects
{
        [SerializeField] Machine_Interactible_Types machine_Interactible_Type;
        public Machine_Interactible_Types Machine_Interactible_Type => machine_Interactible_Type;
}


public enum Machine_Interactible_Types
{
    Dial, Lever
}