using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Machine_Interaction : MonoBehaviour, I_Interactable_Machine
{
    [SerializeField] Machine_Types machine_Type;

    [HideInInspector]public Transform look_At_Target;
    [HideInInspector]public Transform machine_Cam_Transform;
    [HideInInspector]public Transform basket_Position;
    public Transform table_Top_Position;
    [HideInInspector]public bool isBasketLoaded;
    [HideInInspector]public bool isMachineBusy;
    [HideInInspector]public bool isCounterTop;
    [SerializeField] public TMP_Text timeText;
    [SerializeField] public TMP_Text dialText;
    [SerializeField] private GameObject objectToSpawnAtEnd;
    
    [SerializeField] private Transform objectSpawnPosition;
    
    [SerializeField] Material basketTexture;
    [HideInInspector]public bool isBeingUsed;
    
    public Machine_Types Machine_Type => machine_Type;

    private void Start()
    {
        look_At_Target = gameObject.transform.GetChild(1);
        machine_Cam_Transform = gameObject.transform.GetChild(0);
        if (machine_Type is Machine_Types.Laundry_Machine or Machine_Types.Drying_Machine) basket_Position = gameObject.transform.GetChild(2);
    }

    private void Update()
    {
        if (machine_Type is Machine_Types.Cashregister_Machine or Machine_Types.Drying_Machine or Machine_Types.Counter_Top)return;
        basketTexture.color = isBasketLoaded ? Color.black : Color.white;
    }

    #region Interact Switch
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

    public void StartTimer(float waitTime)
    {
        StartCoroutine(TimeToWait(waitTime));
    }
    
    IEnumerator TimeToWait(float waitTime)
    {
        waitTime -= Time.deltaTime;
        timeText.text = $"Time: {(int)waitTime}";
        yield return new WaitForSeconds(waitTime);
        isMachineBusy = false;
        Instantiate(objectToSpawnAtEnd, objectSpawnPosition);
    }
    
}

public enum Machine_Types{
    Laundry_Machine,
    Drying_Machine,
    Cashregister_Machine,
    Counter_Top
}
