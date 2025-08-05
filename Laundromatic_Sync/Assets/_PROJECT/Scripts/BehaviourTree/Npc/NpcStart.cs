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
        if (canHaveCleanClothes && canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = orderObjectsCombined[Random.Range(0, orderObjectsCombined.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().finalOrderType;
            Debug.Log("1");
        }
        else if (canHaveCleanClothes && canHaveDirtyClothes && !canHaveRandomObject)
        {
            orderObject = OrderCleanAndDirty[Random.Range(0, OrderCleanAndDirty.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().finalOrderType;
            Debug.Log("2");
        }
        else if (canHaveCleanClothes && !canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = orderCleanAndRandom[Random.Range(0, orderCleanAndRandom.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().finalOrderType;
            Debug.Log("3");
        }
        else if (!canHaveCleanClothes && canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = orderDirtyAndRandom[Random.Range(0, orderDirtyAndRandom.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().finalOrderType;
            Debug.Log("4");
        }
        else if (canHaveCleanClothes && !canHaveDirtyClothes && !canHaveRandomObject)
        {
            orderObject = cleanBasket;
            orderType = orderObject.GetComponent<OrderInformation>().finalOrderType;
            Debug.Log("5");
        }
        else if (!canHaveCleanClothes && canHaveDirtyClothes && !canHaveRandomObject)
        {
            orderObject = dirtyBasket;
            orderType = orderObject.GetComponent<OrderInformation>().finalOrderType;
            Debug.Log("6");

        }
        else if (!canHaveCleanClothes && !canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = randomObject;
            orderType = orderObject.GetComponent<OrderInformation>().finalOrderType;
            Debug.Log("7");
        }

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
        npcDialogueSo.DialogieInOrder[0] = $"Can you pwease take this and give me back {orderType}";
        npcDialogueSo.DialogieInOrder[npcDialogueSo.DialogieInOrder.Count - 1] = $"Remember I want {orderType}, UwU";
    }




}
