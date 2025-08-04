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
    [TextArea(3, 50)]
    public List<string> DialogieInOrder = new();

    public NpcOrderTypes orderType;
    GameObject orderObject;

    void Start()
    {
        if (canHaveCleanClothes && canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = orderObjectsCombined[Random.Range(0, orderObjectsCombined.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().orderType;
            SpawnObject(orderObject);
            return;
        }
        if (canHaveCleanClothes && canHaveDirtyClothes && !canHaveRandomObject)
        {
            orderObject = OrderCleanAndDirty[Random.Range(0, OrderCleanAndDirty.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().orderType;
            SpawnObject(orderObject);
            return;
        }
        if (canHaveCleanClothes && !canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = orderCleanAndRandom[Random.Range(0, orderCleanAndRandom.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().orderType;
            SpawnObject(orderObject);
            return;
        }
        if (!canHaveCleanClothes && canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = orderDirtyAndRandom[Random.Range(0, orderDirtyAndRandom.Count)];
            orderType = orderObject.GetComponent<OrderInformation>().orderType;
            SpawnObject(orderObject);
            return;
        }
        if (canHaveCleanClothes && !canHaveDirtyClothes && !canHaveRandomObject)
        {
            orderObject = cleanBasket;
            SpawnObject(cleanBasket);
            return;
        }
        if (!canHaveCleanClothes && canHaveDirtyClothes && !canHaveRandomObject)
        {
            orderObject = dirtyBasket;
            SpawnObject(dirtyBasket);
            return;
        }
        if (!canHaveCleanClothes && !canHaveDirtyClothes && canHaveRandomObject)
        {
            orderObject = randomObject;
            SpawnObject(randomObject);
            return;
        }
    }

    void SpawnObject(GameObject objectToSpawn)
    {
        Debug.Log($"[NpcStats] Spawning item {objectToSpawn}");
        Instantiate(objectToSpawn, objectSpawnPosition);
    }




}
