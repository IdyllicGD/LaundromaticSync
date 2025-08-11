using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject itemToSpawn;
    [SerializeField] float spawnCheckDelayTime;
    void Start()
    {
        SpawnItem(itemToSpawn);
    }

    void SpawnItem(GameObject itemToSpawn)
    {
        StartCoroutine(TimeToWait(spawnCheckDelayTime));
        if (gameObject.transform.childCount == 0) return;
        Instantiate(itemToSpawn);
    }

    IEnumerator TimeToWait(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnItem(itemToSpawn);
    }
}
