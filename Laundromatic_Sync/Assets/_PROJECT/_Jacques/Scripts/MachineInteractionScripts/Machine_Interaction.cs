using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Machine_Interaction : MonoBehaviour, I_Interactable_Machine
{
    [SerializeField] Machine_Types machine_Type;
    public Machine_Types Machine_Type => machine_Type;
    
    
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
