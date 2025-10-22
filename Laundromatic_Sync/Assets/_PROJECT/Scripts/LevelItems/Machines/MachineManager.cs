using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    public List<TimerMachines> machineList = new();


    [Button]
    public void ResetAllMachines()
    {
        for (int i = 0; i < machineList.Count; i++)
        {
            machineList[i].ResetMachine();
            machineList[i].interupt = true;
        }

    }

}
