using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] public List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] StudioEventEmitter buttonSound;
    public LaundryMachine lm;
    public DryerMachine dm;
    public bool isForLm;
    public int currentStage;

    public void SpawnButton()
    {
        if (currentStage == spawnPoints.Count)
        {
            currentStage = 0;
            return;
        }
        int maxStage = spawnPoints.Count;
        switch (maxStage)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                Instantiate(buttons[Random.Range(0, buttons.Count)], spawnPoints[currentStage].position, Quaternion.identity, spawnPoints[currentStage]);
                if (currentStage != 0) { buttonSound.Play(); }
                currentStage++;
                
                
                break;
        }
    }
}
