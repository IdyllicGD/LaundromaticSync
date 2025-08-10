using System.Collections.Generic;
using _PROJECT.Scripts.temp.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class NpcStart : MonoBehaviour
{
    [SerializeField] bool canHaveCleanClothes;
    [SerializeField] bool canHaveDirtyClothes;
    [SerializeField] bool canHaveRandomObject;
    [SerializeField] List<GameObject> orderObjectsCombined;
    [SerializeField] List<GameObject> OrderCleanAndDirty;
    [SerializeField] List<GameObject> orderCleanAndRandom;
    [SerializeField] List<GameObject> orderDirtyAndRandom;
    [SerializeField] GameObject cleanBasket;
    [SerializeField] GameObject dirtyBasket;
    [SerializeField] GameObject randomObject;
    [SerializeField] Transform objectSpawnPosition;
    [SerializeField] DialogueSO npcDialogueSo;

    public NpcFinalOrderTypes orderType;
    public GameObject orderObject;

    void Start()
    {

        orderObject = dirtyBasket;
        orderObject.GetComponent<OrderInformation>().isCleanOrder = true;


        ChangeDialogueText();
        SpawnObject(orderObject);

    }

    void SpawnObject(GameObject objectToSpawn)
    {
        Debug.Log($"[NpcStats] Spawning item {objectToSpawn}");
        Instantiate(objectToSpawn, objectSpawnPosition);
    }

    void ChangeDialogueText()
    {
        npcDialogueSo.DialogieInOrder[0] += $"{orderType}";
        npcDialogueSo.DialogieInOrder[npcDialogueSo.DialogieInOrder.Count - 1] += $"{orderType}";
    }




}
