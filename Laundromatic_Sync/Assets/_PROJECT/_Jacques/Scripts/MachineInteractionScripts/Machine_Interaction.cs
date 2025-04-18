using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Machine_Interaction : MonoBehaviour, I_Interactable_Machine
{
    [SerializeField] Machine_Types machine_Type;

    [HideInInspector]public Transform look_At_Target;
    [HideInInspector]public Transform machine_Cam_Transform;
    public Machine_Types Machine_Type => machine_Type;

    private void Start()
    {
        look_At_Target = gameObject.transform.GetChild(1);
        machine_Cam_Transform = gameObject.transform.GetChild(0);
    }

    #region Interact Swithc
    public void Interact()
    {
        switch (machine_Type)
        {
            case Machine_Types.Laundry_Machine:
                LaundryMechanicsStart();
                break;
            case Machine_Types.Drying_Machine:
                DryingMechanicsStart();
                break;
            case Machine_Types.Cashregister_Machine:
                CashRegisterMechanicsStart();
                break;
        }
    }
    
    #endregion

    #region Machine Code Sections

    #region Laundry

    void LaundryMechanicsStart()
    {
        Debug.Log($"[Laundry Machine Mechanics Start]");
    }

    #endregion

    #region Drying

    void DryingMechanicsStart()
    {
        Debug.Log($"[Drying Machine Mechanics Start]");
    }

    #endregion
    
    #region Cashregister

    void CashRegisterMechanicsStart()
    {
        Debug.Log($"[Cash register Machine Mechanics Start]");
    }

    #endregion
    
    #endregion
    
}

public enum Machine_Types{
    Laundry_Machine,
    Drying_Machine,
    Cashregister_Machine
}
