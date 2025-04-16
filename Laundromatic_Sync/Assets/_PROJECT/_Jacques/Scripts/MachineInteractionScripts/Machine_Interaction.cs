using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Machine_Interaction : MonoBehaviour, I_Interactable_Machine
{
    [SerializeField] Machine_Types machine_Type;
    
    public Transform look_At_Target;
    public Transform machine_Cam_Transform;
    public Machine_Types Machine_Type => machine_Type;
    
    
    #region Interact Swithc
    public void Interact()
    {
        switch (machine_Type)
        {
            case Machine_Types.Laundry_Machine:
                LaundryStart();
                break;
            case Machine_Types.Drying_Machine:
                DryingStart();
                break;
            case Machine_Types.Cashregister_Machine:
                CaashregisterStart();
                break;
        }
    }
    
    #endregion

    #region Machine Code Sections

    #region Laundry

    void LaundryStart()
    {
        Debug.Log($"[Laundry Machine Start]");
    }

    #endregion

    #region Drying

    void DryingStart()
    {
        Debug.Log($"[Drying Machine Start]");
    }

    #endregion
    
    #region Cashregister

    void CaashregisterStart()
    {
        Debug.Log($"[Cashregister Machine Start]");
    }

    #endregion
    
    #endregion
    
}

public enum Machine_Types{
    Laundry_Machine,
    Drying_Machine,
    Cashregister_Machine
}
