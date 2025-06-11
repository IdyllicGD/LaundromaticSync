using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
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
                currentStage++;
                break;
        }
    }
}
